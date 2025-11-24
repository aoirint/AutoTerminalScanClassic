#nullable enable

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using AutoTerminalScanClassic.Managers;
using BepInEx.Configuration;

namespace AutoTerminalScanClassic;

public enum BroadcastMode
{
    SelfOnly,
    HostOnly,
    Always
}

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Lethal Company.exe")]
public class AutoTerminalScanClassic : BaseUnityPlugin
{
    internal static new ManualLogSource? Logger { get; private set; }

    internal static Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

    internal static AutoTerminalScanManager AutoTerminalScanManager { get; } = new();

    internal static ConfigEntry<bool>? EnabledConfig { get; private set; }

    internal static ConfigEntry<BroadcastMode>? BroadcastModeConfig { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;

        EnabledConfig = Config.Bind(
            "General",
            "Enabled",
            true,
            "Set to false to disable this mod."
        );

        BroadcastModeConfig = Config.Bind(
            "General",
            "BroadcastMode",
            BroadcastMode.SelfOnly,
            "Controls whether this mod sends scan results to other players." +
            " If SelfOnly, you can still see scan results but not send to other players." +
            " If HostOnly, you send scan results to other players only when you are the host." +
            " If Always, you always send scan results to other players."
        );

        harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
