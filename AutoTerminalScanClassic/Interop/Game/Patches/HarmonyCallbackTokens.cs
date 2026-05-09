#nullable enable

namespace AutoTerminalScanClassic.Interop.Game.Patches;

/// <summary>
/// Stable identifiers for Harmony callback diagnostics.
/// </summary>
internal static class HarmonyCallbackTokens
{
    // Tokens are validation identifiers, not display strings. Keep them tied to
    // patched base-game methods so diagnostic filters survive message wording changes.
    public const string RoundManagerFinishGeneratingNewLevelClientRpcPostfix =
        "round_manager.finish_generating_new_level_client_rpc.postfix";

    public const string TimeOfDayMoveTimeOfDayPostfix =
        "time_of_day.move_time_of_day.postfix";
}
