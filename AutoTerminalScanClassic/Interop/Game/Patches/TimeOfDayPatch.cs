#nullable enable

using HarmonyLib;

namespace AutoTerminalScanClassic.Interop.Game.Patches;

[HarmonyPatch(typeof(TimeOfDay))]
internal static class TimeOfDayPatch
{
    /// <summary>
    /// Sends the delayed scan delta after base-game time movement reaches the classic gate.
    /// </summary>
    [HarmonyPatch(nameof(TimeOfDay.MoveTimeOfDay))]
    [HarmonyPostfix]
    public static void MoveTimeOfDayPostfix(TimeOfDay __instance)
    {
        // Wait one frame to ensure the hive and eggs are spawned before the
        // second scan. The 100f offset preserves the original manager-era gate.
        var elapsedGlobalTime = __instance.globalTime - 100f;
        var globalTimeSpeedMultiplier = __instance.globalTimeSpeedMultiplier;
        if (elapsedGlobalTime < globalTimeSpeedMultiplier)
        {
            return;
        }

        // Keep the patch as a timing gate only; the controller/handler path
        // owns role checks, scan comparison, and chat routing. The guard wraps
        // only the callback handoff so failed diagnostics cannot change timing.
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.TimeOfDayMoveTimeOfDayPostfix,
            notify: AutoTerminalScanClassic.Controller.HandleMoveTimeOfDay
        );
    }
}
