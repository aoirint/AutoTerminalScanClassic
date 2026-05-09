#nullable enable

namespace AutoTerminalScanClassic.Core.Ports;

/// <summary>
/// Logger port used by Core use cases and game interop adapters.
/// </summary>
/// <remarks>
/// Keeping logging behind a port avoids coupling Core scan policy to BepInEx
/// while preserving the existing log levels and message text.
/// </remarks>
internal interface IPluginLogger
{
    void LogDebug(string message);

    void LogInfo(string message);

    void LogWarning(string message);

    void LogError(string message);
}
