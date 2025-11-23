#nullable enable

using BepInEx.Logging;

namespace AutoTerminalScanClassic.Utils;

internal static class ChatUtils
{
    internal static ManualLogSource Logger => AutoTerminalScanClassic.Logger!;

    public static bool SendChatToSelfOnly(string message)
    {
        var hudManager = HUDManager.Instance;
        if (hudManager == null)
        {
            Logger.LogError("HUDManager.Instance is null.");
            return false;
        }

        var gameNetworkManager = GameNetworkManager.Instance;
        if (gameNetworkManager == null)
        {
            Logger.LogError("GameNetworkManager.Instance is null.");
            return false;
        }

        var localPlayerController = gameNetworkManager.localPlayerController;
        if (localPlayerController == null)
        {
            Logger.LogError("gameNetworkManager.localPlayerController is null.");
            return false;
        }

        var localPlayerUsername = localPlayerController.playerUsername;

        hudManager.AddChatMessage(
            message,
            localPlayerUsername
        );

        return true;
    }

    public static bool SendChatToEveryone(string message)
    {
        var hudManager = HUDManager.Instance;
        if (hudManager == null)
        {
            Logger.LogError("HUDManager.Instance is null.");
            return false;
        }

        var gameNetworkManager = GameNetworkManager.Instance;
        if (gameNetworkManager == null)
        {
            Logger.LogError("GameNetworkManager.Instance is null.");
            return false;
        }

        var localPlayerController = gameNetworkManager.localPlayerController;
        if (localPlayerController == null)
        {
            Logger.LogError("gameNetworkManager.localPlayerController is null.");
            return false;
        }

        var localPlayerId = (int)localPlayerController.playerClientId;

        hudManager.AddTextToChatOnServer(
            message,
            localPlayerId
        );

        return true;
    }
}
