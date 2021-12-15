#if !UNITY_EDITOR && UNITY_ANDROID
using Toast.Gamebase.Internal.Mobile.Android;
#elif !UNITY_EDITOR && UNITY_IOS
using Toast.Gamebase.Internal.Mobile.IOS;
#elif !UNITY_EDITOR && UNITY_WEBGL
using Toast.Gamebase.Internal.Single.WebGL;
#else
using Toast.Gamebase.Internal.Single.Standalone;
#endif

using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseLaunchingImplementation
    {        
        private int launchingStatusHandle;

        private static readonly GamebaseLaunchingImplementation instance = new GamebaseLaunchingImplementation();
        
        public static GamebaseLaunchingImplementation Instance
        {
            get { return instance; }
        }

        IGamebaseLaunching launching;

        private GamebaseLaunchingImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            launching = new AndroidGamebaseLaunching();
#elif !UNITY_EDITOR && UNITY_IOS
            launching = new IOSGamebaseLaunching();
#elif !UNITY_EDITOR && UNITY_WEBGL
            launching = new WebGLGamebaseLaunching();
#else
            launching = new StandaloneGamebaseLaunching();
#endif
        }
        
        public GamebaseResponse.Launching.LaunchingInfo GetLaunchingInformations()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            var launchingInfo = launching.GetLaunchingInformations();
            return JsonMapper.ToObject<GamebaseResponse.Launching.LaunchingInfo>(JsonMapper.ToJson(launchingInfo));            
        }

        public int GetLaunchingStatus()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return launching.GetLaunchingStatus();
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        public void RequestLaunchingInfo(int handle)
        {
            ((Single.CommonGamebaseLaunching)launching).GetLaunchingInfo(handle);
        }

        public void RequestLaunchingStatus(int handle)
        {
            ((Single.CommonGamebaseLaunching)launching).RequestLaunchingStatus(handle);
        }

        public float GetStatusElaspedTime()
        {
            return ((Single.CommonGamebaseLaunching)launching).GetStatusElaspedTime();
        }
#endif
    }
}