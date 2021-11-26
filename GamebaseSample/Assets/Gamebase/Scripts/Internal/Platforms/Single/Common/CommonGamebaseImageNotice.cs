#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using Toast.Gamebase.Internal.Single;

namespace Toast.Gamebase.Internal
{
    public class CommonGamebaseImageNotice : IGamebaseImageNotice
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain))
                    return typeof(CommonGamebaseImageNotice).Name;

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public virtual void ShowImageNotices(GamebaseRequest.ImageNotice.Configuration configuration, int closeHandle, int eventHandle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(closeHandle);
            GamebaseCallbackHandler.UnregisterCallback(closeHandle);
            GamebaseCallbackHandler.UnregisterCallback(eventHandle);

            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
        }

        public virtual void CloseImageNotices()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
        }
    }
}
#endif
