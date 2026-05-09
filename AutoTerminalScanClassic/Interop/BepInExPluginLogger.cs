#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using BepInEx.Logging;

namespace AutoTerminalScanClassic.Interop;

internal sealed class BepInExPluginLogger : IPluginLogger
{
    private readonly ManualLogSource logger;

    public BepInExPluginLogger(ManualLogSource logger)
    {
        this.logger = logger;
    }

    public void LogDebug(string message)
    {
        logger.LogDebug(message);
    }

    public void LogInfo(string message)
    {
        logger.LogInfo(message);
    }

    public void LogWarning(string message)
    {
        logger.LogWarning(message);
    }

    public void LogError(string message)
    {
        logger.LogError(message);
    }
}
