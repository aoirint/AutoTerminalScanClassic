#nullable enable

namespace AutoTerminalScanClassic.Core.State;

/// <summary>
/// Mutable per-level state for the classic two-scan workflow.
/// </summary>
/// <remarks>
/// The state is intentionally small: one baseline count and one completion flag
/// are enough to preserve the original manager behavior across separate game
/// callbacks.
/// </remarks>
internal sealed class ScanState
{
    private int? itemCountOnLevelLoaded;

    /// <summary>
    /// Whether the current level should ignore further delayed send callbacks.
    /// </summary>
    public bool HasSentChatToday { get; private set; }

    /// <summary>
    /// Item count captured immediately after the level generation callback.
    /// </summary>
    public int? ItemCountOnLevelLoaded => itemCountOnLevelLoaded;

    /// <summary>
    /// Marks the current level as complete for chat-send purposes.
    /// </summary>
    public void MarkSent()
    {
        HasSentChatToday = true;
    }

    /// <summary>
    /// Clears scan state for a newly generated level.
    /// </summary>
    public void ResetForNewLevel()
    {
        itemCountOnLevelLoaded = null;
        HasSentChatToday = false;
    }

    /// <summary>
    /// Stores the baseline count used by the later scan-delta calculation.
    /// </summary>
    public void RecordLevelLoadedItemCount(int itemCount)
    {
        itemCountOnLevelLoaded = itemCount;
    }
}
