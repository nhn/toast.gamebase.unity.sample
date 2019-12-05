#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseNetwork : IGamebaseNetwork
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain))
                    return typeof(CommonGamebaseNetwork).Name;

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public GamebaseNetworkType GetNetworkType()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return GamebaseNetworkType.TYPE_NOT;
        }

        public string GetNetworkTypeName()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return "";
        }
        
        public virtual bool IsConnected()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return true;
        }

        public virtual void IsConnected(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.DataDelegate<bool>>(handle));
        }
    }
}
#endif