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
| Scene enumeration | `UnityEngine.Object.FindObjectsOfType<GrabbableObject>()` | Returns the loaded objects whose item state can be classified. |
| Item data | `public Item itemProperties` | A null value means the object cannot be classified as loot. |
| Ship-room state | `public bool isInShipRoom` | Excludes items in the ship room. |
| Elevator state | `public bool isInElevator` | Excludes items in the elevator. |

### `Item`

| Member | Declaration | Role |
| --- | --- | --- |
| Scrap flag | `public bool isScrap` | Includes only scrap items. |

## Implementation choices

### Take the baseline count

#### Patch `RoundManager.FinishGeneratingNewLevelClientRpc()` with a postfix — recommended

The callback names the client-side completion of level generation, making it
the relevant base-game state boundary for the baseline.

#### Use a scene-load callback or a fixed delay

Neither establishes the same level-generation completion boundary; both can
run before or after the game state represented by the RPC.

### Take the later count

#### Patch `TimeOfDay.MoveTimeOfDay()` with a postfix and evaluate the time gate — recommended

The later boundary is defined by `globalTime` and
`globalTimeSpeedMultiplier`, so this remains tied to base-game time
progression rather than to a mod-local timer.

#### Use a coroutine delay or poll in `Update()`

Those approaches measure mod-local elapsed time. They do not identify the same
base-game time tick or guarantee the documented gate was crossed.

### Select counted objects

#### Apply the explicit `GrabbableObject` predicate — recommended

Use `itemProperties`, `itemProperties.isScrap`, `isInShipRoom`, and
`isInElevator`. These values distinguish remaining scrap from non-scrap and
ship-contained objects.

#### Count all `GrabbableObject` instances

This includes objects that do not satisfy the terminal-scan definition.

#### Rely on scan UI text

The UI is an already-aggregated result and does not expose the object-level
classification inputs required by the count.

### Find countable objects

#### Enumerate loaded `GrabbableObject` components for each count — recommended

`Object.FindObjectsOfType<GrabbableObject>()` provides the current scene set
whose `itemProperties`, ship-room, and elevator flags define the predicate.
Evaluate the same predicate for the baseline and later count.

#### Retain a previous enumeration or infer the count from a terminal field

Objects can be spawned, despawned, or move into the ship between observations.
A cached set and terminal text do not expose the current object-level inputs.

### Handle missing item data

#### Skip the object and record an unavailable read — recommended

A null `itemProperties` value cannot establish whether the object is scrap.

#### Treat null `itemProperties` as zero

This silently turns an incomplete observation into a count.

## Count and timing

Count a `GrabbableObject` only when `itemProperties` is non-null,
`itemProperties.isScrap` is true, `isInShipRoom` is false, and `isInElevator`
is false. A missing `itemProperties` is an unavailable observation, not a
zero-value item.

`RoundManager.FinishGeneratingNewLevelClientRpc()` is the baseline callback.
The game invokes it after the client-side level generation completion path.
`TimeOfDay.Update()` invokes `MoveTimeOfDay()` during time progression, so its
postfix supplies the later polling callback. For the classic comparison, do not
take the second count until:

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
