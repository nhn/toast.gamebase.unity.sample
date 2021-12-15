using System.Collections.Generic;

namespace Toast.Gamebase.Internal
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
                    }
                    
                    public class RelatedUrls
                    {
                        public string termsUrl;
                        public string punishRuleUrl;
                        public string personalInfoCollectionUrl;
                    }

                    public class Install
                    {
                        public string url;
                    }

                    public class IDP
                    {
                        public class LoginWebView
                        {
                            public string titleBgColor;
                            public string titleTextColor;
                            public string title;
                        }

                        public string clientId;
                        public string clientSecret;
                        public string additional;
                        public string callbackUrl;

                        public LoginWebView loginWebView;
                    }

                    public class LoginUrls
                    {
                        public string gamebaseUrl;
                    }

                    public class Language
                    {
                        public string deviceLanguage;
                        public string defaultLanguage;
                    }

                    public class CustomerService
                    {
                        public string type;
                        public string url;
                        public string accessInfo;
                    }

                    public AccessInfo accessInfo;
                    public Dictionary<string, IDP> idP;
                    public Install install;
                    public RelatedUrls relatedUrls;
                    public Language language;
                    public string storeCode;
                    public string typeCode;
                    public LoginUrls loginUrls;
                    public CustomerService customerService;
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

                    public class Introspection
                    {
                        public int intervalSeconds;
                    }

                    public Introspection introspection;
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

            public class ErrorExtras
            {
                public class UpdateInfo
                {
                    public string installUrl;
                    public string message;
                }

                public class Language
                {
                    public string deviceLanguage;
                    public string defaultLanguage;
                }

                public UpdateInfo updateInfo;
                public Language language;
            }

            public class ImageNoticeWeb
            {
                public class ImageNoticeInfo
                {
                    public int imageNoticeId;
                    public string path;
                    public string clickScheme;
                    public string clickType;
                }

                public string domain;
                public List<ImageNoticeInfo> pageList;
            }

            public string date;
            public CommonResponse.Header header;
            public Request request;
            public Launching launching;
            public List<TCIap> tcIap;
            public TCProduct tcProduct;
            public string tcLaunching;
            public ErrorExtras errorExtras;
            public ImageNoticeWeb imageNoticeWeb;
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

        public class IntrospectInfo : BaseVO
        {
            public CommonResponse.Header header;
        }
    }
}