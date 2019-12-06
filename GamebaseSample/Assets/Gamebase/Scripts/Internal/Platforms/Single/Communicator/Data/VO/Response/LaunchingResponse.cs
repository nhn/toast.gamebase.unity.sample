#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class LaunchingResponse
    {
        #region common data
        public class Request
        {
            public string clientVersion;
            public string deviceCountryCode;
            public string deviceKey;
            public string deviceModel;
            public string displayLanguage;
            public string deviceLanguage;
            public long lastCheckedNoticeTime;
            public string osCode;
            public string osVersion;
            public string sdkVersion;
            public string userId;
            public string usimCountryCode;
            public string uuid;
        }

        public class Status
        {
            public int code;
            public string message;
        }
        #endregion
        
        public class LaunchingInfo : BaseVO
        {
            public class Launching
            {
                public class App
                {
                    public class AccessInfo
                    {
                        public string serverAddress;
                        public string csInfo;
                    }
                    
                    public class RelatedUrls
                    {
                        public string termsUrl;
                        public string csUrl;
                        public string punishRuleUrl;
                        public string personalInfoCollectionUrl;
                    }

                    public class Install
                    {
                        public string url;
                    }

                    public class IDP
                    {
                        public string clientId;
                        public string clientSecret;
                        public string additional;
                    }

                    public class LoginUrls
                    {
                        public string gamebaseUrl;
                    }

                    public class Language
                    {
                        public string deviceLanguage;
                    }

                    public AccessInfo accessInfo;
                    public Dictionary<string, IDP> idP;
                    public Install install;
                    public RelatedUrls relatedUrls;
                    public Language language;
                    public string storeCode;
                    public string typeCode;
                    public LoginUrls loginUrls;
                }

                public class Maintenance
                {
                    public string url;
                    public string typeCode;
                    public string pageTypeCode;
                    public string reason;
                    public string message;
                    public string timezone;
                    public string beginDate;
                    public string endDate;
                    public long localBeginDate;
                    public long localEndDate;
                }

                public class Notice
                {
                    public string message;
                    public string title;
                    public string url;
                }

                public class TCGBClient
                {
                    public class Stability
                    {
                        public bool useFlag;
                        public bool useFlagSpecificUser;
                        public string logLevel;
                        public string appKey;
                        public long appKeyVersion;
                        public int initFailCount;
                    }

                    public class ForceRemoteSettings
                    {
                        public class Settings
                        {
                            public string policy;
                            public string indicator;
                            public string reason;
                            public string appKeyIndicator;
                            public string appKeyLog;
                        }
                        public Settings log;
                    }

                    public Stability stability;
                    public ForceRemoteSettings forceRemoteSettings;
                }
                
                public class Standalone
                {
                    public string loginUrl;
                    public string purchasedListUrl_kr;
                    public string purchasedListUrl_jp;
                }

                public App app;
                public Maintenance maintenance;
                public Notice notice;
                public Status status;
                public TCGBClient tcgbClient;
                public Standalone standalone;
            }

            public class TCIap
            {
                public string id;
                public string name;
                public string storeCode;
                public string storeAppId;
            }

            public class TCProduct
            {
                public TCProductAppKeyInfo gamebase;
                public TCProductAppKeyInfo tcLaunching;
                public TCProductAppKeyInfo iap;
                public TCProductAppKeyInfo push;

                public class TCProductAppKeyInfo
                {
                    public string appKey;
                }
            }

            public string date;
            public CommonResponse.Header header;
            public Request request;
            public Launching launching;
            public List<TCIap> tcIap;
            public TCProduct tcProduct;
            public string tcLaunching;
        }

        public class LaunchingStatus : BaseVO
        {
            public class Launching
            {
                public Status status;
            }
            
            public Launching launching;
            public Request request;
            public string date;
        }

        public class HeartbeatInfo : BaseVO
        {
            public CommonResponse.Header header;
        }
    }
}
#endif