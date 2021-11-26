#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System;
using System.Collections;
using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class WebSocket
    {
        private static readonly WebSocket instance = new WebSocket();

        public static WebSocket Instance
        {
            get { return instance; }
        }

        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return typeof(WebSocket).Name;
                }

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public enum PollingIntervalType
        {
            SHORT_INTERVAL,
            LONG_INTERVAL
        }

        public const float LONG_INTERVAL            = 1f;
        public const float RETRY_CONNECT_DELAY      = 0.5f;
        public const float RESPONSE_DELAY           = 0.1f;

        private IWebSocket socket;
        
        private Queue<RequestQueueItem> requestQueue;
        private RequestQueueItem requestQueueItem;

        private int itemLength                      = 0;

        private bool isTimeOutCheck                 = false;
        private bool isRequestQueueCheck            = false;

        public class RequestQueueItem
        {
            public WebSocketRequest.RequestVO requestVO;
            public GamebaseCallback.GamebaseDelegate<string> callback;
            public int retryCount;
            public int retryLimits = 5;
            public int index;

            public RequestQueueItem(int index, WebSocketRequest.RequestVO requestVO, GamebaseCallback.GamebaseDelegate<string> callback)
            {
                this.requestVO = requestVO;
                this.callback = callback;

                this.index = index;
                retryCount = 0;                
            }
        }

        public WebSocket()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            socket = new WebSocketImplementation();
#elif !UNITY_EDITOR || UNITY_WEBGL
            socket = new WebGLWebSocketImplementation();
#endif
            socket.SetRecvEvent(RecvEvent);
            requestQueue = new Queue<RequestQueueItem>();                    
        }
        
        public void Initialize()
        {
            if (string.IsNullOrEmpty(Lighthouse.URI) == true)
            {
                Lighthouse.URI = Lighthouse.ZoneType.REAL.GetEnumMemberValue();
            }
            socket.Initialize(Lighthouse.URI);
        }

        public IEnumerator Connect(GamebaseCallback.ErrorDelegate callback)
        {
            yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, socket.Connect(callback));            
        }

        public void Disconnect()
        {            
            socket.Close();
        }

        private void RequestEnqueue(RequestQueueItem item)
        {
            requestQueue.Enqueue(item);

            if (isRequestQueueCheck == false)
            {
                isRequestQueueCheck = true;
                GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, UpdateQueue());
            }            
        }

        private RequestQueueItem RequestDequeue()
        {
            RequestQueueItem item = requestQueue.Dequeue();
            return item;
        }        

        public void Request(WebSocketRequest.RequestVO vo, GamebaseCallback.GamebaseDelegate<string> callback)
        {
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, InternetReachability((reachable) =>
            {
                if (reachable == true)
                {
                    RequestEnqueue(new RequestQueueItem(itemLength++, vo, callback));
                }
                else
                {
                    if (callback == null)
                    {
                        return;
                    }

                    callback(string.Empty, new GamebaseError(GamebaseErrorCode.SOCKET_ERROR, domain: Domain, transactionId: vo.transactionId, message: GamebaseStrings.SOCKET_NO_INTERNET_CONNECTION));
                }
            }));
        }

        private IEnumerator Send()
        {
            if (requestQueueItem.retryCount >= requestQueueItem.retryLimits)            
            {
                // requestQueueItem.retryLimits 만큼 재전송 실패
                var error = new GamebaseError(GamebaseErrorCode.SOCKET_ERROR)
                {
                    domain = Domain,
                    message = GamebaseStrings.SOCKET_SEND_FAIL,
                    transactionId = requestQueueItem.requestVO.transactionId
                };

                requestQueueItem.callback(string.Empty, error);
                requestQueueItem = null;
                yield break;
            }
         
            if (socket.IsConnected() == false)
            {
                GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, RetryConnect());
                yield break;
            }
            GamebaseLog.Debug(string.Format("Send Count:{0}", requestQueueItem.retryCount), this);
            GamebaseLog.Debug(string.Format("request:{0}", GamebaseJsonUtil.ToPrettyJsonString(requestQueueItem.requestVO)), this);
         
            yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, socket.Send(JsonMapper.ToJson(requestQueueItem.requestVO), (error) =>
            {
                if (error == null)
                {
                    return;
                }                    
                
                error.transactionId = requestQueueItem.requestVO.transactionId;
                requestQueueItem.callback(string.Empty, error);                
            }));

            socket.SetPollingInterval(PollingIntervalType.SHORT_INTERVAL);
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, CheckTimeout());
        }

        private IEnumerator CheckTimeout()
        {            
            float waitTime = 0;
            isTimeOutCheck = true;
            while (isTimeOutCheck == true)
            {
                if (waitTime >= CommunicatorConfiguration.timeout)
                {
                    isTimeOutCheck = false;
                    if (requestQueueItem == null)
                    {
                        GamebaseLog.Warn("requestQueueItem is null", this);
                    }
                    else
                    {
                        GamebaseLog.Debug(string.Format("Time Out TransactionId : {0}", requestQueueItem.requestVO.transactionId.ToString()), this);
                        RetryItem();
                    }                 

                    yield break;
                }
                waitTime += Time.unscaledDeltaTime;
                yield return null;
            }
            GamebaseLog.Debug("Finish time out check.", this);
        }

        private IEnumerator RetryConnect()
        {            
            yield return new WaitForSecondsRealtime(RETRY_CONNECT_DELAY);
            yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, InternetReachability((reachable) =>
            {
                if (reachable == true)
                {
                    GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, socket.Reconnect((error) =>
                    {
                        if (requestQueueItem != null)
                        {
                            RetryItem();
                        }

                        if (Gamebase.IsSuccess(error) == false)
                        {
                            GamebaseLog.Debug("Reconnect failed.", this);
                        }
                    }));
                }
                else
                {
                    if (requestQueueItem != null)
                    {
                        // 인터넷에 열결되지 않음
                        var error = new GamebaseError(GamebaseErrorCode.SOCKET_ERROR)
                        {
                            domain = Domain,
                            message = GamebaseStrings.SOCKET_NO_INTERNET_CONNECTION,
                            transactionId = requestQueueItem.requestVO.transactionId
                        };

                        requestQueueItem.callback(string.Empty, error);
                        requestQueueItem = null;
                    }
                }                    
            }));         
        }
                
        private void RetryItem()
        {
            requestQueueItem.retryCount += 1;
            // 순차 보장을 위한 처리
            // Queue 백업
            RequestQueueItem[] itemList = null;
            if(0 < requestQueue.Count)
            {
                itemList = requestQueue.ToArray();
            }

            // Queue Clear
            requestQueue.Clear();

            // Queue에 순서에 맞게 추가
            RequestEnqueue(requestQueueItem);
            if(itemList != null)
            {
                foreach(var item in itemList)
                {
                    requestQueue.Enqueue(item);
                }
            }

            requestQueueItem = null;
        }

        private void RecvEvent(string response)
        {
            GamebaseLog.Debug(GamebaseJsonUtil.ToPrettyJsonString(response), this);

            ProtocolResponse protocol = JsonMapper.ToObject<ProtocolResponse>(response);

            if (null != protocol.header.serverPush)
            {
                ServerPush.Instance.OnServerPush(response);
                return;
            }
            
            if(protocol.header.transactionId == requestQueueItem.requestVO.transactionId)
            {                
                if (requestQueueItem.callback != null)
                {
                    requestQueueItem.callback(response, null);
                }
                isTimeOutCheck = false;                
                requestQueueItem = null;
                socket.SetPollingInterval(PollingIntervalType.LONG_INTERVAL);
            }
            else
            {
                GamebaseLog.Debug(string.Format("Missing response TransactionId : {0}", protocol.header.transactionId.ToString()), this);
            }
        }        

        private IEnumerator UpdateQueue()
        {
            while (isRequestQueueCheck == true)
            {
                if (0 == requestQueue.Count)
                {
                    isRequestQueueCheck = false;
                    yield return null;
                }
                else
                {
                    if (requestQueueItem == null)
                    {
                        requestQueueItem = RequestDequeue();
                        yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, Send());
                    }

                    yield return null;
                }
            }
        }

        private IEnumerator InternetReachability(Action<bool> callback)
        {
            bool internetReachability = true;
#if !UNITY_EDITOR && UNITY_WEBGL
            bool receivedCallback = false;
                
            Gamebase.Network.IsConnected((reachable) => 
            {
                internetReachability = reachable;
                receivedCallback = true;
            });
                
            while(receivedCallback == false)
            {
                yield return null;
            }
#else
            internetReachability = Gamebase.Network.IsConnected();
#endif
            
            if (callback != null)
            {
                callback(internetReachability);
            }
            yield return null;
        }

        public bool IsWebSocketConnected()
        {
            return socket.IsConnected();
        }
    }
}
#endif