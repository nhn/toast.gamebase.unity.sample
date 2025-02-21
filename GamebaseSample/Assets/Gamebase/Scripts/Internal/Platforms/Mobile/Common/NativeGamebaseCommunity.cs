#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseCommunity : IGamebaseCommunity
    {
        protected static class GamebaseCommunity
        {
            public const string COMMUNITY_API_OPEN_WITH_CONFIGURATION = "gamebase://openCommunityWithConfiguration";
        }

        protected INativeMessageSender messageSender = null;
        protected string CLASS_NAME = string.Empty;


        public NativeGamebaseCommunity()
        {
            Init();
        }

        protected virtual void Init()
        {
            messageSender.Initialize(CLASS_NAME);
            DelegateManager.AddDelegate(GamebaseCommunity.COMMUNITY_API_OPEN_WITH_CONFIGURATION, DelegateManager.SendErrorDelegateOnce);
        }

        public void OpenCommunity(GamebaseRequest.Community.Configuration configuration, int handle)
        {
            string jsonData = JsonMapper.ToJson(
               new UnityMessage(
                   GamebaseCommunity.COMMUNITY_API_OPEN_WITH_CONFIGURATION,
                   handle: handle,
                   jsonData: JsonMapper.ToJson(configuration)
                   ));
            messageSender.GetAsync(jsonData);
        }
    }
}
#endif