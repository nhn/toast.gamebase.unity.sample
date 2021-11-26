namespace Toast.Gamebase.Internal
{
    internal interface IGamebase
    {
        void SetDebugMode(bool isDebugMode);
        void Initialize(GamebaseRequest.GamebaseConfiguration configuration, int handle);

        string GetSDKVersion();
        string GetUserID();
        string GetAccessToken();
        string GetLastLoggedInProvider();        
        string GetDeviceLanguageCode();
        string GetCarrierCode();
        string GetCarrierName();
        string GetCountryCode();
        string GetCountryCodeOfUSIM();
        string GetCountryCodeOfDevice();
        bool IsSandbox();
        void SetDisplayLanguageCode(string languageCode);
        string GetDisplayLanguageCode();

        void AddObserver(int handle);
        void RemoveObserver();
        void RemoveAllObserver();
        void AddServerPushEvent(int handle);
        void RemoveServerPushEvent();
        void RemoveAllServerPushEvent();

        void AddEventHandler(int handle);
        void RemoveEventHandler();
        void RemoveAllEventHandler();
    }
}
