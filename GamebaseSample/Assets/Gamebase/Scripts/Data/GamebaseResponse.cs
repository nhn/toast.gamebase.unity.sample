using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase
{
    public class GamebaseResponse
    {
        public class SDK
        {
            public class ObserverMessage
            {
                public string type;
                public Dictionary<string, object> data;
            }

            public class ServerPushMessage
            {
                public string type;
                public string data;
            }
        }

        public class Launching
        {
            public class LaunchingInfo
            {
                public GamebaseLaunching launching;
                public TCProductInfo tcProduct;
                public List<TCIAPInfo> tcIap;
                public string tcLaunching;

                public class GamebaseLaunching
                {
                    public LaunchingStatus status;
                    public APP app;
                    public Maintenance maintenance;
                    public LaunchingNotice notice;
                    public TCGBClient tcgbClient;

                    public class APP
                    {
                        public AccessInfo accessInfo;
                        public RelatedURLs relatedUrls;
                        public Install install;
                        public Dictionary<string, LaunchingIDPInfo> idP;
                        public string typeCode;
                        public LoginUrls loginUrls;                        

                        public class AccessInfo
                        {
                            public string serverAddress;
                            public string csInfo;
                        }

                        public class RelatedURLs
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

                        public class LoginUrls
                        {
                            public string gamebaseUrl;
                        }

                        public class LaunchingIDPInfo
                        {
                            public string clientId;
                            public string clientSecret;
                            public string additional;
                        }                        
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

                    public class LaunchingNotice
                    {
                        public string message;
                        public string title;
                        public string url;
                    }

                    public class TCGBClient
                    {
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

                        public class Stability
                        {
                            public bool useFlag;
                            public bool useFlagSpecificUser;
                            public string logLevel;
                            public string appKey;
                            public long appKeyVersion;
                            public int initFailCount;
                        }

                        public Stability stability;
                        public ForceRemoteSettings forceRemoteSettings;
                    }
                }

                public class TCProductInfo
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

                public class TCIAPInfo
                {
                    public string id;
                    public string name;
                    public string storeCode;
                }                
            } 

            public class LaunchingStatus
            {
                public int code;
                public string message;
            }
        }

        public class Auth
        {
            public class AuthToken
            {
                public Token token;
                public Common.Member member;

                public class Token
                {
                    public string accessToken;
                    public string accessTokenSecret;
                }
            }

            public class AuthProviderProfile
            {
                public Dictionary<string, object> information;
            }

            public class BanInfo
            {
                public string userId;
                public string banType;
                public long beginDate;
                public long endDate;
                public string message;
                public string csInfo;
                public string csUrl;
            }

            public class TransferAccountInfo
            {
                public class Account
                {
                    public string id;
                    public string password;
                }

                public class Condition
                {
                    public string transferAccountType;
                    public string expirationType;
                    public long expirationDate;
                }

                public string issuedType;
                public Account account;
                public Condition condition;
            }

            public class TransferKeyInfo
            {
                public string transferKey;
                public long regDate;
                public long expireDate;
            }

            public class ForcingMappingTicket
            {
                private const string FORCING_MAPPING_TICKET = "forcingMappingTicket";

                public string userId;
                public string mappedUserId;
                public string idPCode;
                public string forcingMappingKey;
                public long expirationDate;

                public static ForcingMappingTicket MakeForcingMappingTicket(GamebaseError error)
                {
                    if (error == null || error.extras == null)
                    {
                        return null;
                    }

                    if (error.extras.ContainsKey(FORCING_MAPPING_TICKET) == true)
                    {
                        string jsonString = error.extras[FORCING_MAPPING_TICKET];

                        if (string.IsNullOrEmpty(jsonString) == true)
                        {
                            return null;
                        }
                        ForcingMappingTicket forcingMappingTicket = JsonMapper.ToObject<ForcingMappingTicket>(jsonString);

                        return forcingMappingTicket;
                    }

                    return null;
                }
            }
            public class TransferAccountFailInfo
            {
                private const string APP_ID                 = "appId";
                private const string ID                     = "id";
                private const string STATUS                 = "status";
                private const string FAIL_COUNT             = "failCount";
                private const string BLOCK_END_DATA         = "blockEndDate";
                private const string REG_DATA               = "regDate";
                private const string FAIL_TRANSFER_ACCOUNT  = "failTransferAccount";

                public string appId         = string.Empty;
                public string id            = string.Empty;
                public string status        = string.Empty;
                public int failCount        = 0;
                public long blockEndDate    = 0;
                public long regDate         = 0;

                public static TransferAccountFailInfo MakeTransferAccountFailInfo(GamebaseError error)
                {
                    if(error == null || error.extras == null)
                    {
                        return null;
                    }

                    if(error.extras.ContainsKey(FAIL_TRANSFER_ACCOUNT) == true)
                    {
                        string jsonString = error.extras[FAIL_TRANSFER_ACCOUNT];

                        if(string.IsNullOrEmpty(jsonString) == true)
                        {
                            return null;
                        }
                        TransferAccountFailInfo transferAccountFailInfo = JsonMapper.ToObject<TransferAccountFailInfo>(jsonString);
                        
                        return transferAccountFailInfo;
                    }

                    return null;
                }
            }
        }

        public class Purchase
        {
            public class PurchasableReceipt
            {
                public long itemSeq;
                public float price;
                public string currency;
                public string paymentSeq;
                public string purchaseToken;
                public string marketItemId;
            }

            public class PurchasableRetryTransactionResult
            {
                public List<PurchasableReceipt> successList;
                public List<PurchasableReceipt> failList;
            }

            public class PurchasableItem
            {
                public long itemSeq;
                public float price;
                public string currency;
                public string itemName;
                public string marketItemId;
            }

            public class IapPurchase
            {
                public string paymentId;
                public string paymentSequence;
                public string originalPaymentId;
                public string ProductId;
                public GamebasePurchase.ProductType productType;
                public string userId;
                public float price;
                public string priceCurrencyCode;
                public string accessToken;
                public long purchaseTime;
                public long expiryTime;
            }

            public class IapProduct
            {
                public string productSeq;
                public string productId;
                public string productName;
                public GamebasePurchase.ProductType productType;
                public bool isActive;
                public float price;
                public string currency;
                public string localizedPrice;
            }

            public class RequestProductDetails
            {
                public List<IapProduct> Products { get; set; }
                public List<IapProduct> InvalidProducts { get; set; }
            }
        }

        public class Push
        {
            public class PushConfiguration
            {
                public bool pushEnabled;
                public bool adAgreement;
                public bool adAgreementNight;
                public string displayLanguageCode;
            }

            public class PushAgreement
            {
                public bool allowAdvertisements;
                public bool allowNightAdvertisements;
                public bool allowNotifications;
            }

            public class PushTokenInfo
            {
                public string activatedDateTime;
                public PushAgreement agreement;
                public string country;
                public string language;
                public string pushType;
                public string timeZone;
                public string token;
                public string userId;
            }
        }

        public static class Logger
        {
            public class CrashLogData
            {               
                public GamebaseLoggerConst.LogType logType;
                public string condition;
                public string stackTrace;
            }

            public class LogEntry
            {
                public string logType;
                public GamebaseLoggerConst.LogLevel logLevel;
                public string message;
                public string transactionId;
                public long createTime;
                public Dictionary<string, object> userFields = new Dictionary<string, object>();
            }

            public class LogFilter
            {
                public string name;
            }

            public class LoggerListenerData
            {
                public class LogEntryData
                {
                    public string logType;
                    public int logLevel;
                    public string message;
                    public string transactionId;
                    public long createTime;
                    public Dictionary<string, object> userFields = new Dictionary<string, object>();
                }
                
                public string type;
                public LogEntryData logEntryData;
                public LogFilter logFilterData;
                public string errorMessage;
            }

            public class CrashListenerData
            {
                public bool isSuccess;
                public LogEntry logEntry;
            }
        }

        public class Util
        {

        }

        public class Common
        {
            public class Member
            {
                public class AuthMappingInfo
                {
                    public string authKey;
                    public string authSystem;
                    public string idPCode;
                    public long regDate;
                    public string userId;
                }

                public string appId;
                public List<AuthMappingInfo> authList;
                public long lastLoginDate;
                public long regDate;
                public string userId;
                public string valid;
            }
        }
    }
}