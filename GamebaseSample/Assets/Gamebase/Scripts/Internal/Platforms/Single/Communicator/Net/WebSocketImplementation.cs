#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections;
using System.Collections.Generic;
using Toast.Gamebase.WebSocketSharp;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class WebSocketImplementation : IWebSocket
    {
        private string domain;

        private string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return domain = typeof(WebSocketImplementation).Name;
                }

                return domain;
            }
        }

        private WebSocketSharp.WebSocket socket;
        private Queue<string> messages = new Queue<string>();
        private string errorMessage;
        private string socketAdress;
        private GamebaseError disconnectError;

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

        private bool changeDomain = false;

        public IEnumerator Connect(GamebaseCallback.ErrorDelegate callback)
        {
            CreateSocket();
            socket.ConnectAsync();

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            while (
                disconnectError == null &&
                IsConnected() == false &&
                string.IsNullOrEmpty(GetErrorMessage()) == true &&
                watch.Elapsed.Seconds < CommunicatorConfiguration.connectionTimeout)
            {
                yield return null;
            }

            watch.Stop();

            GamebaseError error = null;

            if (string.IsNullOrEmpty(GetErrorMessage()) == false)
            {
                error = new GamebaseError(GamebaseErrorCode.SOCKET_ERROR, Domain, string.Format("{0}. {1}", GamebaseStrings.SOCKET_CONNECTION_FAILED, GetErrorMessage()));
            }
            else
            {
                if (watch.Elapsed.Seconds >= CommunicatorConfiguration.connectionTimeout)
                {
                    error = new GamebaseError(GamebaseErrorCode.SOCKET_ERROR, Domain, GamebaseStrings.SOCKET_CONNECTION_TIMEOUT);
                }
            }

            if (disconnectError != null)
            {
                // 잘못된 도메인의 경우 서브 도메인으로 변경한다.
                // ex)
                //  1. 잘못된 포트 번호
                //  2. 404
                //  3. 메인 도메인 장애
                if (disconnectError.code == 1006)
                {
                    error = new GamebaseError(GamebaseErrorCode.SOCKET_ERROR, Domain, error: disconnectError);
                    disconnectError = null;

                    var prevDomain = Lighthouse.GetAddress();
                    socketAdress = Lighthouse.GetNextAddress();

                    GamebaseLog.Warn(string.Format("Change domain. prev domain:{0}, next domain:{1}", prevDomain, socketAdress), this);
                    
                    if (string.IsNullOrEmpty(socketAdress) == true)
                    {
                        // 마지막 서브 도메인. (도메인 변경 안됨)
                        // 오류 코드를 전달하고 지표를 전송한다.
                        GamebaseIndicatorReport.AddIndicatorItem(new GamebaseIndicatorReport.IndicatorItem
                        {
                            logType = GamebaseIndicatorReportType.LogType.NETWORK,
                            stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_NETWORK_DOMAIN_CONNECTION_FAILED,
                            logLevel = GamebaseIndicatorReportType.LogLevel.ERROR,
                            error = error
                        });
                    }
                    else
                    {
                        // 서브 도메인 변경 후 재연결을 시도한다.
                        Close();
                        changeDomain = true;

                        yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, Connect(callback));
                        yield break;
                    }
                }
            }

            if (error == null && changeDomain == true)
            {
                GamebaseIndicatorReport.AddIndicatorItem(new GamebaseIndicatorReport.IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.NETWORK,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_NETWORK_CHANGE_DOMAIN_SUCCESS,
                    logLevel = GamebaseIndicatorReportType.LogLevel.INFO,
                    customFields = new Dictionary<string, string>()
                    {
                        { GamebaseIndicatorReportType.AdditionalKey.GB_DOMAIN, Lighthouse.GetAddress() }
                    }
                });

                changeDomain = false;
            }

            Lighthouse.ResetAddress();
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
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false)
            {
                if (UnityEditor.EditorApplication.isPlaying == true)
                {
                    Close();
                }
            }
        }
#endif

#endif

        public IEnumerator Reconnect(GamebaseCallback.ErrorDelegate callback)
        {
            if (socket == null)
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
            if (IsConnected() == false)
            {
                yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, Reconnect((error) =>
                {
                    callback(error);

                    if (error == null)
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
            if (recvPolling != null)
            {
                StopRecvPolling();
            }
            recvPolling = GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, RecvPolling());
        }

        public void StopRecvPolling()
        {
            if (recvPolling != null)
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
                    yield return new WaitForSecondsRealtime(WebSocket.RESPONSE_DELAY);
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
                    if(string.IsNullOrEmpty(response) == false)
                    {
                        recvCallback(response);
                    }                    
                }
            }
        }

        public void Close()
        {
            if (socket == null)
            {
                return;
            }

            RemoveEvents();

            if (IsConnected() == false)
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
            if (socket == null)
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
            GamebaseLog.Debug(string.Format("socket disconnected. Code:{0}, Reason:{1}", e.Code, e.Reason), this);
            disconnectError = new GamebaseError(e.Code, Domain, e.Reason);
        }
        #endregion
    }
}
#endif