#nullable enable

namespace AutoTerminalScanClassic.Core.Ports;

/// <summary>
/// Names the plugin configuration values used by Core scan policy.
/// </summary>
/// <remarks>
/// Core depends on this port instead of BepInEx config entries so use cases can
/// stay focused on policy and not configuration binding.
/// </remarks>
internal interface IPluginConfig
{
    /// <summary>
    /// Whether scan capture and chat sending should run for new levels.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// User-selected routing policy for the scan-result chat message.
    /// </summary>
    BroadcastMode BroadcastMode { get; }
}
