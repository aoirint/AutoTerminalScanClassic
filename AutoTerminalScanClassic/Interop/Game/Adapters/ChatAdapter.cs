#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using GameNetcodeStuff;

namespace AutoTerminalScanClassic.Interop.Game.Adapters;

internal sealed class ChatAdapter
{
    private readonly IPluginLogger logger;

    public ChatAdapter(IPluginLogger logger)
    {
        this.logger = logger;
    }

    public bool SendChatToSelfOnly(string message)
    {
        var hudManager = HUDManager.Instance;
        if (hudManager == null)
        {
            logger.LogError("HUDManager.Instance is null.");
            return false;
        }

        var localPlayerController = GetLocalPlayerController();
        if (localPlayerController == null)
        {
            return false;
        }

        hudManager.AddChatMessage(
            message,
            localPlayerController.playerUsername
        );

        return true;
    }

    public bool SendChatToEveryone(string message)
    {
        var hudManager = HUDManager.Instance;
        if (hudManager == null)
        {
            logger.LogError("HUDManager.Instance is null.");
            return false;
        }

        var localPlayerController = GetLocalPlayerController();
        if (localPlayerController == null)
        {
            return false;
        }

        hudManager.AddTextToChatOnServer(
            message,
            (int)localPlayerController.playerClientId
        );

        return true;
    }

    private PlayerControllerB? GetLocalPlayerController()
    {
        var gameNetworkManager = GameNetworkManager.Instance;
        if (gameNetworkManager == null)
        {
            logger.LogError("GameNetworkManager.Instance is null.");
            return null;
        }

        var localPlayerController = gameNetworkManager.localPlayerController;
        if (localPlayerController == null)
        {
            logger.LogError("gameNetworkManager.localPlayerController is null.");
            return null;
        }

        return localPlayerController;
    }
}
