#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using UnityEngine;

namespace AutoTerminalScanClassic.Interop.Game.Adapters;

internal sealed class TerminalScanAdapter
{
    private readonly IPluginLogger logger;

    public TerminalScanAdapter(IPluginLogger logger)
    {
        this.logger = logger;
    }

    public int? ScanItemCount()
    {
        var grabbableObjects = Object.FindObjectsOfType<GrabbableObject>();

        var scannedItemCount = 0;
        foreach (var grabbableObject in grabbableObjects)
        {
            var itemProperties = grabbableObject.itemProperties;
            if (itemProperties == null)
            {
                logger.LogError("grabbableObject.itemProperties is null.");
                return null;
            }

            // Based on the terminal `scan` command logic in the base game.
            if (itemProperties.isScrap && !grabbableObject.isInShipRoom && !grabbableObject.isInElevator)
            {
                scannedItemCount++;
            }
        }

        logger.LogDebug($"Scanned and found {scannedItemCount} items.");
        return scannedItemCount;
    }
}
