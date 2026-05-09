#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.State;
using AutoTerminalScanClassic.Core.Validation;

namespace AutoTerminalScanClassic.Core.UseCases;

/// <summary>
/// Computes the scan-count delta and sends the result once for the current level.
/// </summary>
/// <remarks>
/// The use case preserves the original manager behavior: compare the level-load
/// scan with the later time-advance scan, format the same two-number message,
/// and route chat according to BroadcastMode.
/// </remarks>
internal sealed class SendScanResultOnceUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginConfig config;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;
    private readonly ScanState scanState;

    public SendScanResultOnceUseCase(
        IGameInterop gameInterop,
        IPluginConfig config,
        IPluginLogger logger,
        IValidationLogger validationLogger,
        ScanState scanState
    )
    {
        this.gameInterop = gameInterop;
        this.config = config;
        this.logger = logger;
        this.validationLogger = validationLogger;
        this.scanState = scanState;
    }

    /// <summary>
    /// Sends the scan result if this level has not already produced a chat message.
    /// </summary>
    public SendScanResultOnceResult Execute()
    {
        if (scanState.HasSentChatToday)
        {
            // SDC records no-op validation results as explicit outcomes. Keep
            // ATSC's once-per-level guard visible for validation plans too.
            validationLogger.Record(
                ValidationLogRecord.HourAdvancedScanResult(ValidationLogScanResult.AlreadySent)
            );
            return SendScanResultOnceResult.AlreadySent;
        }

        if (!config.Enabled)
        {
            // Disabled mode marks the level as complete just like the old
            // manager did, preventing repeated debug logs on every time tick.
            scanState.MarkSent();
            logger.LogDebug("Not enabled.");
            validationLogger.Record(
                ValidationLogRecord.HourAdvancedScanResult(ValidationLogScanResult.Disabled)
            );
            return SendScanResultOnceResult.Disabled;
        }

        if (scanState.ItemCountOnLevelLoaded == null)
        {
            // A missing baseline means the level-load callback never produced a
            // valid count; sending a delta from only the second scan would be
            // misleading, so leave the state retryable for diagnostics.
            logger.LogError("itemCountOnLevelLoaded is null.");
            validationLogger.Record(
                ValidationLogRecord.HourAdvancedScanResult(
                    ValidationLogScanResult.MissingLevelLoadedScan
                )
            );
            return SendScanResultOnceResult.MissingLevelLoadedScan;
        }
        var itemCountOnLevelLoaded = scanState.ItemCountOnLevelLoaded.Value;

        var itemCountOnHourAdvancedNullable = gameInterop.ScanItemCount();
        if (itemCountOnHourAdvancedNullable == null)
        {
            logger.LogError("itemCountOnHourAdvanced is null.");
            validationLogger.Record(
                ValidationLogRecord.HourAdvancedScanResult(ValidationLogScanResult.ScanFailed)
            );
            return SendScanResultOnceResult.ScanFailed;
        }
        var itemCountOnHourAdvanced = itemCountOnHourAdvancedNullable.Value;

        var itemCountDifference = itemCountOnHourAdvanced - itemCountOnLevelLoaded;

        // Keep both raw counts in the debug log so behavior can be compared
        // with the pre-refactor manager without interpreting the chat message.
        logger.LogDebug(
            "Hour advanced scan complete." +
            $" itemCountOnLevelLoaded={itemCountOnLevelLoaded}" +
            $" itemCountOnHourAdvanced={itemCountOnHourAdvanced}" +
            $" itemCountDifference={itemCountDifference}"
        );
        validationLogger.Record(
            ValidationLogRecord.HourAdvancedScanResult(
                result: ValidationLogScanResult.Success,
                itemCountOnLevelLoaded: itemCountOnLevelLoaded,
                itemCountOnHourAdvanced: itemCountOnHourAdvanced,
                itemCountDifference: itemCountDifference
            )
        );

        // The chat payload intentionally remains the historical compact
        // "initial delta" format used by this mod's classic scan workflow.
        var message = $"{itemCountOnLevelLoaded} {itemCountDifference}";
        var target = SelectChatTarget(config.BroadcastMode);
        var sendChatSuccess = SendChat(target: target, message: message);
        validationLogger.Record(
            ValidationLogRecord.ChatSendResult(
                broadcastMode: config.BroadcastMode,
                target: target,
                success: sendChatSuccess
            )
        );

        if (!sendChatSuccess)
        {
            logger.LogError($"Failed to send chat message. message={message}");
            return SendScanResultOnceResult.SendFailed;
        }
        logger.LogDebug($"Sent chat message successfully. message={message}");

        // Mark after a successful send so transient scan/chat failures can be
        // retried by later MoveTimeOfDay callbacks in the same level.
        scanState.MarkSent();
        return SendScanResultOnceResult.Success;
    }

    /// <summary>
    /// Converts user-facing broadcast configuration into the interop chat operation.
    /// </summary>
    private ChatSendTarget SelectChatTarget(BroadcastMode broadcastMode)
    {
        if (broadcastMode == BroadcastMode.SelfOnly)
        {
            return ChatSendTarget.SelfOnly;
        }

        if (broadcastMode == BroadcastMode.HostOnly && !gameInterop.IsHost())
        {
            // HostOnly is fail-local for non-host clients: they still see their
            // own scan result, but they do not ask the server to broadcast it.
            return ChatSendTarget.SelfOnly;
        }

        return ChatSendTarget.Everyone;
    }

    /// <summary>
    /// Dispatches the selected chat operation through the game interop port.
    /// </summary>
    private bool SendChat(ChatSendTarget target, string message)
    {
        return target switch
        {
            ChatSendTarget.SelfOnly => gameInterop.SendChatToSelfOnly(message),
            ChatSendTarget.Everyone => gameInterop.SendChatToEveryone(message),
            _ => false
        };
    }
}
