#nullable enable

using AutoTerminalScanClassic.Core.Ports;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Reflection;

namespace AutoTerminalScanClassic.Interop.Game.Adapters;

/// <summary>
/// Owns base-game chat APIs for local-only and server-routed scan messages.
/// </summary>
/// <remarks>
/// Core chooses the target; this adapter resolves HUD/player singletons and
/// calls the specific chat API that preserves the configured delivery scope.
/// </remarks>
internal sealed class ChatAdapter
{
    private static readonly MethodInfo? AddChatMessageMethod = AccessTools.Method(
        typeof(HUDManager),
        "AddChatMessage",
        [typeof(string), typeof(string), typeof(int), typeof(bool)]
    );

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

        // v81 makes this HUD-only method private. Reflection keeps the
        // SelfOnly contract without routing the message through the server.
        if (AddChatMessageMethod == null)
        {
            logger.LogError("HUDManager.AddChatMessage(string, string, int, bool) was not found.");
            return false;
        }

        try
        {
            AddChatMessageMethod.Invoke(
                hudManager,
                [message, localPlayerController.playerUsername, -1, false]
            );
        }
        catch (TargetInvocationException exception)
        {
            logger.LogError(
                $"HUDManager.AddChatMessage failed. exception_type={exception.InnerException?.GetType().FullName ?? exception.GetType().FullName}"
            );
            return false;
        }
        catch (Exception exception)
        {
            logger.LogError(
                $"HUDManager.AddChatMessage invocation failed. exception_type={exception.GetType().FullName}"
            );
            return false;
        }

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

        // AddTextToChatOnServer mirrors normal player chat routing and uses the
        // local player's client ID so the message appears under their name.
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
