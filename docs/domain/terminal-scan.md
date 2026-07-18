# Terminal Scan

## Evidence scope

This document records the terminal scan domain used for Lethal Company v81,
Steam manifest `6423525044216269478`. Recheck the evidence when the target game
version changes.

The claims are `direct_static` observations from the v81 decompilation's
`Terminal.cs`, `RoundManager.cs`, and `TimeOfDay.cs`. The matching v81 asset
root was inspected; no serialized override is used as a premise here. Actual
spawn completion remains a runtime observation, so the timing section states a
code gate rather than a guaranteed observation window.

## Counted objects

The scan count is based on `GrabbableObject` instances. An object belongs in the
remaining-loot count only when all of the following are true:

- `itemProperties` is available.
- `itemProperties.isScrap` is true.
- The object is not in the ship room.
- The object is not in the elevator.

Treat a missing `itemProperties` value as an unavailable observation rather
than a zero or partial count.

## Timing

Level generation and time advancement are separate game events. A scan taken
immediately after level generation provides a baseline. A later scan must wait
until the relevant spawned objects are available; otherwise the comparison can
describe spawn timing rather than item movement.

For the classic timing used by this repository, the later gate is reached when
`globalTime - 100f` is at least `globalTimeSpeedMultiplier`.

## Comparison interpretation

Given a baseline count and a later count, the difference is:

```text
later count - baseline count
```

Keep the two source counts available when diagnosing a surprising difference.
A count difference alone does not identify which items moved or why.

## Change checklist

Before relying on a terminal-scan comparison, confirm:

1. The target game version and manifest match the evidence.
2. Both observations use the same inclusion rules.
3. Neither observation is partial or unavailable.
4. The later observation is taken after the relevant spawn window.
