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
    public sealed class GamebaseCommunityImplementation
    {
        private static readonly GamebaseCommunityImplementation instance = new GamebaseCommunityImplementation();

        public static GamebaseCommunityImplementation Instance
        {
            get { return instance; }
        }

        private IGamebaseCommunity community;

        private GamebaseCommunityImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            community = new AndroidGamebaseCommunity();
#elif !UNITY_EDITOR && UNITY_IOS
            community = new IOSGamebaseCommunity();
#elif !UNITY_EDITOR && UNITY_WEBGL
            community = new WebGLGamebaseCommunity();
#else
            community = new StandaloneGamebaseCommunity();
#endif
        }
        public void OpenCommunity(GamebaseRequest.Community.Configuration configuration, GamebaseCallback.ErrorDelegate callback)
        {
            GamebaseGameInformationReport.Instance.AddApiName("OpenCommunityWithConfiguration");
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            community.OpenCommunity(configuration, handle);
        }

    }
}