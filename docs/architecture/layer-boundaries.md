# Layer Boundaries

## Core

`Core` owns scan state, use cases, callback handlers, result values, and port
interfaces. It decides when a baseline may be reset, when a later count may be
compared, and whether a result has already been sent.

Core depends on abstractions such as `IGameInterop`, `IPluginConfig`,
`IPluginLogger`, and `IValidationLogger`. It does not reference BepInEx,
Harmony, Unity, or Lethal Company types.

## Interop

`Interop` implements the Core ports and owns BepInEx configuration and
logging, Harmony patch definitions, game-object access, networking, and chat.
Harmony callbacks are small adapters: they guard exceptions, then delegate to
`PluginController`.

## Composition

`PluginController.Create()` is the composition root. It creates concrete
adapters, state, use cases, and handlers, then provides the callback-facing
methods used by Interop. New external dependencies should enter through an
Interop adapter and a Core port rather than through static access from a use
case.
