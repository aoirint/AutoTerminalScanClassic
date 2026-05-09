#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.State;
using AutoTerminalScanClassic.Core.Validation;

namespace AutoTerminalScanClassic.Core.UseCases;

/// <summary>
/// Starts the per-level scan workflow by recording the level-load item count.
/// </summary>
/// <remarks>
/// This use case owns the first scan in the classic two-scan calculation; the
/// later time-of-day use case consumes the stored count to compute the delta.
/// </remarks>
internal sealed class ResetScanForNewLevelUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginConfig config;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;
    private readonly ScanState scanState;

    public ResetScanForNewLevelUseCase(
        IGameInterop gameInterop,
        IPluginConfig config,
        IPluginLogger logger,
        IValidationLogger validationLogger,
        ScanState scanState
    )
    {
        this.gameInterop = gameInterop;
        this.config = config;
        this.logger = logger;
        this.validationLogger = validationLogger;
        this.scanState = scanState;
    }

    /// <summary>
    /// Clears previous level state and captures the scan baseline for this level.
    /// </summary>
    public ResetScanForNewLevelResult Execute()
    {
        if (!config.Enabled)
        {
            // Disabled mode is treated as already sent so later time callbacks
            // stay quiet until the next level-load callback resets the decision.
            scanState.MarkSent();
            validationLogger.Record(
                ValidationLogRecord.LevelLoadedScanResult(ValidationLogScanResult.Disabled)
            );
            return ResetScanForNewLevelResult.Disabled;
        }

        // Clear the previous baseline before scanning. If the scan fails, the
        // send use case can detect the missing baseline instead of reusing an
        // old level's count.
        scanState.ResetForNewLevel();

        var itemCount = gameInterop.ScanItemCount();
        if (itemCount == null)
        {
            logger.LogError("itemCount is null.");
            validationLogger.Record(
                ValidationLogRecord.LevelLoadedScanResult(ValidationLogScanResult.ScanFailed)
            );
            return ResetScanForNewLevelResult.ScanFailed;
        }

        scanState.RecordLevelLoadedItemCount(itemCount.Value);

        // Keep the baseline visible in debug logs because it is one half of the
        // user-facing compact chat message sent later in the level.
        logger.LogDebug(
            "Level loaded scan complete." +
            $" itemCountOnLevelLoaded={itemCount}"
        );
        validationLogger.Record(
            ValidationLogRecord.LevelLoadedScanResult(
                result: ValidationLogScanResult.Success,
                itemCount: itemCount.Value
            )
        );
        return ResetScanForNewLevelResult.Success;
    }
}
