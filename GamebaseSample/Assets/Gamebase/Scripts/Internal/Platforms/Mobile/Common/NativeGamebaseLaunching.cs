#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using System;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseLaunching : IGamebaseLaunching
    {
        protected class GamebaseLaunching
        {
            public const string LAUNCHING_API_GET_LAUNCHING_INFORMATIONS     = "gamebase://getLaunchingInformations";
            public const string LAUNCHING_API_GET_LAUNCHING_STATUS           = "gamebase://getLaunchingStatus";
        }

        protected INativeMessageSender  messageSender   = null;
        protected string                CLASS_NAME      = string.Empty;

        public NativeGamebaseLaunching()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);
        }

        virtual public LaunchingResponse.LaunchingInfo GetLaunchingInformations()
        {
            string jsonData     = JsonMapper.ToJson(new UnityMessage(GamebaseLaunching.LAUNCHING_API_GET_LAUNCHING_INFORMATIONS));
            string jsonString   = messageSender.GetSync(jsonData);

            if(string.IsNullOrEmpty(jsonString) == true)
            {
                return null;
            }

            return JsonMapper.ToObject<LaunchingResponse.LaunchingInfo>(jsonString);
        }

        virtual public int GetLaunchingStatus()
        {
            string jsonData     = JsonMapper.ToJson(new UnityMessage(GamebaseLaunching.LAUNCHING_API_GET_LAUNCHING_STATUS));
            string jsonString = messageSender.GetSync(jsonData);

            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return 0;
            }

            return Convert.ToInt32(jsonString);
        }
    }
}
#endif