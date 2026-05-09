#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.UseCases;

namespace AutoTerminalScanClassic.Core.Handlers;

internal sealed class TimeOfDayCallbackHandler
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginLogger logger;
    private readonly SendScanResultOnceUseCase sendScanResultOnceUseCase;

    public TimeOfDayCallbackHandler(
        IGameInterop gameInterop,
        IPluginLogger logger,
        SendScanResultOnceUseCase sendScanResultOnceUseCase
    )
    {
        this.gameInterop = gameInterop;
        this.logger = logger;
        this.sendScanResultOnceUseCase = sendScanResultOnceUseCase;
    }

    public void HandleMoveTimeOfDay()
    {
        if (!gameInterop.IsClient())
        {
            logger.LogDebug("Not the client. Skipping MoveTimeOfDayPostfix.");
            return;
        }

        sendScanResultOnceUseCase.Execute();
    }
}
