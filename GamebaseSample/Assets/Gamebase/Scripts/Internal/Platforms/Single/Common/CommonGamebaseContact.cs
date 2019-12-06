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

        public void OpenContact(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle));
        }
    }
}
#endif