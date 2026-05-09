#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.State;

namespace AutoTerminalScanClassic.Core.UseCases;

internal sealed class ResetScanForNewLevelUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginConfig config;
    private readonly IPluginLogger logger;
    private readonly ScanState scanState;

    public ResetScanForNewLevelUseCase(
        IGameInterop gameInterop,
        IPluginConfig config,
        IPluginLogger logger,
        ScanState scanState
    )
    {
        this.gameInterop = gameInterop;
        this.config = config;
        this.logger = logger;
        this.scanState = scanState;
    }

    public ResetScanForNewLevelResult Execute()
    {
        if (!config.Enabled)
        {
            scanState.MarkSent();
            return ResetScanForNewLevelResult.Disabled;
        }

        scanState.ResetForNewLevel();

        var itemCount = gameInterop.ScanItemCount();
        if (itemCount == null)
        {
            logger.LogError("itemCount is null.");
            return ResetScanForNewLevelResult.ScanFailed;
        }

        scanState.RecordLevelLoadedItemCount(itemCount.Value);

        logger.LogDebug(
            "Level loaded scan complete." +
            $" itemCountOnLevelLoaded={itemCount}"
        );
        return ResetScanForNewLevelResult.Success;
    }
}
