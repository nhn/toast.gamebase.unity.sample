using System.Collections.Generic;
using Toast.Gamebase.Internal;

namespace Toast.Gamebase
{
    public class GamebaseRequest
    {
        public class GamebaseConfiguration
        {
            public string   appID;
            public string   appVersion;
            public string   zoneType;
            public string   displayLanguageCode;
            public bool     enablePopup;
            public bool     enableLaunchingStatusPopup;
            public bool     enableBanPopup;
            public bool     enableKickoutPopup;
            public string   storeCode;
            public string   fcmSenderId;
            public bool     useWebViewLogin;                // Unity Standalone only
        }        

        public class Auth
        {
            public class TransferAccountRenewConfiguration
            {
                public enum RenewalTargetType
                {
                    PASSWORD,
                    ID_PASSWORD,
                }

                private const string RENEWAL_TYPE           = "renewalModeType";
                private const string TARGET_ACCOUNT         = "renewalTargetType";
                private const string ACCOUNT_ID             = "accountId";
                private const string ACCOUNT_PASSWORD       = "accountPassword";
                
                private const string RENEWAL_MODE_MANUAL    = "MANUAL";
                private const string RENEWAL_MODE_AUTO      = "AUTO";
                
                private string renewalModeType = string.Empty;
                private RenewalTargetType renewalTargetType;
                private string accountId = string.Empty;
                private string accountPassword = string.Empty;                               

                public static TransferAccountRenewConfiguration MakeManualRenewConfiguration(string accountId, string accountPassword)
                {
                    TransferAccountRenewConfiguration renewTransferAccount = new TransferAccountRenewConfiguration();
                    renewTransferAccount.renewalModeType = RENEWAL_MODE_MANUAL;
                    renewTransferAccount.renewalTargetType = RenewalTargetType.ID_PASSWORD;
                    renewTransferAccount.accountId = accountId;
                    renewTransferAccount.accountPassword = accountPassword;

                    return renewTransferAccount;
                }

                public static TransferAccountRenewConfiguration MakeManualRenewConfiguration(string accountPassword)
                {
                    TransferAccountRenewConfiguration renewTransferAccount = new TransferAccountRenewConfiguration();
                    renewTransferAccount.renewalModeType = RENEWAL_MODE_MANUAL;
                    renewTransferAccount.renewalTargetType = RenewalTargetType.PASSWORD;
                    renewTransferAccount.accountPassword = accountPassword;

                    return renewTransferAccount;
                }

                public static TransferAccountRenewConfiguration MakeAutoRenewConfiguration(RenewalTargetType type)
                {
                    TransferAccountRenewConfiguration renewTransferAccount = new TransferAccountRenewConfiguration();
                    renewTransferAccount.renewalModeType = RENEWAL_MODE_AUTO;
                    renewTransferAccount.renewalTargetType = type;

                    return renewTransferAccount;
                }

                public string ToJsonString()
                {
                    LitJson.JsonData jsonData = new LitJson.JsonData();

                    jsonData[RENEWAL_TYPE] = renewalModeType;
                    jsonData[TARGET_ACCOUNT] = (int)renewalTargetType;

                    if (string.IsNullOrEmpty(accountId) == false)
                    {
                        jsonData[ACCOUNT_ID] = accountId;
                    }

                    if (string.IsNullOrEmpty(accountPassword) == false)
                    {
                        jsonData[ACCOUNT_PASSWORD] = accountPassword;
                    }

                    return LitJson.JsonMapper.ToJson(jsonData);
                }
            }
        }

        public class Push
        {
            public class PushConfiguration
            {
                public bool pushEnabled;
                public bool adAgreement;
                public bool adAgreementNight;
                public string displayLanguageCode = string.Empty;
            }
        }

        public class Webview
        {
            public class GamebaseWebViewConfiguration
            {
                public string title;
                public int orientation;
                public int colorR;
                public int colorG;
                public int colorB;
                public int colorA;
                public int barHeight;
                public bool isBackButtonVisible;
                public string backButtonImageResource;
                public string closeButtonImageResource;
            }

            public class SchemeConfiguration
            {
                public List<string> schemeList;
                public int schemeEvent;

                public SchemeConfiguration()
                {

                }

                public SchemeConfiguration(List<string> schemeList, int schemeEvent)
                {
                    this.schemeList = schemeList;
                    this.schemeEvent = schemeEvent;
                }
            }
        }

        public class Analytics
        {
            public class GameUserData
            {
                public int userLevel = 0;
                public string channelId = null;
                public string characterId = null;
                public string characterClassId = null;

                public GameUserData(int userLevel)
                {
                    this.userLevel = userLevel;
                }
            }

            public class LevelUpData
            {
                public int userLevel = 0;
                public long levelUpTime = -1;

                public LevelUpData(int userLevel, long levelUpTime)
                {
                    this.userLevel = userLevel;
                    this.levelUpTime = levelUpTime;

                    if(this.levelUpTime < 0)
                    {
                        GamebaseLog.Warn("levelUpTime parameter can not be negative.", this);
                    }
                }                
            }
        }

        public static class Purchase
        {
            public class Configuration
            {
                public string appKey;
                public string storeCode;
            }
        }

        public static class Logger
        {
            public class Configuration
            {
                public Configuration(string appKey)
                {
                    this.appKey = appKey;
                    serviceZone = "REAL";
                    enableCrashReporter = true;
                    enableCrashErrorLog = false;
                }

                public string appKey;
                public bool enableCrashReporter;
                public bool enableCrashErrorLog;
                public string serviceZone;
            }

            public class LogData
            {
                public string message;
                public Dictionary<string, string> userFields = new Dictionary<string, string>();
            }

            public class UserField
            {
                public string key;
                public string value;
            }
        }
    }    
}