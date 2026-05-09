# AutoTerminalScanClassic

A Lethal Company mod that scans the number of scraps on the moon and sends it
to the chat at the start of each day, just like the terminal scan technique.

## Compatibility

- Current package metadata targets Lethal Company v81.5 with BepInExPack
  v5.4.2305.
- Earlier v0.1.x releases were prepared for Lethal Company v73.
- Real-game validation for the current v81.5 migration is not yet recorded.

## What it does

- Daily Automatic Terminal Scan: at the start of each day, scans the number of
  scraps on the moon and sends it to the chat like `19 4`.
    - `19` is the level-load scan count.
    - `4` is the later difference after outdoor scrap can appear.
    - The count intentionally follows vanilla terminal scan behavior, including
      vanilla-style inaccuracies.

## Who needs to install

Client-side only. The host does not need to install this mod.

However, any client may send their own scan results to the chat, which may spam
the chat. It is recommended for the host to install this mod and ask other
players to configure their own installs not to send duplicate scan results.

## Configuration

| Name | Type | Default | Description |
| :--- | :--- | :------ | :---------- |
| `Enabled` | bool | true | Set to false to disable this mod. |
| `BroadcastMode` | enum | SelfOnly | Controls whether this mod sends scan results to other players. `SelfOnly` shows scan results only to you. `HostOnly` sends scan results to other players only when you are the host. `Always` always sends scan results to other players. |
| `ValidationLogging` | bool | false | Debug option for structured release-validation logs. Leave this disabled unless you are collecting validation evidence. |

## Differences from [matsuura/AutoScan](https://thunderstore.io/c/lethal-company/p/matsuura/AutoScan/)

`matsuura/AutoScan` updates the number of scraps when players collect scraps
into the ship. It may be affected by Nutcracker shotguns, Butler knives, and
Maneater feeding during the day.

This mod only calculates the number of scraps at the start of each day and does
not update after that.

## Differences from [abu/AutoScan](https://thunderstore.io/c/lethal-company/p/abu/AutoScan/)

`abu/AutoScan` splits the number of scraps based on the item type.

This mod splits the number of scraps based solely on scan timing.

`abu/AutoScan` uses GameObject hierarchy to exclude scraps on the ship and the
cruiser, which may result in a more accurate count than vanilla.

This mod counts scraps exactly like the vanilla terminal scan, which may result
in an inaccurate count like vanilla.

## Differences from [Happyness/AutoTerminalScan](https://thunderstore.io/c/lethal-company/p/Happyness/AutoTerminalScan/)

`Happyness/AutoTerminalScan` reports the number of scraps categorized by the
item type, such as indoor scrap, bee hives, or bird presence.

This mod splits the number of scraps based solely on scan timing.

`Happyness/AutoTerminalScan` reports dungeon type information.

This mod only reports the number of scraps.

## Differences from [PsyKO345/ScrapCounter](https://thunderstore.io/c/lethal-company/p/PsyKO345/ScrapCounter/)

`PsyKO345/ScrapCounter` splits the number of scraps based on the item type.

This mod splits the number of scraps based solely on scan timing.

`PsyKO345/ScrapCounter` uses GameObject hierarchy to exclude scraps on the
cruiser, which may result in a more accurate count than vanilla.

This mod counts scraps exactly like the vanilla terminal scan, which may result
in an inaccurate count like vanilla.

`PsyKO345/ScrapCounter` displays the number of scraps using a custom UI element.

This mod sends the number of scraps to the chat.

## Differences from [MrListivn/LandingLootToChat](https://thunderstore.io/c/lethal-company/p/MrListivn/LandingLootToChat/)

`MrListivn/LandingLootToChat` splits the number of scraps based on the item
type, such as one handed, two handed, or bee hives.

This mod splits the number of scraps based solely on scan timing.

`MrListivn/LandingLootToChat` reports dungeon type information.

This mod only reports the number of scraps.

## AI Disclosure

Some parts of this project were developed with AI tools based on large language
models (LLMs), including agent-based tools. The project maintainer reviews the
code. This disclosure is made in compliance with Thunderstore and community
policies.
