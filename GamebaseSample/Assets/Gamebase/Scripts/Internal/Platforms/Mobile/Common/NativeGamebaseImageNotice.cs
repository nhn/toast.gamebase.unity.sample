#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseImageNotice : IGamebaseImageNotice
    {

        protected class GamebaseImageNotice
        {
            public const string IMAGE_NOTICE_API_SHOW_IMAGE_NOTICES = "gamebase://showImageNotices";
            public const string IMAGE_NOTICE_API_CLOSE_IMAGE_NOTICES = "gamebase://closeImageNotices";
            public const string IMAGE_NOTICE_API_SCHEME_EVENT = "gamebase://schemeEventImageNotices";
        }

        protected class ExtraData
        {
            public int schemeEvent;

            public ExtraData(int schemeEvent)
            {
                this.schemeEvent = schemeEvent;
            }
        }

        protected INativeMessageSender messageSender = null;
        protected string CLASS_NAME = string.Empty;


        public NativeGamebaseImageNotice()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);

            DelegateManager.AddDelegate(GamebaseImageNotice.IMAGE_NOTICE_API_SHOW_IMAGE_NOTICES, DelegateManager.SendErrorDelegateOnce, OnCloseCallback);            
            DelegateManager.AddDelegate(GamebaseImageNotice.IMAGE_NOTICE_API_SCHEME_EVENT, DelegateManager.SendGamebaseDelegate<string>);
        }

        public void ShowImageNotices(GamebaseRequest.ImageNotice.Configuration configuration, int closeCallback, int eventCallback)
        {
            string extraData = JsonMapper.ToJson(new ExtraData(eventCallback));
            string configurationJson;

            if(configuration == null)
            {
                configurationJson = JsonMapper.ToJson(new GamebaseRequest.ImageNotice.Configuration());
            }
            else
            {
                configurationJson = JsonMapper.ToJson(configuration);
            }

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseImageNotice.IMAGE_NOTICE_API_SHOW_IMAGE_NOTICES,
                    handle: closeCallback,
                    jsonData: configurationJson,
                    extraData: extraData
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void CloseImageNotices()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseImageNotice.IMAGE_NOTICE_API_CLOSE_IMAGE_NOTICES));
            messageSender.GetAsync(jsonData);
        }

       protected void OnCloseCallback(NativeMessage message)
        {
            if (false == string.IsNullOrEmpty(message.extraData))
            {
                int schemeEventHandle = int.Parse(message.extraData);
                GamebaseCallbackHandler.UnregisterCallback(schemeEventHandle);
            }
        }
    }
}
#endif
