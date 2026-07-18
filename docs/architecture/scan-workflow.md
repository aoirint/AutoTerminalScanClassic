# Scan Workflow

## Model

`ScanState` holds the baseline count and whether the current level has already
received its result. It is session state, not a record of every scanned item.

`ResetScanForNewLevelUseCase` clears that state. `SendScanResultOnceUseCase`
calculates and delivers the comparison only after a baseline exists and no
result has been sent for the level.

## Callback flow

The `RoundManager` Harmony postfix enters `RoundCallbackHandler`, which
resets the state and captures the baseline through the game interop boundary.
The `TimeOfDay` postfix enters `TimeOfDayCallbackHandler`. When the
domain-defined delayed gate is reached, it asks the send-once use case to
capture the later count and send the result.

The callback timing and count predicate are base-game knowledge; see
[../domain/terminal-scan.md](../domain/terminal-scan.md). The decision to send
only one result per level belongs to this mod.

## Delivery policy

The mod reports one comparison result for each generated level. Repeated time
ticks must not resend the same result, and a missing baseline must not be
treated as zero. Validation logging records these outcomes without becoming a
second source of scan state.
