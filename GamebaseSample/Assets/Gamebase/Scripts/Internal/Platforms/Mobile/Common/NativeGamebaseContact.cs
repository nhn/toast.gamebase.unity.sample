#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseContact : IGamebaseContact
    {
        protected static class GamebaseContact
        {
            public const string CONTACT_API_OPEN_CONTACT             = "gamebase://openContact";
        }

        protected INativeMessageSender  messageSender   = null;
        protected string                CLASS_NAME      = string.Empty;
        

        public NativeGamebaseContact()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);
            DelegateManager.AddDelegate(GamebaseContact.CONTACT_API_OPEN_CONTACT, DelegateManager.SendErrorDelegateOnce);
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
    }
}
#endif