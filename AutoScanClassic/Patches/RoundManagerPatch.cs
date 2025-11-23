#nullable enable

using AutoScanClassic.Utils;
using BepInEx.Logging;
using HarmonyLib;

namespace AutoScanClassic.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    internal static ManualLogSource Logger => AutoScanClassic.Logger!;

    [HarmonyPatch(nameof(RoundManager.FinishGeneratingNewLevelClientRpc))]
    [HarmonyPostfix]
    public static void FinishGeneratingNewLevelClientRpcPostfix(RoundManager __instance)
    {
        if (!NetworkUtils.IsServer())
        {
            Logger.LogDebug("Not the server. Skipping FinishGeneratingNewLevelClientRpcPostfix.");
            return;
        }

        var autoTerminalScanManager = AutoScanClassic.AutoTerminalScanManager;
        if (autoTerminalScanManager == null)
        {
            Logger.LogError("AutoTerminalScanManager is null.");
            return;
        }

        autoTerminalScanManager.ResetAndScanForNewLevel();
    }

    [HarmonyPatch(nameof(RoundManager.AdvanceHourAndSpawnNewBatchOfEnemies))]
    [HarmonyPostfix]
    public static void AdvanceHourAndSpawnNewBatchOfEnemiesPostfix(RoundManager __instance)
    {
        if (!NetworkUtils.IsServer())
        {
            Logger.LogDebug("Not the server. Skipping AdvanceHourAndSpawnNewBatchOfEnemiesPostfix.");
            return;
        }

        var autoTerminalScanManager = AutoScanClassic.AutoTerminalScanManager;
        if (autoTerminalScanManager == null)
        {
            Logger.LogError("AutoTerminalScanManager is null.");
            return;
        }

        autoTerminalScanManager.ScanAndSendChatOnce();
    }
}
