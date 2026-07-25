# Scan Workflow

The base-game callbacks, terminal scan branches, and time values used here are
defined in [../domain/terminal-scan.md](../domain/terminal-scan.md).

## Model

`ScanState` holds the baseline count and whether the current level has already
received its result. It is session state, not a record of every scanned item.

`ResetScanForNewLevelUseCase` clears that state. `SendScanResultOnceUseCase`
calculates and delivers the comparison only after a baseline exists and no
result has been sent for the level.

## Callback flow

The `RoundManager` Harmony postfix enters `RoundCallbackHandler`. When enabled,
it resets state and captures the baseline through the game interop boundary.
The `TimeOfDay` postfix evaluates this mod's delayed gate before it enters
`TimeOfDayCallbackHandler`; a passing gate asks the send-once use case to
capture the later count and send the result.

The decision to send only one result per level belongs to this mod.

## Gates and retries

Both callback handlers stop unless the local peer is a client. When enabled,
the level-generation callback resets `ScanState`, then captures the baseline
with the terminal scan's outside-ship predicate even if the game is in ship
phase. A null item definition aborts that complete count and leaves no baseline;
it is not skipped as a zero-value object.

The `TimeOfDay` postfix preserves the classic manager-era gate:
`globalTime - 100f >= globalTimeSpeedMultiplier`. Once this is true, every
later time callback can attempt the comparison. The report delta is
`laterCount - baselineCount`; it describes a count change, not the identity or
cause of an item movement.

If the mod is disabled at level load, it marks the state sent without taking a
baseline. Otherwise, a missing baseline, later scan failure, or chat-send
failure leaves the state retryable on a later eligible time tick. Only a
successful chat send marks an enabled run sent.

## Late first spawn relative to the gate

The delayed gate is based only on `globalTime`; it does not wait for an enemy
vent, a daytime-enemy batch, or a displayed-clock event. The first-batch
schedule is documented in
[../domain/enemy-spawn-scheduling.md](../domain/enemy-spawn-scheduling.md).
The first batch can complete after the classic 07:40 gate when an interior vent
remains scheduled later in the first cycle.

At the first eligible callback, the mod performs the later terminal scan and
sends the count delta when that scan and chat delivery both succeed. A hive or
other countable object that has already spawned is reflected only as part of
that aggregate count; the result does not identify its type or spawn cause.
An object that spawns after a successful send is absent from that result:
`ScanState` is marked sent and the mod performs no automatic rescan for the
level. A failed scan or chat send remains retryable on a later time callback.

## Delivery policy

When enabled and both observations plus delivery succeed, the mod reports one
comparison result for a generated level. Repeated time ticks must not resend a
successful result, and a missing baseline must not be treated as zero.
Validation logging records these outcomes without becoming a second source of
scan state.

`BroadcastMode.SelfOnly` sends only to the local player. `HostOnly` sends to
everyone only when the local client is host; a non-host instead receives its
own result. The remaining broadcast mode sends to everyone. These routing
choices are mod policy, not terminal behaviour.
