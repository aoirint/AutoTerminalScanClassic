#nullable enable

using HarmonyLib;

namespace AutoTerminalScanClassic.Interop.Game.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal static class RoundManagerPatch
{
    [HarmonyPatch(nameof(RoundManager.FinishGeneratingNewLevelClientRpc))]
    [HarmonyPostfix]
    public static void FinishGeneratingNewLevelClientRpcPostfix()
    {
        AutoTerminalScanClassic.Controller.HandleFinishGeneratingNewLevelClientRpc();
    }
}
