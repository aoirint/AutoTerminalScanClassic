#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using BepInEx.Configuration;

namespace AutoTerminalScanClassic.Interop;

/// <summary>
/// Binds BepInEx configuration entries and exposes them through the Core config port.
/// </summary>
/// <remarks>
/// ConfigEntry values stay live, so Core reads the current user configuration
/// without depending on BepInEx types.
/// </remarks>
internal sealed class BepInExPluginConfig : IPluginConfig
{
    private readonly ConfigEntry<bool> enabledConfig;
    private readonly ConfigEntry<BroadcastMode> broadcastModeConfig;
    private readonly ConfigEntry<bool> validationLoggingConfig;

    private BepInExPluginConfig(
        ConfigEntry<bool> enabledConfig,
        ConfigEntry<BroadcastMode> broadcastModeConfig,
        ConfigEntry<bool> validationLoggingConfig
    )
    {
        this.enabledConfig = enabledConfig;
        this.broadcastModeConfig = broadcastModeConfig;
        this.validationLoggingConfig = validationLoggingConfig;
    }

    public bool Enabled => enabledConfig.Value;

    public BroadcastMode BroadcastMode => broadcastModeConfig.Value;

    public bool ValidationLogging => validationLoggingConfig.Value;

    /// <summary>
    /// Creates the plugin configuration entries used by the scan workflow.
    /// </summary>
    public static BepInExPluginConfig Bind(ConfigFile config)
    {
        var enabledConfig = config.Bind(
            "General",
            "Enabled",
            true,
            "Set to false to disable this mod."
        );

        // BroadcastMode names are intentionally described in the config text
        // because the enum is the user's stable behavior-selection surface.
        var broadcastModeConfig = config.Bind(
            "General",
            "BroadcastMode",
            BroadcastMode.SelfOnly,
            "Controls whether this mod sends scan results to other players." +
            " If SelfOnly, you can still see scan results but not send to other players." +
            " If HostOnly, you send scan results to other players only when you are the host." +
            " If Always, you always send scan results to other players."
        );

        // Keep validation logging opt-in because it is intended for focused
        // release checks and produces machine-readable lines in normal logs.
        var validationLoggingConfig = config.Bind(
            "Debug",
            "ValidationLogging",
            false,
            "Enable structured validation logs for release validation and troubleshooting."
        );

        return new BepInExPluginConfig(
            enabledConfig: enabledConfig,
            broadcastModeConfig: broadcastModeConfig,
            validationLoggingConfig: validationLoggingConfig
        );
    }
}
