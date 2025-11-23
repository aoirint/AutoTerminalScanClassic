#nullable enable

using AutoTerminalScanClassic.Utils;
using BepInEx.Logging;
using HarmonyLib;

namespace AutoTerminalScanClassic.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    internal static ManualLogSource Logger => AutoTerminalScanClassic.Logger!;

    [HarmonyPatch(nameof(RoundManager.FinishGeneratingNewLevelClientRpc))]
    [HarmonyPostfix]
    public static void FinishGeneratingNewLevelClientRpcPostfix(RoundManager __instance)
    {
        if (!NetworkUtils.IsServer())
        {
            Logger.LogDebug("Not the server. Skipping FinishGeneratingNewLevelClientRpcPostfix.");
            return;
        }

        var autoTerminalScanManager = AutoTerminalScanClassic.AutoTerminalScanManager;
        if (autoTerminalScanManager == null)
        {
            Logger.LogError("AutoTerminalScanManager is null.");
            return;
        }

        autoTerminalScanManager.ResetAndScanForNewLevel();
    }
}
