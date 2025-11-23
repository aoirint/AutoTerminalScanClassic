#nullable enable

using AutoTerminalScanClassic.Utils;
using BepInEx.Logging;
using HarmonyLib;

namespace AutoTerminalScanClassic.Patches;

[HarmonyPatch(typeof(TimeOfDay))]
internal class TimeOfDayPatch
{
    internal static ManualLogSource Logger => AutoTerminalScanClassic.Logger!;

    [HarmonyPatch(nameof(TimeOfDay.MoveTimeOfDay))]
    [HarmonyPostfix]
    public static void MoveTimeOfDayPostfix(TimeOfDay __instance)
    {
        if (!NetworkUtils.IsServer())
        {
            Logger.LogDebug("Not the server. Skipping AdvanceHourAndSpawnNewBatchOfEnemiesPostfix.");
            return;
        }

        // Wait one frame to ensure the hive and eggs are spawned.
        var elapsedGlobalTime = __instance.globalTime - 100f;
        var globalTimeSpeedMultiplier = __instance.globalTimeSpeedMultiplier;
        if (elapsedGlobalTime < globalTimeSpeedMultiplier)
        {
            return;
        }

        var autoTerminalScanManager = AutoTerminalScanClassic.AutoTerminalScanManager;
        if (autoTerminalScanManager == null)
        {
            Logger.LogError("AutoTerminalScanManager is null.");
            return;
        }

        autoTerminalScanManager.ScanAndSendChatOnce();
    }
}
