#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public sealed class Introspect
    {
        private const int MAX_RETRY_COUNT = 2;

        private static readonly Introspect instance = new Introspect();

        public static Introspect Instance
        {
            get { return instance; }
        }
                
        private float refrashIntervalTime;
        private float retryIntervalTime;
        private float sentIntervalTime;
        private float sentStandardTime;

        private int retryCount = 0;

        public Introspect()
        {
        }

        public void SetInterval(float intervalTime)
        {
            if(intervalTime <= 0)
            {
                return;
            }

            refrashIntervalTime = intervalTime;
            retryIntervalTime = refrashIntervalTime * 0.25f;
        }

        public void StartIntrospect()
        {
            if (IsSendable() == false)
            {
                return;
            }

            GamebaseLog.Debug("Start Introspect", this);

            sentIntervalTime = refrashIntervalTime;
            sentStandardTime = Time.realtimeSinceStartup;

            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.INTROSPECT_TYPE, ExecuteIntrospect());
        }

        public void StopIntrospect()
        {
            GamebaseLog.Debug("Stop Introspect", this);
                        
            GamebaseCoroutineManager.StopAllCoroutines(GamebaseGameObjectManager.GameObjectType.INTROSPECT_TYPE);
        }

        private IEnumerator ExecuteIntrospect()
        {
            while(true)
            {
                if(sentStandardTime + sentIntervalTime <= Time.realtimeSinceStartup)
                {
                    sentStandardTime = Time.realtimeSinceStartup;
                    SendIntrospect();
                }                
                yield return null;
            }            
        }

        private void SendIntrospect()
        {
            GamebaseLog.Debug("Send Introspect", this);
            var requestVO = IntrospectMessage.MakeIntrospectMessage();
            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseError introspectError = error;

                if ( introspectError == null )
                {
                    var vo = JsonMapper.ToObject<LaunchingResponse.IntrospectInfo>(response);
                    if (vo.header.isSuccessful == true)
                    {
                        GamebaseLog.Debug("Send Introspect succeeded", this);
                        retryCount = 0;
                        sentIntervalTime = refrashIntervalTime;
                        return;
                    }
                    else
                    {
                        introspectError = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, string.Empty);
                    }
                }

                if (introspectError != null)
                {
                    if (introspectError.code == GamebaseErrorCode.SOCKET_RESPONSE_TIMEOUT ||
                        introspectError.code == GamebaseErrorCode.SOCKET_ERROR)
                    {
                        sentIntervalTime = retryIntervalTime;
                        retryCount++;

                        if (retryCount <= MAX_RETRY_COUNT)
                        {                            
                            return;
                        }
                    }
                    
                    retryCount = 0;

                    StopIntrospect();
                    SendObserverEvent(introspectError);
                }
            });
        }

        private void SendObserverEvent(GamebaseError error)
        {
            GamebaseResponse.SDK.ObserverMessage observerMessage = new GamebaseResponse.SDK.ObserverMessage()
            {
                type = GamebaseObserverType.INTROSPECT,
                data = new Dictionary<string, object>()
                    {
                        {
                            "error", error
                        }
                    }
            };

            GamebaseObserverManager.Instance.OnObserverEvent(observerMessage);
            SendEventMessage(observerMessage);
        }

        private void SendEventMessage(GamebaseResponse.SDK.ObserverMessage message)
        {
            GamebaseResponse.Event.GamebaseEventObserverData observerData = new GamebaseResponse.Event.GamebaseEventObserverData();

            object extrasData = null;            
            if (message.data.TryGetValue("error", out extrasData) == true)
            {
                observerData.extras = ((GamebaseError)extrasData).ToString();
            }

            GamebaseResponse.Event.GamebaseEventMessage eventMessage = new GamebaseResponse.Event.GamebaseEventMessage();
            eventMessage.category = string.Format("observer{0}", GamebaseStringUtil.Capitalize(message.type));
            eventMessage.data = JsonMapper.ToJson(observerData);

            GamebaseEventHandlerManager.Instance.OnEventHandler(eventMessage);
        }

        private bool IsSendable()
        {
            if (IsLoggedin() == false)
            {
                return false;
            }

            if(refrashIntervalTime <= 0)
            {
                return false;
            }

            return true;
        }

        private bool IsLoggedin()
        {
            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                return false;
            }

            return true;
        }
    }
}
#endif
