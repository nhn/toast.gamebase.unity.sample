#if UNITY_EDITOR || UNITY_STANDALONE

using System.Runtime.InteropServices;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseNetwork : CommonGamebaseNetwork
    {
        public StandaloneGamebaseNetwork()
        {
            Domain = typeof(StandaloneGamebaseNetwork).Name;
        }

        public override bool IsConnected()
        {
            return GamebaseNativeUtils.Instance.IsNetworkConnected;
        }
    }
}
#endif