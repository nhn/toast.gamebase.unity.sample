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

            GamebaseSystemPopup.Instance.ShowServerPushPopup(message.popup);            
            
            SendServerPushMessage(serverPush.type, message.result);
        }
        
        private void SendServerPushMessage(string type, string data)
        {
            GamebaseLog.Debug(string.Format("type : {0}, data : {1}", type, data), this);
            GamebaseResponse.SDK.ServerPushMessage serverPushMessage = new GamebaseResponse.SDK.ServerPushMessage();
            serverPushMessage.type = type;
            serverPushMessage.data = data;

            GamebaseServerPushEventManager.Instance.OnServerPushEvent(serverPushMessage);

            SendEventMessage(serverPushMessage);
        }

        private void SendEventMessage(GamebaseResponse.SDK.ServerPushMessage message)
        {
            GamebaseResponse.Event.GamebaseEventServerPushData serverPushData = new GamebaseResponse.Event.GamebaseEventServerPushData();
            serverPushData.extras = message.data;

            GamebaseResponse.Event.GamebaseEventMessage eventMessage = new GamebaseResponse.Event.GamebaseEventMessage();
            eventMessage.category = string.Format("serverPush{0}", GamebaseStringUtil.Capitalize(message.type));
            eventMessage.data = JsonMapper.ToJson(serverPushData);

            GamebaseEventHandlerManager.Instance.OnEventHandler(eventMessage);
        }
    }   
}
#endif