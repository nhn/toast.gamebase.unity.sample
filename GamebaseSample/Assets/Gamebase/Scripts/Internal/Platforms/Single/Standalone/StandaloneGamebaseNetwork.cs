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

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int flags, int reserved);

        public override bool IsConnected()
        {
            int flags;
            return InternetGetConnectedState(out flags, 0);
        }
#endif
    }
}
#endif