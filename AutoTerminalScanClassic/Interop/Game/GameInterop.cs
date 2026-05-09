#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Interop.Game.Adapters;

namespace AutoTerminalScanClassic.Interop.Game;

/// <summary>
/// Game-facing implementation of the mod operations requested by Core.
/// </summary>
/// <remarks>
/// Presents one ATSC-oriented surface while focused adapters handle networking,
/// terminal scan counting, and chat delivery.
/// </remarks>
internal sealed class GameInterop : IGameInterop
{
    private readonly NetworkAdapter networkAdapter;
    private readonly TerminalScanAdapter terminalScanAdapter;
    private readonly ChatAdapter chatAdapter;

    public GameInterop(IPluginLogger logger)
    {
        // Keep adapters split by Unity responsibility so Core behavior can be
        // reviewed independently from singleton lookup and HUD/chat mechanics.
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
