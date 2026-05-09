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
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.RoundManagerFinishGeneratingNewLevelClientRpcPostfix,
            notify: AutoTerminalScanClassic.Controller.HandleFinishGeneratingNewLevelClientRpc
        );
    }
}
