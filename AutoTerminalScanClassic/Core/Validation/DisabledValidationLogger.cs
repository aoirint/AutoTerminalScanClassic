#nullable enable

using AutoTerminalScanClassic.Core.Ports;

namespace AutoTerminalScanClassic.Core.Validation;

/// <summary>
/// No-op validation logger used when structured diagnostics are disabled.
/// </summary>
/// <remarks>
/// Keeping this behind the same port lets use cases record validation intent
/// unconditionally while the Debug config decides whether anything is emitted.
/// </remarks>
internal sealed class DisabledValidationLogger : IValidationLogger
{
    public static DisabledValidationLogger Instance { get; } = new();

    private DisabledValidationLogger()
    {
    }

    /// <summary>
    /// Drops the event without touching the normal plugin log.
    /// </summary>
    public void Record(ValidationLogRecord record)
    {
    }
}
