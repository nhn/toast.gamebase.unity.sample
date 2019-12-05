#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public interface IWebSocket
    {
        void Initialize(string socketAdress);
        IEnumerator Connect(GamebaseCallback.ErrorDelegate callback);
        IEnumerator Reconnect(GamebaseCallback.ErrorDelegate callback);
        IEnumerator Send(string jsonString, GamebaseCallback.ErrorDelegate callback);
        void SetRecvEvent(GamebaseCallback.DataDelegate<string> callback);
        void Close();
        bool IsConnected();
        string GetErrorMessage();
        void StartRecvPolling();
        void StopRecvPolling();
        void SetPollingInterval(WebSocket.PollingIntervalType type);
    }
}
#endif