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
	public class GamebaseTermsImplementation
	{
        private static readonly GamebaseTermsImplementation instance = new GamebaseTermsImplementation();

        public static GamebaseTermsImplementation Instance
        {
            get { return instance; }
        }

        private IGamebaseTerms terms;

        private GamebaseTermsImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            terms = new AndroidGamebaseTerms();
#elif !UNITY_EDITOR && UNITY_IOS
            terms = new IOSGamebaseTerms();
#elif !UNITY_EDITOR && UNITY_WEBGL
            terms = new WebGLGamebaseTerms();
#else
            terms = new StandaloneGamebaseTerms();
#endif
        }

        public void ShowTermsView(GamebaseCallback.GamebaseDelegate<GamebaseResponse.DataContainer> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            terms.ShowTermsView(handle);
        }

        public void UpdateTerms(GamebaseRequest.Terms.UpdateTermsConfiguration configuration, GamebaseCallback.ErrorDelegate callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            terms.UpdateTerms(configuration, handle);
        }

        public void QueryTerms(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Terms.QueryTermsResult> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            terms.QueryTerms(handle);
        }
    }
}
