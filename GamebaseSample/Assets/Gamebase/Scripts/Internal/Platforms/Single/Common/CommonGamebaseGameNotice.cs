#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseGameNotice : IGamebaseGameNotice
    {
        private string domain = string.Empty;
        
        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return typeof(CommonGamebaseGameNotice).Name;
                }

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public virtual void OpenGameNotice(GamebaseRequest.GameNotice.Configuration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
            GamebaseCallbackHandler.UnregisterCallback(handle);

            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
        }
        
        public virtual void RequestGameNoticeInfo(GamebaseRequest.GameNotice.Configuration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GameNoticeResponse.GameNoticeInfo>>(handle);
            GamebaseCallbackHandler.UnregisterCallback(handle);

            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
        }
    }
}
#endif