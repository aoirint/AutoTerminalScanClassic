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
/// <remarks>
/// Core handlers own scan policy and state transitions behind this boundary;
/// patches only translate base-game callback timing into controller calls.
/// </remarks>
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

        // One ScanState instance spans the level-load and time-advance
        // callbacks so the second scan can compare against the first scan and
        // still send only once per level.
        var scanState = new ScanState();

        // Manual wiring is grouped by state lifetime: the shared scan state,
        // use cases that mutate it, then handlers that expose callback-shaped
        // entrypoints to the plugin facade.
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

    /// <summary>
    /// Handles the base-game level-generation completion callback.
    /// </summary>
    public void HandleFinishGeneratingNewLevelClientRpc()
    {
        roundCallbackHandler.HandleFinishGeneratingNewLevelClientRpc();
    }

    /// <summary>
    /// Handles the base-game time-of-day movement callback after the patch timing gate.
    /// </summary>
    public void HandleMoveTimeOfDay()
    {
        timeOfDayCallbackHandler.HandleMoveTimeOfDay();
    }
}
