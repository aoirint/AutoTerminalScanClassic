#nullable enable

using BepInEx;
using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.Validation;
using AutoTerminalScanClassic.Interop;
using AutoTerminalScanClassic.Interop.Game.Patches;

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
    private static PluginController? controller;

    /// <summary>
    /// Shared plugin controller used by game callback types constructed outside startup.
    /// </summary>
    internal static PluginController Controller => controller!;

    private void Awake()
    {
        var logger = new BepInExPluginLogger(base.Logger);
        var config = BepInExPluginConfig.Bind(Config);

        IValidationLogger validationLogger = config.ValidationLogging
            ? new BepInExValidationLogger(logger, System.DateTime.UtcNow)
            : DisabledValidationLogger.Instance;

        validationLogger.Record(
            ValidationLogRecord.PluginLoaded(
                version: MyPluginInfo.PLUGIN_VERSION,
                validationLogging: config.ValidationLogging,
                enabled: config.Enabled,
                broadcastMode: config.BroadcastMode
            )
        );

        controller = PluginController.Create(
            config: config,
            logger: logger,
            validationLogger: validationLogger
        );

        // Startup order matters: configure the guard after the controller is
        // wired and before patching so the first callback can be diagnosed.
        HarmonyCallbackGuard.Configure(
            new HarmonyCallbackDiagnosticReporter(
                logger: logger,
                validationLogger: validationLogger
            )
        );

        // Startup order matters: construct the controller before patching so
        // the first game callback can enter a fully wired plugin boundary.
        HarmonyPatchInstaller.Install();

        logger.LogInfo(
            $"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!"
        );
    }
}
