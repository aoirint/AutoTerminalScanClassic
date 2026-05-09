#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.State;
using AutoTerminalScanClassic.Core.Validation;

namespace AutoTerminalScanClassic.Core.UseCases;

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

    public ResetScanForNewLevelResult Execute()
    {
        if (!config.Enabled)
        {
            scanState.MarkSent();
            validationLogger.Record(
                ValidationLogRecord.LevelLoadedScanResult(ValidationLogScanResult.Disabled)
            );
            return ResetScanForNewLevelResult.Disabled;
        }

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
