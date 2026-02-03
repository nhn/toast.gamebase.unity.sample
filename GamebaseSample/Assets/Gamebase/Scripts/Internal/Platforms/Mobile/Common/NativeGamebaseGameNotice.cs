#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseGameNotice : IGamebaseGameNotice
    {
        protected static class GamebaseCommunity
        {
            public const string GAME_NOTICE_API_OPEN_GAME_NOTICE = "gamebase://openGameNotice";
            public const string GAME_NOTICE_API_REQUEST_GAME_NOTICE_INFO = "gamebase://requestGameNoticeInfo";
        }

        protected INativeMessageSender messageSender = null;
        protected string CLASS_NAME = string.Empty;

        public NativeGamebaseGameNotice()
        {
            Init();
        }

        protected virtual void Init()
        {
            messageSender.Initialize(CLASS_NAME);
            DelegateManager.AddDelegate(GamebaseCommunity.GAME_NOTICE_API_OPEN_GAME_NOTICE, DelegateManager.SendErrorDelegateOnce);
            DelegateManager.AddDelegate(GamebaseCommunity.GAME_NOTICE_API_REQUEST_GAME_NOTICE_INFO, DelegateManager.SendGamebaseDelegateOnce<GameNoticeResponse.GameNoticeInfo>);
        }

        public void OpenGameNotice(GamebaseRequest.GameNotice.Configuration configuration, int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseCommunity.GAME_NOTICE_API_OPEN_GAME_NOTICE,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(configuration)
                ));
            messageSender.GetAsync(jsonData);
        }

        public void RequestGameNoticeInfo(GamebaseRequest.GameNotice.Configuration configuration, int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseCommunity.GAME_NOTICE_API_REQUEST_GAME_NOTICE_INFO,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(configuration)
                ));
            messageSender.GetAsync(jsonData);
        }
    }
}
#endif