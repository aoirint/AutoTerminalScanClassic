#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using Unity.Netcode;

namespace AutoTerminalScanClassic.Interop.Game.Adapters;

/// <summary>
/// Owns Unity Netcode role checks used by scan and broadcast policy.
/// </summary>
/// <remarks>
/// Missing NetworkManager data fails closed at this boundary so Core use cases
/// do not perform scan or broadcast work from an unknown network role.
/// </remarks>
internal sealed class NetworkAdapter
{
    private readonly IPluginLogger logger;

    public NetworkAdapter(IPluginLogger logger)
    {
        this.logger = logger;
    }

    public bool IsClient()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            logger.LogError("NetworkManager.Singleton is null.");
            return false;
        }

        return networkManager.IsClient;
    }

    public bool IsHost()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            logger.LogError("NetworkManager.Singleton is null.");
            return false;
        }

        return networkManager.IsHost;
    }
}
