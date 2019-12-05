#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseAnalytics : IGamebaseAnalytics
    {
        protected static class GamebaseAnalytics
        {
            public const string ANALYTICS_API_SET_GAME_USER_DATA             = "gamebase://setGameUserData";
            public const string ANALYTICS_API_TRACE_LEVEL_UP                 = "gamebase://traceLevelUp";
        }

        protected INativeMessageSender  messageSender   = null;
        protected string                CLASS_NAME      = string.Empty;
        

        public NativeGamebaseAnalytics()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);            
        }

        virtual public void SetGameUserData(GamebaseRequest.Analytics.GameUserData gameUserData)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAnalytics.ANALYTICS_API_SET_GAME_USER_DATA,
                    jsonData: JsonMapper.ToJson(gameUserData)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void TraceLevelUp(GamebaseRequest.Analytics.LevelUpData levelUpData)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAnalytics.ANALYTICS_API_TRACE_LEVEL_UP,
                    jsonData: JsonMapper.ToJson(levelUpData)
                    ));
            messageSender.GetAsync(jsonData);
        }
    }
}
#endif