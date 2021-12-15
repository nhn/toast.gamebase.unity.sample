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
    public sealed class GamebaseNetworkImplementation
    {        
        private int networkStateHandle;

        private static readonly GamebaseNetworkImplementation instance = new GamebaseNetworkImplementation();
        
        public static GamebaseNetworkImplementation Instance
        {
            get { return instance; }
        }

        IGamebaseNetwork network;

        private GamebaseNetworkImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            network = new AndroidGamebaseNetwork();
#elif !UNITY_EDITOR && UNITY_IOS
            network = new IOSGamebaseNetwork();
#elif !UNITY_EDITOR && UNITY_WEBGL
            network = new WebGLGamebaseNetwork();
#else
            network = new StandaloneGamebaseNetwork();
#endif
        }

        public GamebaseNetworkType GetNetworkType()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return network.GetNetworkType();
        }

        public string GetNetworkTypeName()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return network.GetNetworkTypeName();
        }

        public bool IsConnected()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return network.IsConnected();
        }
        
        public void IsConnected(GamebaseCallback.DataDelegate<bool> callback)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            network.IsConnected(handle);
        }
    }
}