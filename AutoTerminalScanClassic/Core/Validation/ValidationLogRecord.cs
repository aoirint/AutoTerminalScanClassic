#nullable enable

using System.Collections.Generic;
using AutoTerminalScanClassic.Core.UseCases;

namespace AutoTerminalScanClassic.Core.Validation;

internal enum ValidationLogScanResult
{
    Disabled,
    MissingLevelLoadedScan,
    ScanFailed,
    Success
}

/// <summary>
/// Immutable validation event description with stable event names and fields.
/// </summary>
/// <remarks>
/// The factories keep call sites tied to ATSC domain events while this type
/// centralizes the external schema, token spelling, and privacy boundary.
/// </remarks>
internal sealed class ValidationLogRecord
{
    // Call sites choose semantic events through named factories; this type owns
    // the stable field names and token spelling.
    private ValidationLogRecord(string eventName, Dictionary<string, object?>? fields = null)
    {
        EventName = eventName;
        Fields = fields;
    }

    public string EventName { get; }

    public Dictionary<string, object?>? Fields { get; }

    /// <summary>
    /// Describes plugin startup configuration without including local environment details.
    /// </summary>
    public static ValidationLogRecord PluginLoaded(
        string version,
        bool validationLogging,
        bool enabled,
        BroadcastMode broadcastMode
    )
    {
        return new(
            "plugin_loaded",
            new()
            {
                ["version"] = version,
                ["validation_logging"] = validationLogging,
                ["enabled"] = enabled,
                ["broadcast_mode"] = ToBroadcastModeToken(broadcastMode)
            }
        );
    }

    public static ValidationLogRecord ControllerCreated()
    {
        return new("controller_created");
    }

    /// <summary>
    /// Describes a swallowed Harmony callback exception using stable diagnostic fields.
    /// </summary>
    public static ValidationLogRecord CallbackException(string callback, string exceptionType)
    {
        return new(
            "callback_exception",
            new()
            {
                ["callback"] = callback,
                ["exception_type"] = exceptionType
            }
        );
    }

    public static ValidationLogRecord LevelLoadedScanResult(
        ValidationLogScanResult result,
        int? itemCount = null
    )
    {
        return new(
            "level_loaded_scan_result",
            CreateScanFields(result: result, itemCount: itemCount)
        );
    }

    public static ValidationLogRecord HourAdvancedScanResult(
        ValidationLogScanResult result,
        int? itemCountOnLevelLoaded = null,
        int? itemCountOnHourAdvanced = null,
        int? itemCountDifference = null
    )
    {
        var fields = CreateScanFields(result: result);
        fields["item_count_on_level_loaded"] = itemCountOnLevelLoaded;
        fields["item_count_on_hour_advanced"] = itemCountOnHourAdvanced;
        fields["item_count_difference"] = itemCountDifference;
        return new("hour_advanced_scan_result", fields);
    }

    public static ValidationLogRecord ChatSendResult(
        BroadcastMode broadcastMode,
        ChatSendTarget target,
        bool success
    )
    {
        return new(
            "chat_send_result",
            new()
            {
                ["broadcast_mode"] = ToBroadcastModeToken(broadcastMode),
                ["target"] = ToChatSendTargetToken(target),
                ["success"] = success
            }
        );
    }

    private static Dictionary<string, object?> CreateScanFields(
        ValidationLogScanResult result,
        int? itemCount = null
    )
    {
        // Scan events share the same result token and optional count field so
        // release checks can compare level-load and delayed scans consistently.
        return new()
        {
            ["result"] = ToScanResultToken(result),
            ["item_count"] = itemCount
        };
    }

    private static string ToBroadcastModeToken(BroadcastMode broadcastMode)
    {
        // Config enum names are user-facing, but validation logs use
        // lower_snake_case tokens for schema stability across C# renames.
        return broadcastMode switch
        {
            BroadcastMode.SelfOnly => "self_only",
            BroadcastMode.HostOnly => "host_only",
            BroadcastMode.Always => "always",
            _ => "unknown"
        };
    }

    private static string ToChatSendTargetToken(ChatSendTarget target)
    {
        // Unknown enum values fall back to schema-safe tokens instead of
        // leaking numeric values into validation output.
        return target switch
        {
            ChatSendTarget.SelfOnly => "self_only",
            ChatSendTarget.Everyone => "everyone",
            _ => "unknown"
        };
    }

    private static string ToScanResultToken(ValidationLogScanResult result)
    {
        return result switch
        {
            ValidationLogScanResult.Disabled => "disabled",
            ValidationLogScanResult.MissingLevelLoadedScan => "missing_level_loaded_scan",
            ValidationLogScanResult.ScanFailed => "scan_failed",
            ValidationLogScanResult.Success => "success",
            _ => "unknown"
        };
    }
}
