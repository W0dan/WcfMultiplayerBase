using System.ServiceModel;

namespace MultiplayerContracts
{
    public interface IPlayerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnChatMessageReceived(string player, string message);
    }
}