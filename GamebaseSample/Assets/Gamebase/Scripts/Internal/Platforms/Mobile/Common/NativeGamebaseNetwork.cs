#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using System;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{

    public class NativeGamebaseNetwork : IGamebaseNetwork
    {
        protected class GamebaseNetwork
        {            
            public const string NETWORK_API_GET_TYPE                             = "gamebase://getType";
            public const string NETWORK_API_GET_TYPE_NAME                        = "gamebase://getTypeName";
            public const string NETWORK_API_IS_CONNECTED                         = "gamebase://isConnected";
        }

        protected INativeMessageSender  messageSender   = null;
        protected string                CLASS_NAME      = string.Empty;

        public NativeGamebaseNetwork()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);
        }

        virtual public GamebaseNetworkType GetNetworkType()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseNetwork.NETWORK_API_GET_TYPE));
            string jsonString     = messageSender.GetSync(jsonData);

            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return GamebaseNetworkType.TYPE_NOT;
            }
            else
            {
                return (GamebaseNetworkType)Convert.ToInt32(jsonString);
            }
        }

        virtual public string GetNetworkTypeName()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseNetwork.NETWORK_API_GET_TYPE_NAME));

            return messageSender.GetSync(jsonData);
        }

        virtual public bool IsConnected()
        {
            string jsonData     = JsonMapper.ToJson(new UnityMessage(GamebaseNetwork.NETWORK_API_IS_CONNECTED));
            string jsonString   = messageSender.GetSync(jsonData);

            if (true == string.IsNullOrEmpty(jsonString))
            {
                return false;
            }

            JsonData requestData = JsonMapper.ToObject(jsonString);

            if (null != requestData["isConnected"])
            {
                return (bool)requestData["isConnected"];
            }
            else
            {
                return false;
            }
        }

        virtual public void IsConnected(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.DataDelegate<bool>>(handle));
        }        


        virtual protected void OnAddOnChangedStatusListener(NativeMessage message)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.DataDelegate<GamebaseNetworkType>>(message.handle);
            if (null == callback)
            {
                return;
            }

            callback((GamebaseNetworkType)Convert.ToInt32(message.jsonData));
        }
    }
}
#endif