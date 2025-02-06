#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebasePush : IGamebasePush
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain))
                    return typeof(CommonGamebasePush).Name;

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle));
        }
        
        public void QueryPush(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Push.PushConfiguration>>(handle));
        }

        public void SetSandboxMode(bool isSandbox)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
        }

        public void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, GamebaseRequest.Push.NotificationOptions options, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle));
        }

        public void QueryTokenInfo(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Push.TokenInfo>>(handle));
        }

        public GamebaseResponse.Push.NotificationOptions GetNotificationOptions()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return null;
        }
        
        public void QueryNotificationAllowed(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<bool>>(handle));
        }
    }
}
#endif