#nullable enable

using HarmonyLib;

namespace AutoTerminalScanClassic.Interop.Game.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal static class RoundManagerPatch
{
    /// <summary>
    /// Captures the first scan count after the base game finishes generating a level.
    /// </summary>
    [HarmonyPatch(nameof(RoundManager.FinishGeneratingNewLevelClientRpc))]
    [HarmonyPostfix]
    public static void FinishGeneratingNewLevelClientRpcPostfix()
    {
        // Keep the patch thin: Harmony owns method binding and Core owns the
        // client-role check plus scan-state mutation. The guard only wraps the
        // callback boundary so diagnostics do not leak into scan policy.
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.RoundManagerFinishGeneratingNewLevelClientRpcPostfix,
            notify: AutoTerminalScanClassic.Controller.HandleFinishGeneratingNewLevelClientRpc
        );
    }
}
