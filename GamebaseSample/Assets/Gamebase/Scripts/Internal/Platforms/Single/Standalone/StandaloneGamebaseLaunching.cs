#if UNITY_EDITOR || UNITY_STANDALONE

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseLaunching : CommonGamebaseLaunching
    {
        public StandaloneGamebaseLaunching()
        {
            Domain = typeof(StandaloneGamebaseLaunching).Name;
        }
    }
}
#endif