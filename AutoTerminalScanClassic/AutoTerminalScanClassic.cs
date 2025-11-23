#nullable enable

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using AutoTerminalScanClassic.Generated;
using AutoTerminalScanClassic.Managers;

namespace AutoTerminalScanClassic;

[BepInPlugin(ModInfo.GUID, ModInfo.NAME, ModInfo.VERSION)]
[BepInProcess("Lethal Company.exe")]
public class AutoTerminalScanClassic : BaseUnityPlugin
{
    internal static new ManualLogSource? Logger { get; private set; }

    internal static Harmony harmony = new(ModInfo.GUID);

    internal static AutoTerminalScanManager AutoTerminalScanManager { get; } = new();

    private void Awake()
    {
        Logger = base.Logger;

        harmony.PatchAll();

        Logger.LogInfo($"Plugin {ModInfo.NAME} v{ModInfo.VERSION} is loaded!");
    }
}
