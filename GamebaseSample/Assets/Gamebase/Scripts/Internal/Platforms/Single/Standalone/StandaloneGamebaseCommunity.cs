#if UNITY_EDITOR || UNITY_STANDALONE
namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseCommunity : CommonGamebaseCommunity
    {
        public StandaloneGamebaseCommunity()
        {
            Domain = typeof(StandaloneGamebaseCommunity).Name;
        }
    }
}
#endif