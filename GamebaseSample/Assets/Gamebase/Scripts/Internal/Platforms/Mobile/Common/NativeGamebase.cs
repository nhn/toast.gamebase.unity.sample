#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS

using System;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebase : IGamebase
    {
        protected class Gamebase
        {
            public const string GAMEBASE_API_INITIALIZE                      = "gamebase://initialize";
            public const string GAMEBASE_API_SET_DEBUG_MODE                  = "gamebase://setDebugMode";
            public const string GAMEBASE_API_GET_SDK_VERSION                 = "gamebase://getSDKVersion";
            public const string GAMEBASE_API_GET_USERID                      = "gamebase://getUserID";
            public const string GAMEBASE_API_GET_ACCESSTOKEN                 = "gamebase://getAccessToken";
            public const string GAMEBASE_API_GET_LAST_LOGGED_IN_PROVIDER     = "gamebase://getLastLoggedInProvider";
            public const string GAMEBASE_API_GET_DEVICE_LANGUAGE_CODE        = "gamebase://getDeviceLanguageCode";
            public const string GAMEBASE_API_GET_CARRIER_CODE                = "gamebase://getCarrierCode";
            public const string GAMEBASE_API_GET_CARRIER_NAME                = "gamebase://getCarrierName";
            public const string GAMEBASE_API_GET_COUNTRY_CODE                = "gamebase://getCountryCode";
            public const string GAMEBASE_API_GET_COUNTRY_CODE_OF_USIM        = "gamebase://getCountryCodeOfUSIM";
            public const string GAMEBASE_API_GET_COUNTRY_CODE_OF_DEVICE      = "gamebase://getCountryCodeOfDevice";
            public const string GAMEBASE_API_IS_SANDBOX                      = "gamebase://isSandbox";
            public const string GAMEBASE_API_SET_DISPLAY_LANGUAGE_CODE       = "gamebase://setDisplayLanguageCode";
            public const string GAMEBASE_API_GET_DISPLAY_LANGUAGE_CODE       = "gamebase://getDisplayLanguageCode";
            public const string GAMEBASE_API_ADD_SERVER_PUSH_EVENT           = "gamebase://addServerPushEvent";
            public const string GAMEBASE_API_REMOVE_SERVER_PUSH_EVENT        = "gamebase://removeServerPushEvent";
            public const string GAMEBASE_API_ADD_OBSERVER                    = "gamebase://addObserver";
            public const string GAMEBASE_API_REMOVE_OBSERVER                 = "gamebase://removeObserver";
        }

        protected INativeMessageSender  messageSender   = null;
        protected string                CLASS_NAME      = string.Empty;
 

        public NativeGamebase()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);

            DelegateManager.AddDelegate(Gamebase.GAMEBASE_API_INITIALIZE,               DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Launching.LaunchingInfo>);
            DelegateManager.AddDelegate(Gamebase.GAMEBASE_API_ADD_SERVER_PUSH_EVENT,    DelegateManager.SendDataDelegate<GamebaseResponse.SDK.ServerPushMessage>);
            DelegateManager.AddDelegate(Gamebase.GAMEBASE_API_ADD_OBSERVER,             DelegateManager.SendDataDelegate<GamebaseResponse.SDK.ObserverMessage>);
        }

        virtual public void SetDebugMode(bool isDebugMode)
        {
            var vo = new NativeRequest.SDK.IsDebugMode();
            vo.isDebugMode = isDebugMode;
            GamebaseDebugSettings.Instance.SetDebugMode(isDebugMode);
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    Gamebase.GAMEBASE_API_SET_DEBUG_MODE,
                    jsonData: JsonMapper.ToJson(vo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void Initialize(GamebaseRequest.GamebaseConfiguration configuration, int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    Gamebase.GAMEBASE_API_INITIALIZE,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(configuration)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public string GetSDKVersion()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_SDK_VERSION));
            return messageSender.GetSync(jsonData);
        }

        virtual public string GetUserID()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_USERID));
            return messageSender.GetSync(jsonData);
        }

        virtual public string GetAccessToken()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_ACCESSTOKEN));
            return messageSender.GetSync(jsonData);
        }

        virtual public string GetLastLoggedInProvider()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_LAST_LOGGED_IN_PROVIDER));
            return messageSender.GetSync(jsonData);
        }
        
        virtual public string GetDeviceLanguageCode()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_DEVICE_LANGUAGE_CODE));
            return messageSender.GetSync(jsonData);
        }

        virtual public string GetCarrierCode()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_CARRIER_CODE));
            return messageSender.GetSync(jsonData);
        }

        virtual public string GetCarrierName()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_CARRIER_NAME));
            return messageSender.GetSync(jsonData);
        }

        virtual public string GetCountryCode()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_COUNTRY_CODE));
            return messageSender.GetSync(jsonData);
        }

        virtual public string GetCountryCodeOfUSIM()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_COUNTRY_CODE_OF_USIM));
            return messageSender.GetSync(jsonData);
        }

        virtual public string GetCountryCodeOfDevice()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_COUNTRY_CODE_OF_DEVICE));
            return messageSender.GetSync(jsonData);
        }

        virtual public bool IsSandbox()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_IS_SANDBOX));
            string jsonString = messageSender.GetSync(jsonData);

            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return false;
            }

            JsonData requestData = JsonMapper.ToObject(jsonString);

            if (null != requestData["isSandbox"])
            {
                return (bool)requestData["isSandbox"];
            }

            return false;
        }

        virtual public void SetDisplayLanguageCode(string languageCode)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    Gamebase.GAMEBASE_API_SET_DISPLAY_LANGUAGE_CODE,
                    jsonData: languageCode
                    ));
            messageSender.GetSync(jsonData);
        }

        virtual public string GetDisplayLanguageCode()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_GET_DISPLAY_LANGUAGE_CODE));
            return messageSender.GetSync(jsonData);
        }

        virtual public void AddObserver(int handle)
        {
            if (1 == GamebaseObserverManager.Instance.GetCount())
            {
                string jsonData = JsonMapper.ToJson(
                    new UnityMessage(
                        Gamebase.GAMEBASE_API_ADD_OBSERVER,
                        handle: handle
                        ));
                messageSender.GetSync(jsonData);
            }
        }

        virtual public void RemoveObserver()
        {
            if (0 == GamebaseObserverManager.Instance.GetCount())
            {
                string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_REMOVE_OBSERVER));
                messageSender.GetSync(jsonData);
            }
        }

        virtual public void RemoveAllObserver()
        {
            RemoveObserver();
        }

        virtual public void AddServerPushEvent(int handle)
        {
            if (1 == GamebaseServerPushEventManager.Instance.GetCount())
            {
                string jsonData = JsonMapper.ToJson(
                    new UnityMessage(
                        Gamebase.GAMEBASE_API_ADD_SERVER_PUSH_EVENT,
                        handle: handle
                        ));
                messageSender.GetSync(jsonData);
            }
        }

        virtual public void RemoveServerPushEvent()
        {
            if (0 == GamebaseServerPushEventManager.Instance.GetCount())
            {
                string jsonData = JsonMapper.ToJson(new UnityMessage(Gamebase.GAMEBASE_API_REMOVE_SERVER_PUSH_EVENT));
                messageSender.GetSync(jsonData);
            }
        }

        virtual public void RemoveAllServerPushEvent()
        {
            RemoveServerPushEvent();
        }
    }
}

#endif