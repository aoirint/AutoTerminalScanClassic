#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using BepInEx.Configuration;

namespace AutoTerminalScanClassic.Interop;

internal sealed class BepInExPluginConfig : IPluginConfig
{
    private readonly ConfigEntry<bool> enabledConfig;
    private readonly ConfigEntry<BroadcastMode> broadcastModeConfig;

    private BepInExPluginConfig(
        ConfigEntry<bool> enabledConfig,
        ConfigEntry<BroadcastMode> broadcastModeConfig
    )
    {
        this.enabledConfig = enabledConfig;
        this.broadcastModeConfig = broadcastModeConfig;
    }

    public bool Enabled => enabledConfig.Value;

    public BroadcastMode BroadcastMode => broadcastModeConfig.Value;

    public static BepInExPluginConfig Bind(ConfigFile config)
    {
        var enabledConfig = config.Bind(
            "General",
            "Enabled",
            true,
            "Set to false to disable this mod."
        );

        var broadcastModeConfig = config.Bind(
            "General",
            "BroadcastMode",
            BroadcastMode.SelfOnly,
            "Controls whether this mod sends scan results to other players." +
            " If SelfOnly, you can still see scan results but not send to other players." +
            " If HostOnly, you send scan results to other players only when you are the host." +
            " If Always, you always send scan results to other players."
        );

        return new BepInExPluginConfig(
            enabledConfig: enabledConfig,
            broadcastModeConfig: broadcastModeConfig
        );
    }
}
