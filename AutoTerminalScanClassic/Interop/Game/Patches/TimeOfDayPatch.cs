#nullable enable

using HarmonyLib;

namespace AutoTerminalScanClassic.Interop.Game.Patches;

[HarmonyPatch(typeof(TimeOfDay))]
internal static class TimeOfDayPatch
{
    [HarmonyPatch(nameof(TimeOfDay.MoveTimeOfDay))]
    [HarmonyPostfix]
    public static void MoveTimeOfDayPostfix(TimeOfDay __instance)
    {
        // Wait one frame to ensure the hive and eggs are spawned.
        var elapsedGlobalTime = __instance.globalTime - 100f;
        var globalTimeSpeedMultiplier = __instance.globalTimeSpeedMultiplier;
        if (elapsedGlobalTime < globalTimeSpeedMultiplier)
        {
            return;
        }

        AutoTerminalScanClassic.Controller.HandleMoveTimeOfDay();
    }
}
