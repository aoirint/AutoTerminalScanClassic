#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using AutoTerminalScanClassic.Core.State;
using AutoTerminalScanClassic.Core.Validation;

namespace AutoTerminalScanClassic.Core.UseCases;

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

    public SendScanResultOnceResult Execute()
    {
        if (scanState.HasSentChatToday)
        {
            return SendScanResultOnceResult.AlreadySent;
        }

        if (!config.Enabled)
        {
            scanState.MarkSent();
            logger.LogDebug("Not enabled.");
            validationLogger.Record(
                ValidationLogRecord.HourAdvancedScanResult(ValidationLogScanResult.Disabled)
            );
            return SendScanResultOnceResult.Disabled;
        }

        if (scanState.ItemCountOnLevelLoaded == null)
        {
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

        scanState.MarkSent();
        return SendScanResultOnceResult.Success;
    }

    private ChatSendTarget SelectChatTarget(BroadcastMode broadcastMode)
    {
        if (broadcastMode == BroadcastMode.SelfOnly)
        {
            return ChatSendTarget.SelfOnly;
        }

        if (broadcastMode == BroadcastMode.HostOnly && !gameInterop.IsHost())
        {
            return ChatSendTarget.SelfOnly;
        }

        return ChatSendTarget.Everyone;
    }

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
