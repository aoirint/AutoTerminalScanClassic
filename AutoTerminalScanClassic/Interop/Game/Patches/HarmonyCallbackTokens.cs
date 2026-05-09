#nullable enable

namespace AutoTerminalScanClassic.Interop.Game.Patches;

internal static class HarmonyCallbackTokens
{
    public const string RoundManagerFinishGeneratingNewLevelClientRpcPostfix =
        "round_manager.finish_generating_new_level_client_rpc.postfix";

    public const string TimeOfDayMoveTimeOfDayPostfix =
        "time_of_day.move_time_of_day.postfix";
}
