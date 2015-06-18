using System.ServiceModel;

namespace MultiplayerClient.Contracts
{
    public interface IPlayerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnChatMessageReceived(string player, string message);
    }
}