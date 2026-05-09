#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using Unity.Netcode;

namespace AutoTerminalScanClassic.Interop.Game.Adapters;

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
