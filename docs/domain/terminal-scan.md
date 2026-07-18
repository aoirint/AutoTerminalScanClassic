# Terminal Scan

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

Use the following declarations for the baseline scan and delayed comparison.

## Patch and access targets

### `Terminal`

| Member | Declaration | Role |
| --- | --- | --- |
| Scan expansion | `private string TextPostProcess(string modifiedDisplayText, TerminalNode node)` | Owns the v81 `[scanForItems]` enumeration and ship-phase predicate branches. |

### `RoundManager`

| Member | Declaration | Role |
| --- | --- | --- |
| Level-generation completion | `public void FinishGeneratingNewLevelClientRpc()` | Patch with a postfix to take the baseline after the client has finished level generation. |

### `StartOfRound`

| Member | Declaration | Role |
| --- | --- | --- |
| Ship phase | `public bool inShipPhase` | Selects the terminal scan's ship-phase or outside-ship item predicate. |

### `TimeOfDay`

| Member | Declaration | Role |
| --- | --- | --- |
| Time tick | `private void MoveTimeOfDay()` | Postfix target declared as `[HarmonyPatch(nameof(TimeOfDay.MoveTimeOfDay))]`; use `TimeOfDay __instance` to read time values. |
| Elapsed time | `public float globalTime` | Current base-game elapsed-time value. |
| Time speed | `public float globalTimeSpeedMultiplier` | Current base-game time-speed value. |

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

## Terminal scan branches

When `StartOfRound.Instance.inShipPhase` is false, the v81 terminal scan counts
a `GrabbableObject` only when `itemProperties.isScrap` is true,
`isInShipRoom` is false, and `isInElevator` is false. During ship phase, the
terminal counts scrap without those ship and elevator exclusions. A mod must
choose which base-game branch defines each of its observations.

`RoundManager.FinishGeneratingNewLevelClientRpc()` runs after the client-side
level generation completion path. `TimeOfDay.Update()` invokes
`MoveTimeOfDay()` during time progression. The game exposes both callbacks and
time values; a mod's timing gate, comparison, unavailable-read policy, and
message routing are mod-specific architecture.

## Change checklist

1. Patch the exact no-argument RPC and no-argument private time method above.
2. Choose and document the intended terminal predicate branch for each mod
   observation.
