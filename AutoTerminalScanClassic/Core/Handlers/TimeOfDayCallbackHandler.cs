#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.UseCases;

namespace AutoTerminalScanClassic.Core.Handlers;

/// <summary>
/// Coordinates delayed time-of-day callbacks with the scan-result send use case.
/// </summary>
/// <remarks>
/// The Harmony patch owns the elapsed-time gate; this handler owns client-role
/// validation and dispatch into Core scan policy.
/// </remarks>
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

    /// <summary>
    /// Handles TimeOfDay.MoveTimeOfDay after the patch has allowed the first delayed tick.
    /// </summary>
    public void HandleMoveTimeOfDay()
    {
        if (!gameInterop.IsClient())
        {
            // Chat presentation is client-facing, so non-client or unavailable
            // network state stops before any scan or chat side effects happen.
            logger.LogDebug("Not the client. Skipping MoveTimeOfDayPostfix.");
            return;
        }

        sendScanResultOnceUseCase.Execute();
    }
}
