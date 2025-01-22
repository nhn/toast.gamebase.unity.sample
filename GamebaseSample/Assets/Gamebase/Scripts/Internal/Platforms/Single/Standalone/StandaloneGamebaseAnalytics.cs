#if UNITY_EDITOR || UNITY_STANDALONE
namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseAnalytics : CommonGamebaseAnalytics
    {
        public StandaloneGamebaseAnalytics()
        {
            Domain = typeof(StandaloneGamebaseAnalytics).Name;
        }
    }
}
#endif