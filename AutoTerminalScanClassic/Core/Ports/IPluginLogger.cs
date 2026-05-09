#nullable enable

namespace AutoTerminalScanClassic.Core.Ports;

internal interface IPluginLogger
{
    void LogDebug(string message);

    void LogInfo(string message);

    void LogWarning(string message);

    void LogError(string message);
}
