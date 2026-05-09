#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using UnityEngine;

namespace AutoTerminalScanClassic.Interop.Game.Adapters;

/// <summary>
/// Owns Unity-side item counting for the terminal scan-equivalent result.
/// </summary>
/// <remarks>
/// Core only needs a nullable count; this adapter owns scene object enumeration
/// and the base-game item filters that define what the scan command reports.
/// </remarks>
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
            // Treat missing item metadata as an unreliable scan rather than
            // silently undercounting and sending a misleading chat delta.
            var itemProperties = grabbableObject.itemProperties;
            if (itemProperties == null)
            {
                logger.LogError("grabbableObject.itemProperties is null.");
                return null;
            }

            // Based on the terminal `scan` command logic in the base game:
            // count scrap still on the moon, excluding items already in the
            // ship/elevator because those should not appear as remaining loot.
            if (itemProperties.isScrap && !grabbableObject.isInShipRoom && !grabbableObject.isInElevator)
            {
                scannedItemCount++;
            }
        }

        logger.LogDebug($"Scanned and found {scannedItemCount} items.");
        return scannedItemCount;
    }
}
