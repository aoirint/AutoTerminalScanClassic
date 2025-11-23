#nullable enable

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using AutoTerminalScanClassic.Generated;
using AutoTerminalScanClassic.Managers;
using BepInEx.Configuration;

namespace AutoTerminalScanClassic;

public enum BroadcastMode
{
    SelfOnly,
    HostOnly,
    Everyone
}

[BepInPlugin(ModInfo.GUID, ModInfo.NAME, ModInfo.VERSION)]
[BepInProcess("Lethal Company.exe")]
public class AutoTerminalScanClassic : BaseUnityPlugin
{
    internal static new ManualLogSource? Logger { get; private set; }

    internal static Harmony harmony = new(ModInfo.GUID);

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
            "Controls whether this mod sends scan results to other players."
        );

        harmony.PatchAll();

        Logger.LogInfo($"Plugin {ModInfo.NAME} v{ModInfo.VERSION} is loaded!");
    }
}
