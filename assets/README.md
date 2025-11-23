# AutoTerminalScanClassic

A Lethal Company mod that scans the number of scraps on the moon and sends it to the chat at the start of each day, just like the terminal scan technique.

This mod works for v73+. Maybe works for the earlier versions, but not tested.

## What it does

- Daily automatic terminal scan: At the start of each day, scans the number of scraps on the moon and sends it to the chat like `19 4`.

## Who needs to install

Client-side only. The host does not need to install this mod.

However, any client may send their own scan results to the chat, which may spam the chat.
So it's recommended for the host to install this mod and ask other players to configure not to send their own scan results.

## Differences from [abu/AutoScan](https://thunderstore.io/c/lethal-company/p/abu/AutoScan/)

`abu/AutoScan` splits item counts based on the item type.

This mod splits item counts based solely on scan timing.

`abu/AutoScan` uses GameObject hierarchy to exclude items on the ship and cruiser, which may result in *more accurate* counts than vanilla (e.g., scrap on an unmagnetized cruiser will be excluded).

This mod counts items exactly like the vanilla terminal scan, which may result in *inaccurate* counts like vanilla (e.g., scrap on an unmagnetized cruiser will be included).

## Differences from [Happyness/AutoTerminalScan](https://thunderstore.io/c/lethal-company/p/Happyness/AutoTerminalScan/)

`Happyness/AutoTerminalScan` reports item counts categorized by item types (e.g., indoor scrap, bee hives, bird presence).

This mod splits item counts based solely on scan timing.

`Happyness/AutoTerminalScan` reports dungeon type information.

This mod only reports item counts.

## Differences from [PsyKO345/ScrapCounter](https://thunderstore.io/c/lethal-company/p/PsyKO345/ScrapCounter/)

`PsyKO345/ScrapCounter` splits item counts based on the item type.

This mod splits item counts based solely on scan timing.

`PsyKO345/ScrapCounter` uses GameObject hierarchy to exclude items on the cruiser, which may result in *more accurate* counts than vanilla (e.g., scrap on an unmagnetized cruiser will be excluded).

This mod counts items exactly like the vanilla terminal scan, which may result in *inaccurate* counts like vanilla (e.g., scrap on an unmagnetized cruiser will be included).

`PsyKO345/ScrapCounter` displays scrap counts using a custom UI element.

This mod sends scrap counts to the chat.
