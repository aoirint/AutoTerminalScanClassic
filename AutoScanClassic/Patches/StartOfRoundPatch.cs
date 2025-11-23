#nullable enable

using BepInEx.Logging;
using HarmonyLib;
using AutoScanClassic.Utils;

namespace AutoScanClassic.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal class StartOfRoundPatch
{
    internal static ManualLogSource Logger => AutoScanClassic.Logger!;

    [HarmonyPatch(nameof(StartOfRound.ResetShip))]
    [HarmonyPostfix]
    public static void ResetShipPostfix(StartOfRound __instance)
    {
        if (!NetworkUtils.IsServer())
        {
            Logger.LogDebug("Not the server. Skipping ResetShipPostfix.");
            return;
        }

        // TODO: Reset scrap count memory
    }
}
