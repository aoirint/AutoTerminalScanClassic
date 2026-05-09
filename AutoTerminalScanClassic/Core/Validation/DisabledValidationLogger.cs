#nullable enable

using AutoTerminalScanClassic.Core.Ports;

namespace AutoTerminalScanClassic.Core.Validation;

internal sealed class DisabledValidationLogger : IValidationLogger
{
    public static DisabledValidationLogger Instance { get; } = new();

    private DisabledValidationLogger()
    {
    }

    public void Record(ValidationLogRecord record)
    {
    }
}
