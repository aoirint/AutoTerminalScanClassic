#nullable enable

namespace AutoTerminalScanClassic.Core.Ports;

internal interface IPluginConfig
{
    bool Enabled { get; }

    BroadcastMode BroadcastMode { get; }

    bool ValidationLogging { get; }
}
