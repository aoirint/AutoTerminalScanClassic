#nullable enable

using System;
using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.Validation;

namespace AutoTerminalScanClassic.Interop.Game.Patches;

/// <summary>
/// Emits diagnostics for Harmony callback exceptions swallowed at the patch boundary.
/// </summary>
/// <remarks>
/// The guard owns exception handling; this reporter only translates a caught
/// exception into compact log text and a structured validation event.
/// </remarks>
internal sealed class HarmonyCallbackDiagnosticReporter
{
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public HarmonyCallbackDiagnosticReporter(
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        this.logger = logger;
        this.validationLogger = validationLogger;
    }

    public void RecordCallbackException(string callback, Exception exception)
    {
        // Keep structured validation diagnostics compact and environment-safe:
        // no exception messages, stack traces, Unity object data, or local paths.
        var exceptionType = exception.GetType().FullName ?? exception.GetType().Name;
        logger.LogError(
            $"Harmony callback exception: callback={callback}, exception_type={exceptionType}"
        );
        validationLogger.Record(
            ValidationLogRecord.CallbackException(
                callback: callback,
                exceptionType: exceptionType
            )
        );
    }
}
