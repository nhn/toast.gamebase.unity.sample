namespace Toast.Gamebase.Internal
{
    public static class GamebaseNativeUtils
    {
        private static IGamebaseNativeUtils instance;

        public static IGamebaseNativeUtils Instance
        {
            get
            {
                if (instance == null)
                {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                    instance = new GamebaseWindowsUtils();
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                    instance = new GamebaseMacUtils();
#else
                    instance = new GamebaseDefaultUtils();
#endif
                }
                
                return instance;
            }
        }
    }
}