# AutoTerminalScanClassic

A Lethal Company mod that scans the number of scraps on the moon and sends it to the chat at the start of each day, just like the terminal scan technique.

## Differences from [abu/AutoScan](https://thunderstore.io/c/lethal-company/p/abu/AutoScan/)

`abu/AutoScan` splits item counts based on the item type.

This mod splits item counts based solely on scan timing, just like vanilla.

`abu/AutoScan` uses GameObject hierarchy to exclude items on the ship and cruiser, which may result in *more accurate* counts than vanilla (e.g., scrap on an unmagnetized cruiser will be excluded).

This mod counts items exactly like the vanilla terminal scan, which may result in *inaccurate* counts like vanilla (e.g., scrap on an unmagnetized cruiser will be included).

## Differences from [Happyness/AutoTerminalScan](https://thunderstore.io/c/lethal-company/p/Happyness/AutoTerminalScan/)

`Happyness/AutoTerminalScan` reports item counts categorized by item types (e.g., indoor scrap, bee hives, bird presence).

This mod splits item counts based solely on scan timing, just like vanilla.

`Happyness/AutoTerminalScan` reports dungeon type information that cannot be determined by the vanilla terminal scan.

This mod only reports item counts.
