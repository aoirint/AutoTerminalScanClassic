#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Interop.Game.Adapters;

namespace AutoTerminalScanClassic.Interop.Game;

internal sealed class GameInterop : IGameInterop
{
    private readonly NetworkAdapter networkAdapter;
    private readonly TerminalScanAdapter terminalScanAdapter;
    private readonly ChatAdapter chatAdapter;

    public GameInterop(IPluginLogger logger)
    {
        networkAdapter = new NetworkAdapter(logger);
        terminalScanAdapter = new TerminalScanAdapter(logger);
        chatAdapter = new ChatAdapter(logger);
    }

    public bool IsClient()
    {
        return networkAdapter.IsClient();
    }

    public bool IsHost()
    {
        return networkAdapter.IsHost();
    }

    public int? ScanItemCount()
    {
        return terminalScanAdapter.ScanItemCount();
    }

    public bool SendChatToSelfOnly(string message)
    {
        return chatAdapter.SendChatToSelfOnly(message);
    }

    public bool SendChatToEveryone(string message)
    {
        return chatAdapter.SendChatToEveryone(message);
    }
}
