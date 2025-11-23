#nullable enable

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using AutoScanClassic.Generated;

namespace AutoScanClassic;

[BepInPlugin(ModInfo.GUID, ModInfo.NAME, ModInfo.VERSION)]
[BepInProcess("Lethal Company.exe")]
public class AutoScanClassic : BaseUnityPlugin
{
    internal static new ManualLogSource? Logger { get; private set; }

    internal static Harmony harmony = new(ModInfo.GUID);

    private void Awake()
    {
        Logger = base.Logger;

        harmony.PatchAll();

        Logger.LogInfo($"Plugin {ModInfo.NAME} v{ModInfo.VERSION} is loaded!");
    }
}
