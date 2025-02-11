#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class ServerPush
    {
        public class ServerPushMessage
        {
            public class ServerPushPopup
            {
                public class Message
                {
                    public string title;
                    public string message;
                }

                public string defaultLanguage;
                public Dictionary<string, Message> messages;
            }

            public CommonResponse.Header header;
            public string result;
            public ServerPushPopup popup;
        }

        private static readonly ServerPush instance = new ServerPush();

        public static ServerPush Instance
        {
            get { return instance; }
        }

        private HashSet<string> stampSet = new HashSet<string>();

        public ServerPush()
        {
           
        }

        public void OnServerPush(string response)
        {
            GamebaseLog.Debug(response, this);

            ServerPushMessage message = JsonMapper.ToObject<ServerPushMessage>(response);
            CommonResponse.Header.ServerPush serverPush = message.header.serverPush;

            if (serverPush.type.Equals(GamebaseServerPushType.APP_KICKOUT) == true
                && string.IsNullOrEmpty(GamebaseImplementation.Instance.GetUserID()) == true)
            {
                return;
            }

            if (stampSet.Add(serverPush.stamp) == false)
            {
                return;
            }

            if (serverPush.stopHeartbeat == true)
            {
                Heartbeat.Instance.StopHeartbeat();
                Introspect.Instance.StopIntrospect();
            }            

            if (serverPush.logout == true)
            {
                Gamebase.Logout(null);
            }

            if (serverPush.disconnect == true)
            {
                WebSocket.Instance.Disconnect();
            }

            //------------------------------
            //
            //  AddServerPushEvent(Lagacy)
            //
            //------------------------------
            SendServerPushEvent(serverPush.type, message.result);

            //------------------------------
            //
            //  AddEventHandler
            //
            //------------------------------
            GamebaseResponse.Event.GamebaseEventServerPushData eventData = new GamebaseResponse.Event.GamebaseEventServerPushData
            {
                extras = message.result,
                popup = JsonMapper.ToObject<GamebaseResponse.Event.GamebaseEventServerPushData.ServerPushPopup>(JsonMapper.ToJson(message.popup))
            };

            if (serverPush.type.Equals(GamebaseServerPushType.APP_KICKOUT) == true)
            {
                SendEventMessage(GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT_MESSAGE_RECEIVED, eventData);
            }

            if (serverPush.popup == true)
            {
                GamebaseSystemPopup.Instance.ShowServerPushPopup(message.popup, () => 
                {
                    SendEventMessage(ConvertServerPushTypeToEventCategoryType(serverPush.type), eventData);
                });
            }
            else
            {
                SendEventMessage(ConvertServerPushTypeToEventCategoryType(serverPush.type), eventData);
            }
        }

        private void SendServerPushEvent(string type, string result)
        {
            GamebaseLog.Debug(string.Format("type:{0}, result:{1}", type, result), this);

            GamebaseResponse.SDK.ServerPushMessage serverPushMessage = new GamebaseResponse.SDK.ServerPushMessage
            {
                type = type,
                data = result
            };

            GamebaseServerPushEventManager.Instance.OnServerPushEvent(serverPushMessage);
        }

        private void SendEventMessage(string category, GamebaseResponse.Event.GamebaseEventServerPushData eventData)
        {
            GamebaseResponse.Event.GamebaseEventMessage eventMessage = new GamebaseResponse.Event.GamebaseEventMessage
            {
                category = category,
                data = JsonMapper.ToJson(eventData)
            };

            GamebaseEventHandlerManager.Instance.OnEventHandler(eventMessage);
        }

        private string ConvertServerPushTypeToEventCategoryType(string serverPushType)
        {
            switch (serverPushType)
            {
                case GamebaseServerPushType.APP_KICKOUT:
                    {
                        return GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT;
                    }
                case GamebaseServerPushType.TRANSFER_KICKOUT:
                    {
                        return GamebaseEventCategory.SERVER_PUSH_TRANSFER_KICKOUT;
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }
    }   
}
#endif