using System.ServiceModel;

namespace MultiplayerContracts
{
    [ServiceContract(CallbackContract = typeof(IPlayerCallback))]
    public interface IMultiplayerService
    {
        [OperationContract]
        string Join(string player);

        [OperationContract(IsOneWay = true)]
        void SendChatMessage(string playertoken, string message);
    }
}
