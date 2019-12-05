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

        public enum PollingIntervalType
        {
            SHORT_INTERVAL,
            LONG_INTERVAL
        }

        public const float LONG_INTERVAL            = 1f;
        public const float RETRY_CONNECT_DELAY      = 0.5f;        
        
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
            if (true == string.IsNullOrEmpty(Lighthouse.URI))
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

            if (false == isRequestQueueCheck)
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
                if (true == reachable)
                {
                    RequestEnqueue(new RequestQueueItem(itemLength++, vo, callback));
                }
                else
                {
                    if (null == callback)
                    {
                        return;
                    }

                    callback(string.Empty, new GamebaseError(GamebaseErrorCode.SOCKET_ERROR, transactionId: vo.transactionId));
                }
            }));
        }

        private IEnumerator Send()
        {
            if (false == socket.IsConnected())
            {
                yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, RetryConnect());
                yield break;
            }
            else
            {
                if (0 < requestQueueItem.retryCount)
                {
                    GamebaseLog.Debug(string.Format("Reconnect succeeded. Index of queue item:{0}", requestQueueItem.index), this);
                }
            }
            
            GamebaseLog.Debug(string.Format("request:{0}", GamebaseJsonUtil.ToPrettyJsonString(requestQueueItem.requestVO)), this);
            yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, socket.Send(JsonMapper.ToJson(requestQueueItem.requestVO), (error) =>
            {
                if (null == error)
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
            while (true == isTimeOutCheck)
            {
                if (waitTime >= CommunicatorConfiguration.timeout)
                {
                    GamebaseLog.Debug(string.Format("Time Out TransactionId : {0}", requestQueueItem.requestVO.transactionId.ToString()), this);
                    isTimeOutCheck = false;
                    RetryItem();
                    yield break;
                }
                waitTime += Time.unscaledDeltaTime;
                yield return null;
            }
            GamebaseLog.Debug("Finish time out check.", this);
        }

        private IEnumerator RetryConnect()
        {
            if (requestQueueItem.retryCount < requestQueueItem.retryLimits)
            {
                yield return new WaitForSecondsRealtime(RETRY_CONNECT_DELAY);
                yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, InternetReachability((reachable) => 
                {
                    if (true == reachable)
                    {
                        GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, socket.Reconnect((error) =>
                        {
                            if (true == Gamebase.IsSuccess(error))
                            {
                                RequestEnqueue(requestQueueItem);
                                requestQueueItem = null;
                                return;
                            }

                            RetryItem();
                        }));
                    }
                    else
                    {
                        RetryItem();
                    }

                    GamebaseLog.Debug(string.Format("index:{0}, apiId:{1}, retryCount:{2}, internetReachability:{3}", requestQueueItem.index, requestQueueItem.requestVO.apiId, requestQueueItem.retryCount, reachable), this);
                }));
            }
            else
            {
                GamebaseLog.Debug(string.Format("Reconnect failed. Index of queue item:{0}", requestQueueItem.index), this);
                requestQueueItem.callback(string.Empty, new GamebaseError(GamebaseErrorCode.SOCKET_ERROR));
                requestQueueItem = null;
            }
        }

        private void RetryItem()
        {
            RequestQueueItem[] itemList = null;
            if(0 < requestQueue.Count)
            {
                itemList = requestQueue.ToArray();
            }

            requestQueueItem.retryCount += 1;
            RequestEnqueue(requestQueueItem);
            requestQueueItem = null;

            if(null != itemList)
            {
                foreach(var item in itemList)
                {
                    requestQueue.Enqueue(item);
                }
            }
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
                requestQueueItem.callback(response, null);
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
            while (true == isRequestQueueCheck)
            {
                if (0 == requestQueue.Count)
                {
                    isRequestQueueCheck = false;
                    yield return null;
                }
                else
                {
                    if (null == requestQueueItem)
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
                
            while(false == receivedCallback)
            {
                yield return null;
            }
#else
            internetReachability = Gamebase.Network.IsConnected();
#endif
            callback(internetReachability);
            yield return null;
        }

        public bool IsWebSocketConnected()
        {
            return socket.IsConnected();
        }
    }
}
#endif