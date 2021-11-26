#if UNITY_WEBGL
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using Toast.Gamebase.WebSocketSharp;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class WebGLWebSocketImplementation : IWebSocket
    {
        private const int DEFAULT_NATIVE_REF = -1;
        private int nativeRef = DEFAULT_NATIVE_REF;
        private string socketAdress;

        private GamebaseCallback.DataDelegate<string> recvCallback;
        private WebSocket.PollingIntervalType pollingSpeedType = WebSocket.PollingIntervalType.LONG_INTERVAL;
        private Coroutine recvPolling = null;

        public WebGLWebSocketImplementation()
        {
            Close();
        }

#region DllImport
        [DllImport("__Internal")]
        private static extern int SocketCreate(string url);
        [DllImport("__Internal")]
        private static extern int SocketState(int socketInstance);
        [DllImport("__Internal")]
        private static extern void SocketSend(int socketInstance, string jsonString);
        [DllImport("__Internal")]
        private static extern string SocketRecv(int socketInstance);
        [DllImport("__Internal")]
        private static extern void SocketClose(int socketInstance);
        [DllImport("__Internal")]
        private static extern int SocketError(int socketInstance, byte[] ptr, int length);
#endregion

        public void Initialize(string socketAdress)
        {
            this.socketAdress = socketAdress;            
        }

        private void CreateSocket()
        {
            nativeRef = SocketCreate(socketAdress);
        }

        public IEnumerator Connect(GamebaseCallback.ErrorDelegate callback)
        {
            CreateSocket();

            float waitTime = 0;
            while (false == IsConnected() && null == GetErrorMessage() && waitTime < CommunicatorConfiguration.connectionTimeout)
            {
                waitTime += Time.unscaledDeltaTime;
                yield return null;
            }

            GamebaseError error = null;

            if (false == string.IsNullOrEmpty(GetErrorMessage()))
            {
                error = new GamebaseError(GamebaseErrorCode.SOCKET_ERROR, message: GetErrorMessage());
            }
            else
            {
                if (waitTime >= CommunicatorConfiguration.connectionTimeout)
                {
                    error = new GamebaseError(GamebaseErrorCode.SOCKET_ERROR);
                }
                else
                {
                    GamebaseLog.Debug("socket connected.", this);
                }
            }

            StartRecvPolling();

            callback(error);
        }

        public IEnumerator Reconnect(GamebaseCallback.ErrorDelegate callback)
        {
            GamebaseLog.Debug(string.Format("SocketState:{0}", SocketState(nativeRef)), this);

            if (DEFAULT_NATIVE_REF != nativeRef)
            {
                if ((int)WebSocketState.Open == SocketState(nativeRef))
                {
                    callback(null);
                    yield break;
                }
            }

            Close();
            yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, Connect(callback));
        }

        public IEnumerator Send(string jsonString, GamebaseCallback.ErrorDelegate callback)
        {
            if (false == IsConnected())
            {
                yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, Reconnect((error) =>
                {
                    callback(error);

                    if (null == error)
                    {
                        SocketSend(nativeRef, jsonString);
                    }
                }));
            }
            else
            {
                SocketSend(nativeRef, jsonString);
            }
        }

        public void SetRecvEvent(GamebaseCallback.DataDelegate<string> callback)
        {
            recvCallback = callback;
        }
        
        public void StartRecvPolling()
        {
            if (null != recvPolling)
            {
                StopRecvPolling();
            }
            recvPolling = GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, RecvPolling());
        }

        public void StopRecvPolling()
        {
            if (null != recvPolling)
            {
                GamebaseCoroutineManager.StopCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, recvPolling);
                recvPolling = null;
            }
        }

        public void SetPollingInterval(WebSocket.PollingIntervalType type)
        {
            pollingSpeedType = type;
        }

        private IEnumerator RecvPolling()
        {
            while (true)
            {
                RecvEventCall();

                if (WebSocket.PollingIntervalType.SHORT_INTERVAL == pollingSpeedType)
                {
                    yield return null;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(WebSocket.LONG_INTERVAL);
                }
            }
        }

        private void RecvEventCall()
        {
            string response = SocketRecv(nativeRef);
            if(false == string.IsNullOrEmpty(response))
            {
                if(null != recvCallback)
                {
                    recvCallback(response);
                }
            }
        }

        public void Close()
        {
            if (DEFAULT_NATIVE_REF == nativeRef)
            {
                return;
            }

            if (false == IsConnected())
            {
                nativeRef = DEFAULT_NATIVE_REF;
                return;
            }

            SocketClose(nativeRef);
            GamebaseLog.Debug("socket disconnected.", this);
            nativeRef = DEFAULT_NATIVE_REF;

            StopRecvPolling();
        }

        public bool IsConnected()
        {
            if (DEFAULT_NATIVE_REF == nativeRef)
            {
                return false;
            }

            return SocketState(nativeRef) == (int)WebSocketState.Open;
        }

        public string GetErrorMessage()
        {
            const int bufsize = 1024;
            byte[] buffer = new byte[bufsize];
            int result = SocketError(nativeRef, buffer, bufsize);

            if (0 == result)
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(buffer);
        }
    }
}
#endif