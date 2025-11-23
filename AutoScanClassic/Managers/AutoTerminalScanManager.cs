#nullable enable

using BepInEx.Logging;
using AutoScanClassic.Utils;

namespace AutoScanClassic.Managers;

internal class AutoTerminalScanManager
{
    internal static ManualLogSource Logger => AutoScanClassic.Logger!;

    private int? itemCountOnLevelLoadedNullable;
    private bool hasSentChatToday = false;

    public void ResetAndScanForNewLevel()
    {
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
            Logger.LogDebug("Already sent chat today. Skipping.");
            return;
        }

        if (itemCountOnLevelLoadedNullable == null)
        {
            Logger.LogError("itemCountOnLevelLoaded is null. Aborting ScanAndSendChatOnHourAdvanced.");
            return;
        }
        var itemCountOnLevelLoaded = itemCountOnLevelLoadedNullable.Value;

        var itemCountOnHourAdvancedNullable = TerminalUtils.ScanItemCount();
        if (itemCountOnHourAdvancedNullable == null)
        {
            Logger.LogError("itemCountOnHourAdvanced is null. Aborting ScanAndSendChatOnHourAdvanced.");
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
        var sendChatSuccess = ChatUtils.SendChatToAllAsLocalPlayer(message);
        if (!sendChatSuccess)
        {
            Logger.LogError($"Failed to send chat message. message={message}");
            return;
        }
        Logger.LogDebug($"Sent chat message successfully. message={message}");

        hasSentChatToday = true;
    }
}
