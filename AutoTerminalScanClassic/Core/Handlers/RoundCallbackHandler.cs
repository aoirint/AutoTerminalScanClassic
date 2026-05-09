#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.UseCases;
using AutoTerminalScanClassic.Core.Validation;

namespace AutoTerminalScanClassic.Core.Handlers;

/// <summary>
/// Coordinates round lifecycle callbacks with the level-load scan use case.
/// </summary>
/// <remarks>
/// Interop detects the base-game RPC timing, while Core decides whether this
/// client should capture the first scan baseline for the new level.
/// </remarks>
internal sealed class RoundCallbackHandler
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;
    private readonly ResetScanForNewLevelUseCase resetScanForNewLevelUseCase;

    public RoundCallbackHandler(
        IGameInterop gameInterop,
        IPluginLogger logger,
        IValidationLogger validationLogger,
        ResetScanForNewLevelUseCase resetScanForNewLevelUseCase
    )
    {
        this.gameInterop = gameInterop;
        this.logger = logger;
        this.validationLogger = validationLogger;
        this.resetScanForNewLevelUseCase = resetScanForNewLevelUseCase;
    }

    /// <summary>
    /// Handles RoundManager.FinishGeneratingNewLevelClientRpc after the base game finishes.
    /// </summary>
    public void HandleFinishGeneratingNewLevelClientRpc()
    {
        if (!gameInterop.IsClient())
        {
            // The original patch only needs client-side HUD/scan state. Missing
            // or non-client network state fails closed at this callback boundary.
            logger.LogDebug("Not the client. Skipping FinishGeneratingNewLevelClientRpcPostfix.");
            validationLogger.Record(
                ValidationLogRecord.LevelLoadedScanResult(ValidationLogScanResult.NotClient)
            );
            return;
        }

        resetScanForNewLevelUseCase.Execute();
    }
}
