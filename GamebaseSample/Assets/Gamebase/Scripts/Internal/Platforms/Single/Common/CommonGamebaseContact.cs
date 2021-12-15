#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseContact : IGamebaseContact
    {
        private string domain = string.Empty;
        protected bool isAuthenticationAlreadyProgress = false;
        
        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return typeof(CommonGamebaseContact).Name;
                }

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public virtual void OpenContact(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
            if (callback == null)
            {
                return;
            }

            GamebaseCallbackHandler.UnregisterCallback(handle);

            GamebaseErrorNotifier.FireNotSupportedAPI(
                this,
                callback);
        }

        public virtual void OpenContact(GamebaseRequest.Contact.Configuration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
            if (callback == null)
            {
                return;
            }

            GamebaseCallbackHandler.UnregisterCallback(handle);

            GamebaseErrorNotifier.FireNotSupportedAPI(
                this,
                callback);
        }
        
        public void RequestContactURL(int handle)
        {
            GamebaseContact.Instance.RequestContactURL(handle);
        }

        public void RequestContactURL(GamebaseRequest.Contact.Configuration configuration, int handle)
        {
            GamebaseContact.Instance.RequestContactURL(configuration, handle);
        }
    }
}
#endif