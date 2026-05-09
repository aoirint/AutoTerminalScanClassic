#nullable enable

namespace AutoTerminalScanClassic.Core.Ports;

/// <summary>
/// Names the game operations the scan workflow needs without exposing Unity objects.
/// </summary>
/// <remarks>
/// Core works with counts, role checks, and chat operations; Interop owns the
/// base-game singletons, HUD APIs, and object scans behind this boundary.
/// </remarks>
internal interface IGameInterop
{
    /// <summary>
    /// Returns whether the current network role should run client-side scan callbacks.
    /// </summary>
    bool IsClient();

    /// <summary>
    /// Returns whether BroadcastMode.HostOnly may broadcast to everyone.
    /// </summary>
    bool IsHost();

    /// <summary>
    /// Counts scrap items that the base game's terminal scan would report.
    /// </summary>
    int? ScanItemCount();

    /// <summary>
    /// Writes the scan result only to the local player's HUD chat.
    /// </summary>
    bool SendChatToSelfOnly(string message);

    /// <summary>
    /// Sends the scan result through the base-game server chat path.
    /// </summary>
    bool SendChatToEveryone(string message);
}
