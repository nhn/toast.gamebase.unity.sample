#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebasePush : IGamebasePush
    {
        protected class GamebasePush
        {
            public const string PUSH_API_REGISTER_PUSH      = "gamebase://registerPush";
            public const string PUSH_API_QUERY_PUSH         = "gamebase://queryPush";
            public const string PUSH_API_SET_SANDBOX_MODE   = "gamebase://setSandboxMode";
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

            DelegateManager.AddDelegate(GamebasePush.PUSH_API_REGISTER_PUSH,    DelegateManager.SendErrorDelegateOnce);
            DelegateManager.AddDelegate(GamebasePush.PUSH_API_QUERY_PUSH,       DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Push.PushConfiguration>);
        }

        virtual public void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, int handle)
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

        virtual public void QueryPush(int handle)
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
    }
}
#endif