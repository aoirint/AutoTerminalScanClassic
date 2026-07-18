# Terminal Scan

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

Use the following declarations for the baseline scan and delayed comparison.

## Patch and access targets

### `RoundManager`

| Member | Declaration | Role |
| --- | --- | --- |
| Level-generation completion | `public void FinishGeneratingNewLevelClientRpc()` | Patch with a postfix to take the baseline after the client has finished level generation. |

### `TimeOfDay`

| Member | Declaration | Role |
| --- | --- | --- |
| Time tick | `private void MoveTimeOfDay()` | Patch with `AccessTools.Method(typeof(TimeOfDay), "MoveTimeOfDay")`; use `TimeOfDay __instance` to read the gate fields. |
| Elapsed time | `public float globalTime` | Current time used by the delayed comparison gate. |
| Time speed | `public float globalTimeSpeedMultiplier` | The delayed gate is `globalTime - 100f >= globalTimeSpeedMultiplier`. |

### `GrabbableObject`

| Member | Declaration | Role |
| --- | --- | --- |
| Item data | `public Item itemProperties` | A null value means the object cannot be classified as loot. |
| Ship-room state | `public bool isInShipRoom` | Excludes items in the ship room. |
| Elevator state | `public bool isInElevator` | Excludes items in the elevator. |

### `Item`

| Member | Declaration | Role |
| --- | --- | --- |
| Scrap flag | `public bool isScrap` | Includes only scrap items. |

## Implementation choices

| Decision | Options | Recommended approach | Why |
| --- | --- | --- | --- |
| Take the baseline count | Patch `FinishGeneratingNewLevelClientRpc()`; use a scene-load callback; start a fixed delay | Use a postfix on `FinishGeneratingNewLevelClientRpc()`. | The callback names the client-side completion of level generation; a scene callback or elapsed delay does not establish the same game-state boundary. |
| Take the later count | Use a coroutine delay; poll in `Update()`; patch `MoveTimeOfDay()` | Use a postfix on `MoveTimeOfDay()` and evaluate the documented `globalTime` gate. | The later boundary is defined by base-game time values, so it remains tied to the same time progression as the game rather than to a mod-local timer. |
| Select counted objects | Count all `GrabbableObject`s; rely on scan UI text; apply the explicit item predicate | Apply the `itemProperties`, `isScrap`, `isInShipRoom`, and `isInElevator` predicate. | The predicate distinguishes remaining scrap from non-scrap and ship-contained objects; UI text provides an already-aggregated result without the classification inputs. |
| Handle missing item data | Treat null `itemProperties` as zero; skip and record an unavailable read | Skip and record it as unavailable. | A null item definition cannot establish whether the object is scrap, so converting it to zero silently turns an incomplete observation into a count. |

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
