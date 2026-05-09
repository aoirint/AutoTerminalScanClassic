#nullable enable

using AutoTerminalScanClassic.Core.Handlers;
using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.State;
using AutoTerminalScanClassic.Core.UseCases;
using AutoTerminalScanClassic.Interop.Game;

namespace AutoTerminalScanClassic;

/// <summary>
/// Plugin-facing facade for game callbacks reported by Harmony patches.
/// </summary>
internal sealed class PluginController
{
    private readonly RoundCallbackHandler roundCallbackHandler;
    private readonly TimeOfDayCallbackHandler timeOfDayCallbackHandler;

    private PluginController(
        RoundCallbackHandler roundCallbackHandler,
        TimeOfDayCallbackHandler timeOfDayCallbackHandler
    )
    {
        this.roundCallbackHandler = roundCallbackHandler;
        this.timeOfDayCallbackHandler = timeOfDayCallbackHandler;
    }

    /// <summary>
    /// Builds the plugin controller and manually wires concrete integrations to
    /// Core ports.
    /// </summary>
    public static PluginController Create(IPluginConfig config, IPluginLogger logger)
    {
        IGameInterop gameInterop = new GameInterop(logger);
        var scanState = new ScanState();

        var resetScanForNewLevelUseCase = new ResetScanForNewLevelUseCase(
            gameInterop: gameInterop,
            config: config,
            logger: logger,
            scanState: scanState
        );
        var sendScanResultOnceUseCase = new SendScanResultOnceUseCase(
            gameInterop: gameInterop,
            config: config,
            logger: logger,
            scanState: scanState
        );

        return new PluginController(
            roundCallbackHandler: new RoundCallbackHandler(
                gameInterop: gameInterop,
                logger: logger,
                resetScanForNewLevelUseCase: resetScanForNewLevelUseCase
            ),
            timeOfDayCallbackHandler: new TimeOfDayCallbackHandler(
                gameInterop: gameInterop,
                logger: logger,
                sendScanResultOnceUseCase: sendScanResultOnceUseCase
            )
        );
    }

    public void HandleFinishGeneratingNewLevelClientRpc()
    {
        roundCallbackHandler.HandleFinishGeneratingNewLevelClientRpc();
    }

    public void HandleMoveTimeOfDay()
    {
        timeOfDayCallbackHandler.HandleMoveTimeOfDay();
    }
}
