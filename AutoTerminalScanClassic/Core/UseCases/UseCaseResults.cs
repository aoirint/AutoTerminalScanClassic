#nullable enable

namespace AutoTerminalScanClassic.Core.UseCases;

internal enum ResetScanForNewLevelResult
{
    Disabled,
    ScanFailed,
    Success
}

internal enum SendScanResultOnceResult
{
    AlreadySent,
    Disabled,
    MissingLevelLoadedScan,
    ScanFailed,
    SendFailed,
    Success
}

internal enum ChatSendTarget
{
    SelfOnly,
    Everyone
}
