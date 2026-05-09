#nullable enable

using AutoTerminalScanClassic.Core.Validation;

namespace AutoTerminalScanClassic.Core.Ports;

/// <summary>
/// Records structured validation events without exposing Core to the log transport.
/// </summary>
/// <remarks>
/// Use cases describe semantic validation events; Interop decides whether those
/// events become BepInEx log lines or no-op records when diagnostics are disabled.
/// </remarks>
internal interface IValidationLogger
{
    /// <summary>
    /// Records one validation event using stable event and field names.
    /// </summary>
    void Record(ValidationLogRecord record);
}
