#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.State;

namespace AutoTerminalScanClassic.Core.UseCases;

internal sealed class SendScanResultOnceUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginConfig config;
    private readonly IPluginLogger logger;
    private readonly ScanState scanState;

    public SendScanResultOnceUseCase(
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

    public SendScanResultOnceResult Execute()
    {
        if (scanState.HasSentChatToday)
        {
            return SendScanResultOnceResult.AlreadySent;
        }

        if (!config.Enabled)
        {
            scanState.MarkSent();
            logger.LogDebug("Not enabled.");
            return SendScanResultOnceResult.Disabled;
        }

        if (scanState.ItemCountOnLevelLoaded == null)
        {
            logger.LogError("itemCountOnLevelLoaded is null.");
            return SendScanResultOnceResult.MissingLevelLoadedScan;
        }
        var itemCountOnLevelLoaded = scanState.ItemCountOnLevelLoaded.Value;

        var itemCountOnHourAdvancedNullable = gameInterop.ScanItemCount();
        if (itemCountOnHourAdvancedNullable == null)
        {
            logger.LogError("itemCountOnHourAdvanced is null.");
            return SendScanResultOnceResult.ScanFailed;
        }
        var itemCountOnHourAdvanced = itemCountOnHourAdvancedNullable.Value;

        var itemCountDifference = itemCountOnHourAdvanced - itemCountOnLevelLoaded;

        logger.LogDebug(
            "Hour advanced scan complete." +
            $" itemCountOnLevelLoaded={itemCountOnLevelLoaded}" +
            $" itemCountOnHourAdvanced={itemCountOnHourAdvanced}" +
            $" itemCountDifference={itemCountDifference}"
        );

        var message = $"{itemCountOnLevelLoaded} {itemCountDifference}";
        var sendChatSuccess = SendChat(target: SelectChatTarget(config.BroadcastMode), message: message);

        if (!sendChatSuccess)
        {
            logger.LogError($"Failed to send chat message. message={message}");
            return SendScanResultOnceResult.SendFailed;
        }
        logger.LogDebug($"Sent chat message successfully. message={message}");

        scanState.MarkSent();
        return SendScanResultOnceResult.Success;
    }

    private ChatSendTarget SelectChatTarget(BroadcastMode broadcastMode)
    {
        if (broadcastMode == BroadcastMode.SelfOnly)
        {
            return ChatSendTarget.SelfOnly;
        }

        if (broadcastMode == BroadcastMode.HostOnly && !gameInterop.IsHost())
        {
            return ChatSendTarget.SelfOnly;
        }

        return ChatSendTarget.Everyone;
    }

    private bool SendChat(ChatSendTarget target, string message)
    {
        return target switch
        {
            ChatSendTarget.SelfOnly => gameInterop.SendChatToSelfOnly(message),
            ChatSendTarget.Everyone => gameInterop.SendChatToEveryone(message),
            _ => false
        };
    }
}
