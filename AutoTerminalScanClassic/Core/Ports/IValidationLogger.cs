#nullable enable

using AutoTerminalScanClassic.Core.Validation;

namespace AutoTerminalScanClassic.Core.Ports;

internal interface IValidationLogger
{
    void Record(ValidationLogRecord record);
}
