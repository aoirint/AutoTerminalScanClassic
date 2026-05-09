#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.UseCases;

namespace AutoTerminalScanClassic.Core.Handlers;

internal sealed class RoundCallbackHandler
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginLogger logger;
    private readonly ResetScanForNewLevelUseCase resetScanForNewLevelUseCase;

    public RoundCallbackHandler(
        IGameInterop gameInterop,
        IPluginLogger logger,
        ResetScanForNewLevelUseCase resetScanForNewLevelUseCase
    )
    {
        this.gameInterop = gameInterop;
        this.logger = logger;
        this.resetScanForNewLevelUseCase = resetScanForNewLevelUseCase;
    }

    public void HandleFinishGeneratingNewLevelClientRpc()
    {
        if (!gameInterop.IsClient())
        {
            logger.LogDebug("Not the client. Skipping FinishGeneratingNewLevelClientRpcPostfix.");
            return;
        }

        resetScanForNewLevelUseCase.Execute();
    }
}
