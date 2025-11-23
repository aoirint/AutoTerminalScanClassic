#nullable enable

using BepInEx.Logging;
using AutoTerminalScanClassic.Utils;

namespace AutoTerminalScanClassic.Managers;

internal class AutoTerminalScanManager
{
    internal static ManualLogSource Logger => AutoTerminalScanClassic.Logger!;

    private int? itemCountOnLevelLoadedNullable;
    private bool hasSentChatToday = false;

    public void ResetAndScanForNewLevel()
    {
        var enabled = AutoTerminalScanClassic.EnabledConfig?.Value ?? true;
        if (!enabled)
        {
            hasSentChatToday = true;
            return;
        }

        itemCountOnLevelLoadedNullable = null;
        hasSentChatToday = false;

        var itemCount = TerminalUtils.ScanItemCount();
        if (itemCount == null)
        {
            Logger.LogError("itemCount is null.");
            return;
        }

        itemCountOnLevelLoadedNullable = itemCount;

        Logger.LogDebug(
            "Level loaded scan complete." +
            $" itemCountOnLevelLoaded={itemCount}"
        );
    }

    public void ScanAndSendChatOnce()
    {
        if (hasSentChatToday)
        {
            return;
        }

        var enabled = AutoTerminalScanClassic.EnabledConfig?.Value ?? true;
        if (!enabled)
        {
            hasSentChatToday = true;
            Logger.LogDebug("Not enabled.");
            return;
        }

        var broadcastMode = AutoTerminalScanClassic.BroadcastModeConfig?.Value ?? BroadcastMode.SelfOnly;

        if (itemCountOnLevelLoadedNullable == null)
        {
            Logger.LogError("itemCountOnLevelLoaded is null.");
            return;
        }
        var itemCountOnLevelLoaded = itemCountOnLevelLoadedNullable.Value;

        var itemCountOnHourAdvancedNullable = TerminalUtils.ScanItemCount();
        if (itemCountOnHourAdvancedNullable == null)
        {
            Logger.LogError("itemCountOnHourAdvanced is null.");
            return;
        }
        var itemCountOnHourAdvanced = itemCountOnHourAdvancedNullable.Value;

        var itemCountDifference = itemCountOnHourAdvanced - itemCountOnLevelLoaded;

        Logger.LogDebug(
            "Hour advanced scan complete. " +
            $" itemCountOnLevelLoaded={itemCountOnLevelLoaded}" +
            $" itemCountOnHourAdvanced={itemCountOnHourAdvanced}" +
            $" itemCountDifference={itemCountDifference}"
        );

        var message = $"{itemCountOnLevelLoaded} {itemCountDifference}";

        bool sendChatSuccess;
        if (broadcastMode == BroadcastMode.SelfOnly)
        {
            sendChatSuccess = ChatUtils.SendChatToSelfOnly(message);
        }
        else if (broadcastMode == BroadcastMode.HostOnly)
        {
            if (!NetworkUtils.IsHost())
            {
                sendChatSuccess = ChatUtils.SendChatToSelfOnly(message);
            }
            else
            {
                sendChatSuccess = ChatUtils.SendChatToEveryone(message);
            }
        }
        else
        {
            sendChatSuccess = ChatUtils.SendChatToEveryone(message);
        }

        if (!sendChatSuccess)
        {
            Logger.LogError($"Failed to send chat message. message={message}");
            return;
        }
        Logger.LogDebug($"Sent chat message successfully. message={message}");

        hasSentChatToday = true;
    }
}
