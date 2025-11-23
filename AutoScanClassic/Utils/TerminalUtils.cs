#nullable enable

using BepInEx.Logging;
using UnityEngine;

namespace AutoScanClassic.Utils;

internal static class TerminalUtils
{
    internal static ManualLogSource Logger => AutoScanClassic.Logger!;

    public static int? ScanItemCount()
    {
        var grabbableObjects = Object.FindObjectsOfType<GrabbableObject>();

        var scannedItemCount = 0;
        foreach (var grabbableObject in grabbableObjects)
        {
            var itemProperties = grabbableObject.itemProperties;
            if (itemProperties == null)
            {
                Logger.LogError("grabbableObject.itemProperties is null.");
                return null;
            }

            // Based on the terminal `scan` command logic in the base game
            if (itemProperties.isScrap && !grabbableObject.isInShipRoom && !grabbableObject.isInElevator)
            {
                scannedItemCount++;
            }
        }

        Logger.LogDebug($"Scanned and found {scannedItemCount} items.");
        return scannedItemCount;
    }
}
