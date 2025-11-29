# AutoTerminalScanClassic

A Lethal Company mod that scans the number of scraps on the moon and sends it to the chat at the start of each day, just like the terminal scan technique.

This mod works for v73+. Maybe works for the earlier versions, but not tested.

## What it does

- Daily Automatic Terminal Scan: At the start of each day, scans the number of scraps on the moon and sends it to the chat like `19 4` (`19` indoor scraps, `4` outdoor scraps but maybe inaccurate like vanilla).

## Who needs to install

Client-side only. The host does not need to install this mod.

However, any client may send their own scan results to the chat, which may spam the chat.
So it's recommended for the host to install this mod and ask other players to configure not to send their own scan results.

## Configuration

| Name | Type | Default | Description |
|:--------|:-----|:--------|:------------|
| Enabled | bool | true | Set to false to disable this mod. |
| BroadcastMode | enum | SelfOnly | Controls whether this mod sends scan results to other players. If SelfOnly, you can still see scan results but not send to other players. If HostOnly, you send scan results to other players only when you are the host. If Always, you always send scan results to other players. |

## Differences from [matsuura/AutoScan](https://thunderstore.io/c/lethal-company/p/matsuura/AutoScan/)

`matsuura/AutoScan` updates the number of scraps when players collect scraps into the ship.
Maybe affected by Nutcracker shotguns, Butler knives, and Maneater feeding during the day.

This mod only calculates the number of scraps at the start of each day and does not update after that.

## Differences from [abu/AutoScan](https://thunderstore.io/c/lethal-company/p/abu/AutoScan/)

`abu/AutoScan` splits the number of scraps based on the item type.

This mod splits the number of scraps based solely on scan timing.

`abu/AutoScan` uses GameObject hierarchy to exclude scraps on the ship and the cruiser, which may result in *more accurate* count than vanilla.

This mod counts scraps exactly like the vanilla terminal scan, which may result in *inaccurate* count like vanilla.

## Differences from [Happyness/AutoTerminalScan](https://thunderstore.io/c/lethal-company/p/Happyness/AutoTerminalScan/)

`Happyness/AutoTerminalScan` reports the number of scraps categorized by the item type. e.g., indoor scrap, bee hives, bird presence.

This mod splits the number of scraps based solely on scan timing.

`Happyness/AutoTerminalScan` reports dungeon type information.

This mod only reports the number of scraps.

## Differences from [PsyKO345/ScrapCounter](https://thunderstore.io/c/lethal-company/p/PsyKO345/ScrapCounter/)

`PsyKO345/ScrapCounter` splits the number of scraps based on the item type.

This mod splits the number of scraps based solely on scan timing.

`PsyKO345/ScrapCounter` uses GameObject hierarchy to exclude scraps on the cruiser, which may result in *more accurate* count than vanilla.

This mod counts scraps exactly like the vanilla terminal scan, which may result in *inaccurate* count like vanilla.

`PsyKO345/ScrapCounter` displays the number of scraps using a custom UI element.

This mod sends the number of scraps to the chat.
