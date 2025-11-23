#nullable enable

using BepInEx.Logging;
using Unity.Netcode;

namespace AutoTerminalScanClassic.Utils;

internal static class NetworkUtils
{
    internal static ManualLogSource Logger => AutoTerminalScanClassic.Logger!;

    public static bool IsServer()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Logger.LogError("NetworkManager.Singleton is null.");
            return false;
        }

        return networkManager.IsServer;
    }

    public static bool IsHost()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Logger.LogError("NetworkManager.Singleton is null.");
            return false;
        }

        return networkManager.IsHost;
    }

    public static bool IsClient()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Logger.LogError("NetworkManager.Singleton is null.");
            return false;
        }

        return networkManager.IsClient;
    }
}
