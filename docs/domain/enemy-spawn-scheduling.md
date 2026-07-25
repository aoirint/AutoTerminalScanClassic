# Enemy Spawn Scheduling

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

## Clock and first interior batch

`TimeOfDay` uses `lengthOfHours = 60` and `numberOfHours = 18`, for a
`totalTime` of 1080. The HUD maps day-time value `0` to 06:00 AM; therefore,
day-time value `100` is 07:40 AM.

The server starts interior-enemy scheduling after `currentDayTime > 85`. On
the first call to `RoundManager.PlotOutEnemiesForNextHour()`, each scheduled
vent receives an integer `spawnTime` from `EnemySpawnRandom.Next(10, 120)`.
The lower bound is inclusive and the upper bound is exclusive, so individual
interior vent times range from 06:10 through 07:59.

`RoundManager` spawns a vent enemy only after `currentDayTime > spawnTime`.
It therefore does not treat 07:40 as a fixed first-cycle timestamp.

## Daytime and outside batch transition

`RoundManager.Update()` advances the batch only when both conditions hold:

1. `TimeOfDay.hour > RoundManager.currentHour`.
2. The current interior schedule has no remaining `enemySpawnTimes` entries.

`AdvanceHourAndSpawnNewBatchOfEnemies()` then increments `currentHour` by two
and invokes daytime, outside, and weed spawning in that order. The first
daytime selection uses the two-hour bucket at day-time value `120`, but its
actual invocation waits for the preceding interior schedule to finish.

Consequently, the first daytime batch can occur after 07:40. For example, an
interior vent scheduled at day-time value `118` delays that transition until
just after 07:58. If the first interior plot contains no spawn entries, the
advance can occur immediately after interior scheduling begins. The exact
observed frame also depends on server update timing.

## Assurance daytime enemies

In the Assurance `SelectableLevel` asset, Red Locust Bees and GiantKiwi
are entries in `DaytimeEnemies`. Red Locust Bees are the base-game type shown
as circuit bees by the investigated display. `GiantSapsucker` is a Terminal
keyword asset, while the enemy asset is named `GiantKiwi`.

Those daytime types are selected by `SpawnDaytimeEnemiesOutside()`, not by the
interior vent scheduler. Their occurrence at the first daytime transition is
therefore compatible with a late final interior vent.

## Interpretation constraints

Each runtime `EnemyVent.spawnTime` and `EnemyVent.enemyType` records the
assigned interior schedule after the game has synchronized it. Recomputing a
schedule from a seed, or substituting a constant 07:40 time, can disagree with
the running game when the random-stream history, vents, or other mods differ.

This trace establishes static code and serialized-asset facts for the target
build. It does not prove the timing or synchronization behaviour of another
game version or an altered spawn implementation.

## Evidence and change trigger

The evidence is the target-build `RoundManager`, `TimeOfDay`, `HUDManager`, and
`EnemyVent` managed-code trace plus the `AssuranceLevel`, `RedLocustBees`, and
`GiantKiwi` serialized assets. Recheck this document whenever the supported
Lethal Company build, the relevant game members, or Assurance enemy lists
change.
