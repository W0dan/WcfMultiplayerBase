﻿using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using MultiplayerContracts;

namespace MultiplayerServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class MultiplayerService : IMultiplayerService
    {
        private readonly SubscriberCollection<IPlayerCallback> _players = new SubscriberCollection<IPlayerCallback>();

        public string Join(string playerName)
        {
            var joinedPlayer = _players.Add(playerName, OperationContext.Current.GetCallbackChannel<IPlayerCallback>());

            if (joinedPlayer == null)
            {
                return null;
            }

            _players.Send(joinedPlayer.Token, (player, callback) => callback.OnChatMessageReceived(player, string.Format("{0} has joined", joinedPlayer.Name)));

            return joinedPlayer.Token;
        }

        public void SendChatMessage(string playertoken, string message)
        {
            _players.Send(playertoken, (player, callback) => callback.OnChatMessageReceived(player, message));
        }
    }
}
