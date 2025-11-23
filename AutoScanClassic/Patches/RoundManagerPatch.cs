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

        // TODO: Scan logic
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

        // TODO: Scan logic

        // TODO: Send chat logic
    }
}
