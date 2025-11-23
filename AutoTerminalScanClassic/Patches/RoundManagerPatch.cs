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
        if (!NetworkUtils.IsClient())
        {
            Logger.LogDebug("Not the client. Skipping FinishGeneratingNewLevelClientRpcPostfix.");
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
