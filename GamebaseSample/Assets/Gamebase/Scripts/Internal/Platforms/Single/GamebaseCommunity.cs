#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using UnityEngine;

namespace Toast.Gamebase.Internal.Single
{
    public class GamebaseCommunity
    {
        private static readonly GamebaseCommunity instance = new GamebaseCommunity();

        public static GamebaseCommunity Instance
        {
            get { return instance; }
        }


        public void OpenCommunity(GamebaseRequest.Community.Configuration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
            if (callback == null)
            {
                return;
            }

            GamebaseCallbackHandler.UnregisterCallback(handle);

            GamebaseLog.Debug(string.Format("Community URL : {0}", configuration.forcedURL), this);
            Application.OpenURL(configuration.forcedURL);
        }
    }
}
#endif
