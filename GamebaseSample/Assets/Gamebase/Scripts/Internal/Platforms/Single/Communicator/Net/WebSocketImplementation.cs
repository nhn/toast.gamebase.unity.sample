#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class WebSocketImplementation : IWebSocket
    {   
        private WebSocketSharp.WebSocket socket;
        private Queue<string> messages = new Queue<string>();
        private string errorMessage;
        private string socketAdress;

        private GamebaseCallback.DataDelegate<string> recvCallback;
        private WebSocket.PollingIntervalType pollingSpeedType = WebSocket.PollingIntervalType.LONG_INTERVAL;
        private Coroutine recvPolling = null;

        public WebSocketImplementation()
        {
            Close();

#if UNITY_EDITOR

#if UNITY_2017_2_OR_NEWER
            UnityEditor.EditorApplication.playModeStateChanged += WebSocketCloseOnEditor;
#else
            UnityEditor.EditorApplication.playmodeStateChanged -= WebSocketCloseOnEditor;
            UnityEditor.EditorApplication.playmodeStateChanged += WebSocketCloseOnEditor;
#endif

#endif
        }

        public void Initialize(string socketAdress)
        {
            this.socketAdress = socketAdress;
        }

        private void CreateSocket()
        {
            socket = new WebSocketSharp.WebSocket(socketAdress);
            AddEvents();
        }
        
        public IEnumerator Connect(GamebaseCallback.ErrorDelegate callback)
        {
            CreateSocket();

            socket.ConnectAsync();

            float waitTime = 0;
            while (false == IsConnected() && null == GetErrorMessage() && waitTime < CommunicatorConfiguration.connectionTimeout)
            {
                waitTime += Time.unscaledDeltaTime;
                yield return null;
            }

            GamebaseError error = null;

            if (false == string.IsNullOrEmpty(GetErrorMessage()))
            {
                error = new GamebaseError(GamebaseErrorCode.SOCKET_ERROR, message:GetErrorMessage());
            }
            else
            {
                if (waitTime >= CommunicatorConfiguration.connectionTimeout)
                {
                    error = new GamebaseError(GamebaseErrorCode.SOCKET_ERROR);
                }
            }

            StartRecvPolling();

            callback(error);
        }

#if UNITY_EDITOR

#if UNITY_2017_2_OR_NEWER
        private void WebSocketCloseOnEditor(UnityEditor.PlayModeStateChange state)
        {
            if (UnityEditor.PlayModeStateChange.ExitingPlayMode == state)
            {
                Close();
            }
        }
#else
        private void WebSocketCloseOnEditor()
        {
            if (false == UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (true == UnityEditor.EditorApplication.isPlaying)
                {
                    Close();
                }
            }
        }
#endif

#endif

        public IEnumerator Reconnect(GamebaseCallback.ErrorDelegate callback)
        {
            if (null == socket)
            {
                yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, Connect(callback));
                yield break;
            }

            GamebaseLog.Debug(string.Format("socket.ReadyState:{0}", socket.ReadyState), this);

            if (WebSocketState.Open == socket.ReadyState)
            {
                callback(null);
                yield break;
            }

            Close();
            yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, Connect(callback));
        }

        public void SetPollingInterval(WebSocket.PollingIntervalType type)
        {
            pollingSpeedType = type;
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
                        socket.Send(jsonString);
                    }
                }));
            }
            else
            {
                socket.Send(jsonString);
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

        private IEnumerator RecvPolling()
        {
            while (true)
            {
                RecvEventCall();
                                
                if ( WebSocket.PollingIntervalType.SHORT_INTERVAL == pollingSpeedType)
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
            if (0 != messages.Count)
            {
                if (null != recvCallback)
                {
                    string response = messages.Dequeue();
                    if(false == string.IsNullOrEmpty(response))
                    {
                        recvCallback(response);
                    }                    
                }
            }
        }

        public void Close()
        {
            if (null == socket)
            {
                return;
            }

            RemoveEvents();

            if (false == IsConnected())
            {
                socket = null;
                return;
            }   

            socket.Close();
            socket = null;

            StopRecvPolling();
        }

        public bool IsConnected()
        {
            if (null == socket)
            {
                return false;
            }

            return socket.ReadyState == WebSocketState.Open;
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        #region EventHandler
        private delegate void OnMessageDelegate(object sender, MessageEventArgs e);
        private delegate void OnOpenDelegate(object sender, EventArgs e);
        private delegate void OnErrorDelegate(object sender, ErrorEventArgs e);
        private delegate void OnCloseDelegate(object sender, CloseEventArgs e);

        private void AddEvents()
        {
            socket.OnMessage += OnMessage;
            socket.OnOpen += OnOpen;
            socket.OnError += OnError;
            socket.OnClose += OnClose;
        }

        private void RemoveEvents()
        {
            socket.OnMessage -= OnMessage;
            socket.OnOpen -= OnOpen;
            socket.OnError -= OnError;
            socket.OnClose -= OnClose;
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            messages.Enqueue(e.Data);
        }

        private void OnOpen(object sender, EventArgs e)
        {
            GamebaseLog.Debug("socket connected.", this);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            errorMessage = e.Message;
            GamebaseLog.Warn(GamebaseJsonUtil.ToPrettyJsonString(e) ,this);
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            GamebaseLog.Debug("socket disconnected.", this);
        }
        #endregion
    }
}
#endif