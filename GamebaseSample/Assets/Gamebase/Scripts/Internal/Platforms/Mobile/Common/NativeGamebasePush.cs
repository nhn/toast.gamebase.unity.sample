#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebasePush : IGamebasePush
    {
        protected class GamebasePush
        {
            public const string PUSH_API_REGISTER_PUSH              = "gamebase://registerPush";
            public const string PUSH_API_QUERY_PUSH                 = "gamebase://queryPush";
            public const string PUSH_API_SET_SANDBOX_MODE           = "gamebase://setSandboxMode";
            public const string PUSH_API_REGISTER_PUSH_WITH_OPTION  = "gamebase://registerPushWithOption";
            public const string PUSH_API_QUERY_TOKEN_INFO           = "gamebase://queryTokenInfo";
            public const string PUSH_API_GET_NOTIFICATION_OPTIONS   = "gamebase://getNotificationOptions";
            public const string PUSH_API_QUERY_NOTIFICATION_ALLOWED = "gamebase://queryNotificationAllowed";
        }

        protected INativeMessageSender  messageSender       = null;
        protected string                CLASS_NAME          = string.Empty;

        public NativeGamebasePush()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);

            DelegateManager.AddDelegate(GamebasePush.PUSH_API_REGISTER_PUSH,                DelegateManager.SendErrorDelegateOnce);
            DelegateManager.AddDelegate(GamebasePush.PUSH_API_QUERY_PUSH,                   DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Push.PushConfiguration>);
            DelegateManager.AddDelegate(GamebasePush.PUSH_API_REGISTER_PUSH_WITH_OPTION,    DelegateManager.SendErrorDelegateOnce);
            DelegateManager.AddDelegate(GamebasePush.PUSH_API_QUERY_TOKEN_INFO,             DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Push.TokenInfo>);
            DelegateManager.AddDelegate(GamebasePush.PUSH_API_QUERY_NOTIFICATION_ALLOWED,   DelegateManager.SendGamebaseDelegateOnce<bool>);
        }

        public void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, int handle)
        {
            string jsonString = null;
            if (null != pushConfiguration)
            {
                jsonString = JsonMapper.ToJson(pushConfiguration);
            }

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePush.PUSH_API_REGISTER_PUSH,
                    handle: handle,
                    jsonData: jsonString
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void QueryPush(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePush.PUSH_API_QUERY_PUSH,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void SetSandboxMode(bool isSandbox)
        {
        }

        public void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, GamebaseRequest.Push.NotificationOptions options, int handle)
        {
            string jsonString = null;
            if (null != pushConfiguration)
            {
                jsonString = JsonMapper.ToJson(pushConfiguration);
            }

            string jsonStringExtra = null;
            if (null != options)
            {
                jsonStringExtra = JsonMapper.ToJson(options);
            }

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePush.PUSH_API_REGISTER_PUSH_WITH_OPTION,
                    handle: handle,
                    jsonData: jsonString,
                    extraData: jsonStringExtra
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void QueryTokenInfo(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePush.PUSH_API_QUERY_TOKEN_INFO,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public GamebaseResponse.Push.NotificationOptions GetNotificationOptions()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebasePush.PUSH_API_GET_NOTIFICATION_OPTIONS));
            string jsonString = messageSender.GetSync(jsonData);

            GamebaseResponse.Push.NotificationOptions notificationOptions = null;

            if (jsonString != null)
            {
                notificationOptions = JsonMapper.ToObject<GamebaseResponse.Push.NotificationOptions>(jsonString);
            }

            return notificationOptions;
        }
        
        public void QueryNotificationAllowed(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebasePush.PUSH_API_QUERY_NOTIFICATION_ALLOWED,
                    handle: handle
                ));
            messageSender.GetAsync(jsonData);
        }
    }
}
#endif