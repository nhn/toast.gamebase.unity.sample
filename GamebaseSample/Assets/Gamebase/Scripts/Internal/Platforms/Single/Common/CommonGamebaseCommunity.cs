#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseCommunity : IGamebaseCommunity
    {
        private string domain = string.Empty;
        protected bool isAuthenticationAlreadyProgress = false;
        
        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return typeof(CommonGamebaseCommunity).Name;
                }

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public virtual void OpenCommunity(GamebaseRequest.Community.Configuration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
            if (callback == null)
            {
                return;
            }

            GamebaseCommunity.Instance.OpenCommunity(configuration, handle);
        }
    }
}
#endif