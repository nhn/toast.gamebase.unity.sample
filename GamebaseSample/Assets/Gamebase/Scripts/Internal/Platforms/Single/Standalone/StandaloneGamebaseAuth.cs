#if UNITY_EDITOR || UNITY_STANDALONE
using Toast.Gamebase.Internal.Auth;
using Toast.Gamebase.Internal.Auth.Browser;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseAuth : CommonGamebaseAuth
    {
        public StandaloneGamebaseAuth()
        {
            Domain = typeof(StandaloneGamebaseAuth).Name;

            _browserLoginService = new BrowserLoginService(
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                new WindowsBrowser()
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                new MacOSBrowser()
#endif
            );
        }

        
    }
}
#endif