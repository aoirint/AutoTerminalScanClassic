#nullable enable

namespace AutoTerminalScanClassic.Core.State;

internal sealed class ScanState
{
    private int? itemCountOnLevelLoaded;

    public bool HasSentChatToday { get; private set; }

    public int? ItemCountOnLevelLoaded => itemCountOnLevelLoaded;

    public void MarkSent()
    {
        HasSentChatToday = true;
    }

    public void ResetForNewLevel()
    {
        itemCountOnLevelLoaded = null;
        HasSentChatToday = false;
    }

    public void RecordLevelLoadedItemCount(int itemCount)
    {
        itemCountOnLevelLoaded = itemCount;
    }
}
