#nullable enable

namespace AutoTerminalScanClassic.Core.Ports;

internal interface IGameInterop
{
    bool IsClient();

    bool IsHost();

    int? ScanItemCount();

    bool SendChatToSelfOnly(string message);

    bool SendChatToEveryone(string message);
}
