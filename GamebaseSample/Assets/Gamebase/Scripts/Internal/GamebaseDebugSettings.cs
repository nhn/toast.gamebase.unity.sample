namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseDebugSettings
    {
        private class RemoteSetting
        {
            private const string REMOTE_SETTINGS_ON     = "ON";
            private const string REMOTE_SETTINGS_OFF    = "OFF";
            private const string REMOTE_SETTINGS_SDK    = "SDK";
            
            private string policy       = string.Empty;
            private string indicator    = string.Empty;            

            public void Initialize(string policy, string indicator)
            {                
                if (string.IsNullOrEmpty(policy) == false)
                {
                    this.policy = policy;
                }

                if (string.IsNullOrEmpty(indicator) == false)
                {
                    this.indicator = indicator;
                }
            }

            public bool IsDebugMode(bool isDebugMode)
            {
                if(string.IsNullOrEmpty(policy) == false)
                {
                    if (policy.Equals(REMOTE_SETTINGS_OFF) == true)
                    {
                        return false;
                    }
                    else if (policy.Equals(REMOTE_SETTINGS_ON) == true)
                    {
                        return true;
                    }
                }

                return isDebugMode;
            }

            public bool IsIndicatorMode()
            {
                if (string.IsNullOrEmpty(indicator) == false)
                {
                    if (indicator.Equals(REMOTE_SETTINGS_ON) == true)
                    {
                        return true;
                    }
                }
                else
                {
                    if (policy.Equals(REMOTE_SETTINGS_ON) == true)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static readonly GamebaseDebugSettings instance = new GamebaseDebugSettings();

        public static GamebaseDebugSettings Instance
        {
            get { return instance; }
        }

        private bool isDebugMode = false;
        
        private GamebaseDebugSettings()
        {
            Gamebase.AddObserver((data) => 
            {
                if(data.type.Equals(GamebaseObserverType.LAUNCHING) == true)
                {
                    var launchingInfo = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
                    SetRemoteSettings(launchingInfo);
                }
            });
        }

        public bool IsDebugMode
        {
            get
            {
                return isDebugMode;
            }
            private set
            {
                isDebugMode = value;
            }
        }
                
        private RemoteSetting logSetting                = new RemoteSetting();

        public void SetDebugMode(bool isDebugMode)
        {
            IsDebugMode = isDebugMode;
            GamebaseLog.SetDebugLog(logSetting.IsDebugMode(isDebugMode));
        }

        public void SetRemoteSettings(LaunchingResponse.LaunchingInfo launchingInfo)
        {
            if(launchingInfo == null 
                || launchingInfo.launching == null
                || launchingInfo.launching.tcgbClient == null)
            {
                return;
            }

            var forceRemoteSettings = launchingInfo.launching.tcgbClient.forceRemoteSettings;
            if (forceRemoteSettings != null)
            {
                GamebaseLog.Debug(GamebaseJsonUtil.ToPrettyJsonString(forceRemoteSettings), this);

                logSetting.Initialize(forceRemoteSettings.log.policy, forceRemoteSettings.log.indicator);
                
                GamebaseLog.SetDebugLog(logSetting.IsDebugMode(isDebugMode));

                GamebaseInternalReport.Instance.Initialize(
                    logSetting.IsIndicatorMode(), 
                    forceRemoteSettings.log.appKeyIndicator, 
                    forceRemoteSettings.log.appKeyLog);
            }
            else
            {
                GamebaseLog.Debug("ForceRemoteSettings is null", this);
            }
        }
    }
}
