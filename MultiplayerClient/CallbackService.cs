using System;
using MultiplayerClient.Contracts;
using MultiplayerClient.Extensions;

namespace MultiplayerClient
{
    public class CallbackService : IPlayerCallback
    {
        public event Action<string, string> ChatMessageReceived;

        public void OnChatMessageReceived(string player, string message)
        {
            ChatMessageReceived.Raise(player, message);
        }
    }
}