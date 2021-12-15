#if !UNITY_EDITOR && UNITY_ANDROID
using Toast.Gamebase.Internal.Mobile.Android;
#elif !UNITY_EDITOR && UNITY_IOS
using Toast.Gamebase.Internal.Mobile.IOS;
#elif !UNITY_EDITOR && UNITY_WEBGL
using Toast.Gamebase.Internal.Single.WebGL;
#else
using Toast.Gamebase.Internal.Single.Standalone;
#endif

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseAnalyticsImplementation
    {
        private static readonly GamebaseAnalyticsImplementation instance = new GamebaseAnalyticsImplementation();
        
        public static GamebaseAnalyticsImplementation Instance
        {
            get { return instance; }
        }

        private IGamebaseAnalytics analytics;

        private GamebaseAnalyticsImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            analytics = new AndroidGamebaseAnalytics();
#elif !UNITY_EDITOR && UNITY_IOS
            analytics = new IOSGamebaseAnalytics();
#elif !UNITY_EDITOR && UNITY_WEBGL
            analytics = new WebGLGamebaseAnalytics();
#else
            analytics = new StandaloneGamebaseAnalytics();
#endif
        }

        public void SetGameUserData(GamebaseRequest.Analytics.GameUserData gameUserData)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            analytics.SetGameUserData(gameUserData);
        }

        public void TraceLevelUp(GamebaseRequest.Analytics.LevelUpData levelUpData)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            analytics.TraceLevelUp(levelUpData);
        }
    }
}