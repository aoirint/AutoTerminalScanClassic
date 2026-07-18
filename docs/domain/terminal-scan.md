# Terminal Scan

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

Use the following declarations for the baseline scan and delayed comparison.

## Patch and access targets

| Type | Member | Declaration | Use |
| --- | --- | --- | --- |
| `RoundManager` | Level-generation completion | `public void FinishGeneratingNewLevelClientRpc()` | Patch with a postfix to take the baseline after the client has finished level generation. |
| `TimeOfDay` | Time tick | `private void MoveTimeOfDay()` | Patch with `AccessTools.Method(typeof(TimeOfDay), "MoveTimeOfDay")`; use `TimeOfDay __instance` to read the gate fields. |
| `TimeOfDay` | Elapsed time | `public float globalTime` | Current time used by the delayed comparison gate. |
| `TimeOfDay` | Time speed | `public float globalTimeSpeedMultiplier` | The delayed gate is `globalTime - 100f >= globalTimeSpeedMultiplier`. |
| `GrabbableObject` | Item data | `public Item itemProperties` | A null value means the object cannot be classified as loot. |
| `GrabbableObject` | Ship-room state | `public bool isInShipRoom` | Excludes items in the ship room. |
| `GrabbableObject` | Elevator state | `public bool isInElevator` | Excludes items in the elevator. |
| `Item` | Scrap flag | `public bool isScrap` | Includes only scrap items. |

## Count and timing

Count a `GrabbableObject` only when `itemProperties` is non-null,
`itemProperties.isScrap` is true, `isInShipRoom` is false, and `isInElevator`
is false. A missing `itemProperties` is an unavailable observation, not a
zero-value item.

`RoundManager.FinishGeneratingNewLevelClientRpc()` is the baseline callback.
`TimeOfDay.MoveTimeOfDay()` supplies the later polling callback. For the
classic comparison, do not take the second count until:

```csharp
timeOfDay.globalTime - 100f >= timeOfDay.globalTimeSpeedMultiplier
```

The reported delta is `laterCount - baselineCount`. It identifies a change in
the qualifying object count, not the identity or cause of each item movement.

## Change checklist

1. Patch the exact no-argument RPC and no-argument private time method above.
2. Take baseline and later counts with the same `GrabbableObject` predicate.
3. Retain both counts for diagnostics; do not reduce an unavailable read to 0.
4. Keep the `100f` offset and `globalTimeSpeedMultiplier` comparison together.
