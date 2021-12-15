#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseContact : IGamebaseContact
    {
        protected static class GamebaseContact
        {
            public const string CONTACT_API_OPEN_CONTACT                            = "gamebase://openContact";
            public const string CONTACT_API_OPEN_CONTACT_WITH_CONFIGURATION         = "gamebase://openContactWithConfiguration";
            public const string CONTACT_API_REQUEST_CONTACT_URL                     = "gamebase://requestContactURL";
            public const string CONTACT_API_REQUEST_CONTACT_URL_WITH_CONFIGURATION  = "gamebase://requestContactURLWithConfiguration";
        }

        protected INativeMessageSender  messageSender   = null;
        protected string                CLASS_NAME      = string.Empty;
        

        public NativeGamebaseContact()
        {
            Init();
        }

        protected virtual void Init()
        {
            messageSender.Initialize(CLASS_NAME);
            DelegateManager.AddDelegate(GamebaseContact.CONTACT_API_OPEN_CONTACT,                               DelegateManager.SendErrorDelegateOnce);
            DelegateManager.AddDelegate(GamebaseContact.CONTACT_API_OPEN_CONTACT_WITH_CONFIGURATION,            DelegateManager.SendErrorDelegateOnce);
            DelegateManager.AddDelegate(GamebaseContact.CONTACT_API_REQUEST_CONTACT_URL,                        DelegateManager.SendGamebaseDelegateOnce<string>);
            DelegateManager.AddDelegate(GamebaseContact.CONTACT_API_REQUEST_CONTACT_URL_WITH_CONFIGURATION,     DelegateManager.SendGamebaseDelegateOnce<string>);
        }

        public void OpenContact(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseContact.CONTACT_API_OPEN_CONTACT,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void OpenContact(GamebaseRequest.Contact.Configuration configuration, int handle)
        {
            string jsonData = JsonMapper.ToJson(
               new UnityMessage(
                   GamebaseContact.CONTACT_API_OPEN_CONTACT_WITH_CONFIGURATION,
                   handle: handle,
                   jsonData: JsonMapper.ToJson(configuration)
                   ));
            messageSender.GetAsync(jsonData);
        }

        public void RequestContactURL(int handle)
        {
            string jsonData = JsonMapper.ToJson(
               new UnityMessage(
                   GamebaseContact.CONTACT_API_REQUEST_CONTACT_URL,
                   handle: handle
                   ));
            messageSender.GetAsync(jsonData);
        }

        public void RequestContactURL(GamebaseRequest.Contact.Configuration configuration, int handle)
        {
            string jsonData = JsonMapper.ToJson(
               new UnityMessage(
                   GamebaseContact.CONTACT_API_REQUEST_CONTACT_URL_WITH_CONFIGURATION,
                   handle: handle,
                   jsonData: JsonMapper.ToJson(configuration)
                   ));
            messageSender.GetAsync(jsonData);
        }
    }
}
#endif