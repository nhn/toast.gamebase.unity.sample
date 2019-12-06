#if UNITY_EDITOR || UNITY_STANDALONE

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebase : CommonGamebase
    {
        public StandaloneGamebase()
        {
            Domain = typeof(StandaloneGamebase).Name;
        }
    }
}
#endif