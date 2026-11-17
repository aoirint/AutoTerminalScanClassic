# Domain Knowledge

This directory contains implementation knowledge about Lethal Company and
reusable knowledge about dependencies. It does not define AutoTerminalScanClassic's product model or design decisions.

- [Terminal scan](terminal-scan.md) records the callbacks and item-predicate
  branches exposed by the terminal scan implementation.
- [Enemy spawn scheduling](enemy-spawn-scheduling.md) records the
  base-game timing and category rules behind the first daytime spawn batch.
- [Dependency baseline](dependencies.md) explains NuGet minimum publish age,
  including automatic version selection, publication metadata, and restore limits.
