#nullable enable

namespace AutoTerminalScanClassic.Core.UseCases;

/// <summary>
/// Outcome of capturing the level-load scan baseline.
/// </summary>
internal enum ResetScanForNewLevelResult
{
    /// <summary>
    /// The plugin was disabled, so no baseline should be captured for this level.
    /// </summary>
    Disabled,

    /// <summary>
    /// Interop could not produce a reliable item count.
    /// </summary>
    ScanFailed,

    /// <summary>
    /// The level baseline was recorded successfully.
    /// </summary>
    Success
}

/// <summary>
/// Outcome of attempting the delayed scan-result chat send.
/// </summary>
internal enum SendScanResultOnceResult
{
    /// <summary>
    /// This level has already completed its chat-send attempt successfully or by policy.
    /// </summary>
    AlreadySent,

    /// <summary>
    /// The plugin was disabled when the delayed callback ran.
    /// </summary>
    Disabled,

    /// <summary>
    /// The delayed scan ran without a valid level-load baseline.
    /// </summary>
    MissingLevelLoadedScan,

    /// <summary>
    /// Interop could not produce the delayed item count.
    /// </summary>
    ScanFailed,

    /// <summary>
    /// The chat adapter rejected or could not deliver the formatted message.
    /// </summary>
    SendFailed,

    /// <summary>
    /// The scan delta was sent and the level is marked complete.
    /// </summary>
    Success
}

/// <summary>
/// Internal routing target for the chat adapter selected from BroadcastMode.
/// </summary>
internal enum ChatSendTarget
{
    /// <summary>
    /// Render a local-only HUD chat line.
    /// </summary>
    SelfOnly,

    /// <summary>
    /// Ask the game to route the message through server chat.
    /// </summary>
    Everyone
}
