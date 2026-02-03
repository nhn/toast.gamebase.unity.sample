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
    public sealed class GamebaseGameNoticeImplementation
    {
        private static readonly GamebaseGameNoticeImplementation instance = new GamebaseGameNoticeImplementation();

        public static GamebaseGameNoticeImplementation Instance
        {
            get { return instance; }
        }

        private IGamebaseGameNotice gameNotice;

        private GamebaseGameNoticeImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            gameNotice = new AndroidGamebaseGameNotice();
#elif !UNITY_EDITOR && UNITY_IOS
            gameNotice = new IOSGamebaseGameNotice();
#elif !UNITY_EDITOR && UNITY_WEBGL
            gameNotice = new WebGLGamebaseGameNotice();
#else
            gameNotice = new StandaloneGamebaseGameNotice();
#endif
        }
        public void OpenGameNotice(GamebaseRequest.GameNotice.Configuration configuration, GamebaseCallback.ErrorDelegate callback)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            
            if (configuration == null)
            {
                configuration = new GamebaseRequest.GameNotice.Configuration();
            }
            gameNotice.OpenGameNotice(configuration, handle);
        }
        
        public void RequestGameNoticeInfo(GamebaseRequest.GameNotice.Configuration configuration, GamebaseCallback.GamebaseDelegate<GameNoticeResponse.GameNoticeInfo> callback)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            if(configuration == null)
            {
                configuration = new GamebaseRequest.GameNotice.Configuration();
            }
            gameNotice.RequestGameNoticeInfo(configuration, handle);
        }

    }
}