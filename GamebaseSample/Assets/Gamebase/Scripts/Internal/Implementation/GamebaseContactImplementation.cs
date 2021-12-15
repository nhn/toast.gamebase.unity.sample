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
    public sealed class GamebaseContactImplementation
    {
        private static readonly GamebaseContactImplementation instance = new GamebaseContactImplementation();

        public static GamebaseContactImplementation Instance
        {
            get { return instance; }
        }

        private IGamebaseContact contact;

        private GamebaseContactImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            contact = new AndroidGamebaseContact();
#elif !UNITY_EDITOR && UNITY_IOS
            contact = new IOSGamebaseContact();
#elif !UNITY_EDITOR && UNITY_WEBGL
            contact = new WebGLGamebaseContact();
#else
            contact = new StandaloneGamebaseContact();
#endif
        }

        public void OpenContact(GamebaseCallback.ErrorDelegate callback)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            contact.OpenContact(handle);
        }

        public void OpenContact(GamebaseRequest.Contact.Configuration configuration, GamebaseCallback.ErrorDelegate callback)
        {
            GamebaseGameInformationReport.Instance.AddApiName("OpenContactWithConfiguration");
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            contact.OpenContact(configuration, handle);
        }

        public void RequestContactURL(GamebaseCallback.GamebaseDelegate<string> callback)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            contact.RequestContactURL(handle);
        }

        public void RequestContactURL(GamebaseRequest.Contact.Configuration configuration, GamebaseCallback.GamebaseDelegate<string> callback)
        {
            GamebaseGameInformationReport.Instance.AddApiName("RequestContactURLWithConfiguration");
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            contact.RequestContactURL(configuration, handle);
        }
    }
}