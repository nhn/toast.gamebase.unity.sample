using System;
using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase
{
    public static class GamebaseResponse
    {
        public static class SDK
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

        public static class Event
        {
            public class GamebaseEventMessage
            {
                public string category;
                public string data;
            }

            public class GamebaseEventServerPushData
            {
                public string extras;

                public static GamebaseEventServerPushData From(string jsonString)
                {
                    GamebaseEventServerPushData gamebaseEventServerPushData = JsonMapper.ToObject<GamebaseEventServerPushData>(jsonString);
                    return gamebaseEventServerPushData;
                }
            }

            public class GamebaseEventObserverData
            {
                public int code;
                public string extras;
                public string message;

                public static GamebaseEventObserverData From(string jsonString)
                {
                    GamebaseEventObserverData gamebaseEventObserverData = JsonMapper.ToObject<GamebaseEventObserverData>(jsonString);
                    return gamebaseEventObserverData;
                }
            }

            public class PurchasableReceipt
            {
                public long itemSeq;
                public float price;           
                public string currency;
                public string paymentSeq;
                public string purchaseToken;
                public string marketItemId;
                public string productType;
                public string userId;
                public string paymentId;
                public string originalPaymentId;
                public string payload;
                public long purchaseTime;
                public long expiryTime;

                public static PurchasableReceipt From(string jsonString)
                {
                    PurchasableReceipt purchasableReceipt = JsonMapper.ToObject<PurchasableReceipt>(jsonString);
                    return purchasableReceipt;
                }
            }

            public class PushMessage
            {
                public string body;
                public string extras;
                public string id;
                public string title;

                public static PushMessage From(string jsonString)
                {
                    PushMessage pushMessage = JsonMapper.ToObject<PushMessage>(jsonString);
                    return pushMessage;
                }
            }

            public class PushAction
            {
                public string actionType;
                public PushMessage message;
                public string userText;

                public static PushAction From(string jsonString)
                {
                    PushAction pushAction = JsonMapper.ToObject<PushAction>(jsonString);
                    return pushAction;
                }
            }
        }

        public static class Launching
        {
            /// <summary>
            /// When Gamebase Unity SDK is initialized by using Initialize API, LaunchingInfo object results will be delievered.
            /// This LaunchingInfo object contains settings of the TOAST Gamebase Console and game status.
            /// </summary>
            public class LaunchingInfo
            {
                public GamebaseLaunching launching;
                public TCProductInfo tcProduct;
                public List<TCIAPInfo> tcIap;

                /// <summary>
                /// Launching Information entered by users through TOAST Launching Console.
                /// Retrieve what user entered from TOAST Console in JSON string format.
                /// Refer to the following guide for detail configuration of TOAST Launching.
                /// <para/><see href="https://docs.toast.com/en/Game/Gamebase/en/oper-management/#config">Gamebase and TOAST integration</see>
                /// </summary>
                public string tcLaunching;

                /// <summary>
                /// Launching information of Gamebase.
                /// </summary>
                public class GamebaseLaunching
                {
                    public LaunchingStatus status;
                    public APP app;
                    public Maintenance maintenance;
                    public LaunchingNotice notice;

                    /// <summary>
                    /// App information registered in the TOAST Console.
                    /// <para/>TOAST Console > Game > Gamebase > App
                    /// </summary>
                    public class APP
                    {
                        public AccessInfo accessInfo;
                        public RelatedURLs relatedUrls;
                        public Install install;

                        /// <summary>
                        /// Authentication information.
                        /// <para/>TOAST Console > Game > Gamebase > App > Authentication Information
                        /// </summary>
                        public Dictionary<string, LaunchingIDPInfo> idP;

                        /// <summary>
                        /// Gamebase environment. (e.g. REAL or SANDBOX)
                        /// </summary>
                        public string typeCode;

                        public LoginUrls loginUrls;
                        public CustomerService customerService;


                        public class AccessInfo
                        {
                            /// <summary>
                            /// Server URL.
                            /// <para/>TOAST Console > Game > Gamebase > App > Server URL
                            /// </summary>
                            public string serverAddress;
                        }

                        public class RelatedURLs
                        {
                            /// <summary>
                            /// Terms of Use.
                            /// <para/>TOAST Console > Game > Gamebase > App > InApp URL > Terms of Use
                            /// </summary>
                            public string termsUrl;

                            /// <summary>
                            /// Punishment Provision.
                            /// <para/>TOAST Console > Game > Gamebase > App > InApp URL > Punishment Provision
                            /// </summary>
                            public string punishRuleUrl;

                            /// <summary>
                            /// Personal Info Agreement.
                            /// <para/>TOAST Console > Game > Gamebase > App > InApp URL > Personal Info Agreement
                            /// </summary>
                            public string personalInfoCollectionUrl;
                        }

                        public class Install
                        {
                            /// <summary>
                            /// Install URL.
                            /// <para/>TOAST Console > Game > Gamebase > App > Install URL
                            /// </summary>
                            public string url;
                        }

                        public class LoginUrls
                        {
                            /// <summary>
                            /// Gamebase login url.
                            /// </summary>
                            public string gamebaseUrl;
                        }

                        public class LaunchingIDPInfo
                        {
                            /// <summary>
                            /// Client ID.
                            /// <para/>TOAST Console > Game > Gamebase > App > Authentication Information > Client ID
                            /// </summary>
                            public string clientId;

                            /// <summary>
                            /// Secret Key.
                            /// <para/>TOAST Console > Game > Gamebase > App > Authentication Information > Secret Key
                            /// </summary>
                            public string clientSecret;

                            /// <summary>
                            /// Additional Info.
                            /// <para/>TOAST Console > Game > Gamebase > App > Authentication Information > Additional Info 
                            /// </summary>
                            public string additional;
                        }

                        public class CustomerService
                        {
                            /// <summary>
                            /// Customer center type.
                            /// <para/>TOAST                | TOAST organization products (Online Contact).
                            /// <para/>GAMEBASE             | Customer Center provided by Gamebase
                            /// <para/>USER                 | Developer's own customer center.
                            /// </summary>
                            public string type;

                            /// <summary>
                            /// Customer center access URL.
                            /// </summary>
                            public string url;

                            /// <summary>
                            /// Customer center contact.
                            /// </summary>
                            public string accessInfo;
                        }
                    }

                    /// <summary>
                    /// Maintenance information registered in the TOAST Console.
                    /// <para/>TOAST Console > Game > Gamebase > Operation > Maintenance > Maintenance Register
                    /// </summary>
                    public class Maintenance
                    {
                        /// <summary>
                        /// Detailed maintenance URL.
                        /// Displayed when 'External link' type is selected in the Maintenance Page.
                        /// <para/>TOAST Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > URL
                        /// </summary>
                        public string url;

                        /// <summary>
                        /// <para/>APP: Maintenance set in a game.
                        /// <para/>SYSTEM: Maintenance set by the Gamebase system.
                        /// </summary>
                        public string typeCode;

                        /// <summary>
                        /// This pageTypeCode can be one of the following types.
                        /// <para/>DEFAULT: The gamebase webview html.(bundled resource in gamebase)
                        /// <para/>DEFAULT_HTML: The html registered in the TOAST Console.
                        /// <para/>URL: The external url.
                        /// <para/>URL_PARAM: The external url with maintenance information parameter.
                        /// </summary>
                        public string pageTypeCode;

                        /// <summary>
                        /// Provide reasons for maintenance simply.
                        /// This message is not shown to game users.
                        /// <para/>TOAST Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Reason
                        /// </summary>
                        public string reason;

                        /// <summary>
                        /// Maintenance message.
                        /// <para/>TOAST Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Message
                        /// </summary>
                        public string message;

                        /// <summary>
                        /// Standard timezone.
                        /// <para/>TOAST Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Time > Timezone
                        /// </summary>
                        public string timezone;

                        /// <summary>
                        /// Start time of maintenance. (ISO 8601)
                        /// <para/>TOAST Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Time > Start Date
                        /// </summary>
                        public string beginDate;

                        /// <summary>
                        /// End time of maintenance. (ISO 8601)
                        /// <para/>TOAST Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Time > End Date
                        /// </summary>
                        public string endDate;

                        /// <summary>
                        /// Start time of maintenance. (Epoch time)
                        /// </summary>
                        public long localBeginDate;

                        /// <summary>
                        /// End time of maintenance. (Epoch time)
                        /// </summary>
                        public long localEndDate;
                    }

                    /// <summary>
                    /// Notice information registered in the TOAST Console.
                    /// <para/>TOAST Console > Game > Gamebase > Operation > Notice > Notice Register
                    /// </summary>
                    public class LaunchingNotice
                    {
                        /// <summary>
                        /// Detailed message.
                        /// <para/>TOAST Console > Game > Gamebase > Operation > Notice > Notice Register > Message
                        /// </summary>
                        public string message;

                        /// <summary>
                        /// Title of notice.
                        /// <para/>TOAST Console > Game > Gamebase > Operation > Notice > Notice Register > Title
                        /// </summary>
                        public string title;

                        /// <summary>
                        /// The url of a page.
                        /// Displayed when 'Close + More' is selected in the Bottom button type.
                        /// <para/>TOAST Console > Game > Gamebase > Operation > Notice > Notice Register > Bottom button type
                        /// </summary>
                        public string url;
                    }                   
                }

                /// <summary>
                /// Appkey of TOAST Products linked to Gamebase.
                /// </summary>
                public class TCProductInfo
                {
                    /// <summary>
                    /// Appkey of Gamebase.
                    /// </summary>
                    public TCProductAppKeyInfo gamebase;

                    /// <summary>
                    /// Appkey of TOAST Launching.
                    /// </summary>
                    public TCProductAppKeyInfo tcLaunching;

                    /// <summary>
                    /// Appkey of TOAST IAP.
                    /// </summary>
                    public TCProductAppKeyInfo iap;

                    /// <summary>
                    /// Appkey of TOAST Push.
                    /// </summary>
                    public TCProductAppKeyInfo push;

                    public class TCProductAppKeyInfo
                    {
                        public string appKey;
                    }
                }

                /// <summary>
                /// IAP store information registered in the TOAST Console.
                /// </summary>
                public class TCIAPInfo
                {
                    /// <summary>
                    /// App ID.
                    /// </summary>
                    public string id;

                    /// <summary>
                    /// App name.
                    /// </summary>
                    public string name;

                    /// <summary>
                    /// Store Code.
                    /// <para/>App Store    | AS        | only iOS
                    /// <para/>Google Play  | GG        | only Android
                    /// <para/>One Store    | ONESTORE  | only Android
                    /// <para/>Windows      | WIN       | only Unity Standalone
                    /// <para/>Web          | WEB       | only Unity WebGL and JavaScript
                    /// </summary>
                    public string storeCode;
                }                
            }

            /// <summary>
            /// Status information of game app version set in the Gamebase Unity SDK initialization.
            /// <para/>IN_SERVICE                   | 200 | Service is now normally provided.
            /// <para/>RECOMMEND_UPDATE             | 201 | Update is recommended.
            /// <para/>IN_SERVICE_BY_QA_WHITE_LIST  | 202 | Under maintenance now but QA user service is available.
            /// <para/>IN_TEST                      | 203 | Under test.
            /// <para/>IN_REVIEW                    | 204 | Review in progress.
            /// <para/>REQUIRE_UPDATE               | 300 | Update is required.
            /// <para/>BLOCKED_USER                 | 301 | User whose access has been blocked.
            /// <para/>TERMINATED_SERVICE           | 302 | Service has been terminated.
            /// <para/>INSPECTING_SERVICE           | 303 | Under maintenance now.
            /// <para/>INSPECTING_ALL_SERVICES      | 304 | Under maintenance for the whole service.
            /// <para/>INTERNAL_SERVER_ERROR        | 500 | Error of internal server.
            /// </summary>
            public class LaunchingStatus
            {
                /// <summary>
                /// Game status code. (e.g. 200 ~ 500)
                /// </summary>
                public int code;

                /// <summary>
                /// Game status message.
                /// </summary>
                public string message;
            }

            public class UpdateInfo
            {
                public string installUrl;
                public string message;

                public static UpdateInfo From(GamebaseError error)
                {
                    if (error == null || error.extras == null)
                    {
                        return null;
                    }

                    string jsonString;
                    if(error.extras.TryGetValue(GamebaseErrorExtras.UPDATE_INFO, out jsonString) == false)
                    {
                        return null;
                    }
                    UpdateInfo updateInfo = JsonMapper.ToObject<UpdateInfo>(jsonString);
                    return updateInfo;
                }
            }
        }

        public static class Auth
        {
            public class AuthToken
            {
                public Token token;
                public Common.Member member;

                public class Token
                {
                    /// <summary>
                    /// The authentication information(access token) received after login to IdP.
                    /// </summary>
                    public string accessToken;

                    /// <summary>
                    /// The authentication information(access token secret) received after login to IdP.
                    /// </summary>
                    public string accessTokenSecret;
                }
            }

            /// <summary>
            /// Get access token, User ID, and profiles from externally authenticated SDK.
            /// </summary>
            public class AuthProviderProfile
            {
                public Dictionary<string, object> information;
            }

            /// <summary>
            /// Ban information for user who have been banned.
            /// </summary>
            public class BanInfo
            {
                /// <summary>
                /// User ID who was banned.
                /// </summary>
                public string userId;

                /// <summary>
                /// There are two types of the banning.
                /// (e.g. TEMPORARY or PERMANENT)
                /// </summary>
                public string banType;

                /// <summary>
                /// Time when banning was started.
                /// </summary>
                public long beginDate;

                /// <summary>
                /// Time when banning will be released.
                /// </summary>
                public long endDate;

                /// <summary>
                ///  Banning message.
                /// <para/>TOAST Console > Game > Gamebase > Ban > Register Ban > Message
                /// </summary>
                public string message;

                /// <summary>
                /// Customer center information. (e.g. Customer center email or contact)
                /// <para/>TOAST Console > Game > Gamebase > App > Customer center information
                /// </summary>
                public string csInfo;

                /// <summary>
                /// Service center. (e.g. The URL of the customer center web page.)
                /// <para/>TOAST Console > Game > Gamebase > App > InApp URL > Service center
                /// </summary>
                public string csUrl;

                public static BanInfo From(GamebaseError error)
                {
                    if (error == null || error.extras == null)
                    {
                        return null;
                    }

                    string jsonString;
                    if (error.extras.TryGetValue(GamebaseErrorExtras.BAN_INFO, out jsonString) == false)
                    {
                        return null;
                    }
                    BanInfo banInfo = JsonMapper.ToObject<BanInfo>(jsonString);
                    return banInfo;
                }
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
                public string userId;
                public string mappedUserId;
                public string idPCode;
                public string forcingMappingKey;
                public long expirationDate;
                public string accessToken;
                
                [Obsolete("As of release 2.9.0, use GamebaseResponse.Auth.ForcingMappingTicket.From instead.")]
                public static ForcingMappingTicket MakeForcingMappingTicket(GamebaseError error)
                {
                    return GetForcingMappingTicket(error);
                }

                public static ForcingMappingTicket From(GamebaseError error)
                {
                    return GetForcingMappingTicket(error);
                }

                private static ForcingMappingTicket GetForcingMappingTicket(GamebaseError error)
                {
                    if (error == null || error.extras == null)
                    {
                        return null;
                    }

                    string jsonString = string.Empty;

                    if (error.extras.TryGetValue(GamebaseErrorExtras.FORCING_MAPPING_TICKET, out jsonString) == false)
                    {
                        return null;
                    }

                    return JsonMapper.ToObject<ForcingMappingTicket>(jsonString);
                }
            }

            public class TransferAccountFailInfo
            {
                public string appId         = string.Empty;
                public string id            = string.Empty;
                public string status        = string.Empty;
                public int failCount        = 0;
                public long blockEndDate    = 0;
                public long regDate         = 0;

                [Obsolete("As of release 2.9.0, use GamebaseResponse.Auth.TransferAccountFailInfo.From instead.")]
                public static TransferAccountFailInfo MakeTransferAccountFailInfo(GamebaseError error)
                {
                    return GetTransferAccountFailInfo(error);
                }

                public static TransferAccountFailInfo From(GamebaseError error)
                {
                    return GetTransferAccountFailInfo(error);
                }

                private static TransferAccountFailInfo GetTransferAccountFailInfo(GamebaseError error)
                {
                    if (error == null || error.extras == null)
                    {
                        return null;
                    }

                    string jsonString = string.Empty;

                    if (error.extras.TryGetValue(GamebaseErrorExtras.FAIL_TRANSFER_ACCOUNT, out jsonString) == false)
                    {
                        return null;
                    }

                    return JsonMapper.ToObject<TransferAccountFailInfo>(jsonString);
                }
            }
        }

        public static class Purchase
        {
            public class PurchasableItem
            {
                /// <summary>
                /// gamebaseProductId registered in the TOAST console.
                /// </summary>
                public string gamebaseProductId;

                /// <summary>
                /// Item unique identifier.
                /// </summary>
                public long itemSeq;

                /// <summary>
                /// Price of an item.
                /// </summary>
                public float price;

                /// <summary>
                /// Type of currency.
                /// </summary>
                public string currency;

                /// <summary>
                /// The name of the item.
                /// </summary>
                public string itemName;

                /// <summary>
                /// Item unique identifier registered in the Market.
                /// </summary>
                public string marketItemId;

                /// <summary>
                /// Type of product. (e.g. consumable or auto renewable)
                /// <para/><see cref="GamebasePurchase.ProductType"/>
                /// </summary>
                public string productType;

                /// <summary>
                /// Local price.
                /// </summary>
                public string localizedPrice;

                /// <summary>
                /// Local title.
                /// </summary>
                public string localizedTitle;

                /// <summary>
                /// Local description.
                /// </summary>
                public string localizedDescription;

                /// <summary>
                /// Whether the product is active.
                /// </summary>
                public bool isActive;
            }

            public class PurchasableReceipt
            {
                /// <summary>
                /// gamebaseProductId registered in the TOAST console.
                /// </summary>
                public string gamebaseProductId;

                /// <summary>
                /// Item unique identifier.
                /// </summary>
                public long itemSeq;

                /// <summary>
                /// Price of an item.
                /// </summary>
                public float price;

                /// <summary>
                /// Type of currency.
                /// </summary>
                public string currency;

                /// <summary>
                /// Payment unique identifier.
                /// </summary>
                public string paymentSeq;

                /// <summary>
                /// You will need paymentSeq and purchaseToken when calling the Consume API.
                /// Refer to the following document for the Consume API.
                /// <para/><see href="https://docs.toast.com/en/Game/Gamebase/en/api-guide/#purchase-iap">Consume API</see>
                /// </summary>
                public string purchaseToken;

                /// <summary>
                /// Item unique identifier registered in the Market.
                /// </summary>
                public string marketItemId;

                /// <summary>
                /// Type of product. (e.g. consumable or auto renewable)
                /// <para/><see cref="GamebasePurchase.ProductType"/>
                /// </summary>
                public string productType;

                /// <summary>
                /// User ID.
                /// </summary>
                public string userId;

                /// <summary>
                /// Latest store payment unique identifier.
                /// </summary>
                public string paymentId;

                /// <summary>
                /// Original store payment unique identifier.
                /// </summary>
                public string originalPaymentId;

                /// <summary>
                /// The time the product was purchased.
                /// </summary>
                public long purchaseTime;

                /// <summary>
                /// Expiry time.
                /// </summary>
                public long expiryTime;

                /// <summary>
                /// The input payload is delivered when the purchase is completed.
                /// </summary>
                public string payload;
            }

            /// <summary>
            /// The PurchasableRetryTransactionResult class represent a result after retrying failed purchasing processes.
            /// </summary>
            public class PurchasableRetryTransactionResult
            {
                /// <summary>
                /// This List contains results of succeeded receipt.
                /// </summary>
                public List<PurchasableReceipt> successList;

                /// <summary>
                /// This List contains results of failed receipt.
                /// </summary>
                public List<PurchasableReceipt> failList;
            }            
        }

        public static class Push
        {
            /// <summary>
            /// Get push settings from the push server.
            /// </summary>
            public class PushConfiguration
            {
                private static class AgreePushType
                {
                    public const string NONE = "NONE";
                    public const string DAY = "DAY";
                    public const string NIGHT = "NIGHT";
                    public const string ALL = "ALL";
                }

                private class AgreePush
                {
                    public string agreePush = null;
                }

                /// <summary>
                /// Enable or disable all of the notifications.
                /// </summary>
                public bool pushEnabled;

                /// <summary>
                /// Agree to receive push notification ads.
                /// </summary>
                public bool adAgreement;

                /// <summary>
                /// Agree to receive push notification ads at night.
                /// </summary>
                public bool adAgreementNight;

                /// <summary>
                /// The display language on the push notification UI.
                /// </summary>
                public string displayLanguageCode;

                public static PushConfiguration From(DataContainer dataContainer)
                {
                    if (dataContainer == null || string.IsNullOrEmpty(dataContainer.data) == true)
                    {
                        return null;
                    }

                    AgreePush agreePush = JsonMapper.ToObject<AgreePush>(dataContainer.data);
                    if (agreePush == null)
                    {
                        return null;
                    }

                    if (agreePush.agreePush == null)
                    {
                        return null;
                    }
                    
                    PushConfiguration configuration = new PushConfiguration();

                    switch (agreePush.agreePush)
                    {
                        case AgreePushType.DAY:
                            {
                                configuration.pushEnabled = true;
                                configuration.adAgreement = true;
                                configuration.adAgreementNight = false;
                                break;
                            }
                        case AgreePushType.NIGHT:
                            {
                                configuration.pushEnabled = true;
                                configuration.adAgreement = false;
                                configuration.adAgreementNight = true;
                                break;
                            }
                        case AgreePushType.ALL:
                            {
                                configuration.pushEnabled = true;
                                configuration.adAgreement = true;
                                configuration.adAgreementNight = true;
                                break;
                            }
                        default:
                            {
                                // 'agreePush' values handled in this case: NONE and other (including empty string)
                                configuration.pushEnabled = true;
                                configuration.adAgreement = false;
                                configuration.adAgreementNight = false;
                                break;
                            }
                    }

                    return configuration;
                }
            }

            /// <summary>
            /// Registered notification option when calling the RegisterPush API.
            /// <para/><see cref="Gamebase.Push.RegisterPush(GamebaseRequest.Push.PushConfiguration, GamebaseRequest.Push.NotificationOptions, GamebaseCallback.ErrorDelegate)"/>
            /// </summary>
            public class NotificationOptions
            {
                /// <summary>
                /// Whether to receive notifications when the application is foreground.
                /// </summary>
                public bool foregroundEnabled;

                /// <summary>
                /// Whether to use the badge icon.
                /// </summary>
                public bool badgeEnabled;

                /// <summary>
                /// iOS only
                /// Whether to use notification sound.
                /// </summary>
                public bool soundEnabled;

                /// <summary>
                /// Android only
                /// Notification priority.
                /// <para/>Priority is defined in <see cref="GamebaseNotificationPriority"/>.
                /// </summary>
                public int priority;

                /// <summary>
                /// Android only
                /// The name of the small icon resource, which will be used to represent the notification in the status bar.
                /// </summary>
                public string smallIconName;

                /// <summary>
                /// Android only: This is deprecated in API 26(Android 8.0 Oreo) but you can still use for below 26.
                /// If you specify the file name of mp3 or wav extension in the 'res/raw' folder, the notification sound changes.
                /// </summary>
                public string soundFileName;
            }

            public class Agreement
            {
                /// <summary>
                /// Enable or disable all of the notifications.
                /// </summary>
                public bool pushEnabled;

                /// <summary>
                /// Agree to receive push notification ads.
                /// </summary>
                public bool adAgreement;

                /// <summary>
                /// Agree to receive push notification ads at night.
                /// </summary>
                public bool adAgreementNight;
            }

            /// <summary>
            /// Get token information from the push server.
            /// </summary>
            public class TokenInfo
            {
                /// <summary>
                /// iOS only
                /// Sandbox Environment.
                /// </summary>
                public bool sandbox;

                /// <summary>
                /// Push types (e.g. APNS, APNS_SANDBOX or GCM or TENCENT)
                /// </summary>
                public string pushType;

                /// <summary>
                /// Device token.
                /// </summary>
                public string token;

                /// <summary>
                /// User ID.
                /// </summary>
                public string userId;

                /// <summary>
                /// Country code of user device.
                /// </summary>
                public string deviceCountryCode;

                /// <summary>
                /// Timezone.
                /// </summary>
                public string timezone;

                /// <summary>
                /// Time the token was updated.
                /// </summary>
                public string registeredDateTime;

                /// <summary>
                /// The standard language code.
                /// </summary>
                public string languageCode;

                public Agreement agreement;
            }
        }

        public static class Logger
        {
            /// <summary>
            /// The properties of CrashLogData are the same as the parameters of Application.LogCallback.
            /// </summary>
            public class CrashLogData
            {
                /// <summary>
                /// The type of the log message in Debug.logger.Log or delegate registered with Application.RegisterLogCallback.
                /// <para/><see cref="UnityEngine.LogType"/>
                /// </summary>
                public LogType logType;

                /// <summary>
                /// Condition.
                /// </summary>
                public string condition;

                /// <summary>
                /// StackTrace.
                /// </summary>
                public string stackTrace;
            }

            public class LogEntry
            {
                /// <summary>
                /// Log types are defined in <see cref="Toast.Logger.ToastLoggerType"/>.
                /// </summary>
                public string logType;

                /// <summary>
                /// Log levels are defined in <see cref="GamebaseLoggerConst.LogLevel"/>.
                /// </summary>
                public GamebaseLoggerConst.LogLevel logLevel;

                /// <summary>
                /// Log messages to send to the Log &amp; Crash server.
                /// </summary>
                public string message;

                /// <summary>
                /// Original log number.
                /// </summary>
                public string transactionId;

                /// <summary>
                /// The time when the log was created.
                /// </summary>
                public long createTime;

                /// <summary>
                /// Additional information to send to the Log &amp; Crash server.
                /// </summary>
                public Dictionary<string, object> userFields = new Dictionary<string, object>();
            }

            public class LogFilter
            {
                /// <summary>
                /// The filter used when filtering log.
                /// </summary>
                public string name;
            }
        }

        public static class Common
        {
            public class Member
            {
                /// <summary>
                /// The list of IdPs mapped to user IDs.
                /// </summary>
                public class AuthMappingInfo
                {
                    /// <summary>
                    /// User separator issued at authSystem.
                    /// </summary>
                    public string authKey;

                    /// <summary>
                    /// Authentication system internally used within Gamebase.
                    /// User authentication system to be provided.
                    /// </summary>
                    public string authSystem;

                    /// <summary>
                    /// User-authenticated IdP information. (e.g. Guest, PAYCO, and Facebook)
                    /// </summary>
                    public string idPCode;

                    /// <summary>
                    /// Mapping time between IdP information with user account.
                    /// </summary>
                    public long regDate;

                    /// <summary>
                    /// User ID.
                    /// </summary>
                    public string userId;
                }
                
                public class GraceBanInfo
                {
                    public class PaymentStatus
                    {
                        /// <summary>
                        /// Amount paid.
                        /// </summary>
                        public double amount;
                        
                        /// <summary>
                        /// Number of payment required.
                        /// </summary>
                        public int count;
                        
                        /// <summary>
                        /// Currency.
                        /// </summary>
                        public string currency;
                    }

                    public class ReleaseRuleCondition
                    {   
                        /// <summary>
                        /// Amount to be paid.
                        /// </summary>
                        public double amount;
                        
                        /// <summary>
                        /// Number of payment required.
                        /// </summary>
                        public int count;
                        
                        /// <summary>
                        /// Currency.
                        /// </summary>
                        public string currency;
                        
                        /// <summary>
                        /// Condition for amount and count. ("AND" / "OR")
                        /// </summary>
                        public string conditionType;
                    }
                    
                    /// <summary>
                    /// Grace period expiration date. (epoch time in milliseconds)
                    /// If the payment conditions set before the period are satisfied,
                    /// user's ban will be released.
                    /// </summary>
                    public long gracePeriodDate;
                    
                    /// <summary>
                    /// Message about grace ban. (Encoded string)
                    /// </summary>
                    public string message;
                    
                    /// <summary>
                    /// Current payment status.
                    /// </summary>
                    public PaymentStatus paymentStatus;
                    
                    /// <summary>
                    /// Payment condition to release ban.
                    /// </summary>
                    public ReleaseRuleCondition releaseRuleCondition;
                }

                /// <summary>
                /// App ID.
                /// </summary>
                public string appId;

                /// <summary>
                /// The list of IdPs mapped to user IDs.
                /// </summary>
                public List<AuthMappingInfo> authList;

                /// <summary>
                /// Last login time.
                /// Not available for first-time login user.
                /// </summary>
                public long lastLoginDate;

                /// <summary>
                /// Time when a user created an account.
                /// </summary>
                public long regDate;

                /// <summary>
                /// User ID.
                /// </summary>
                public string userId;

                /// <summary>
                /// Value of a successful login user is "Y".
                /// </summary>
                public string valid;
                
                /// <summary>
                /// Temporary withdrawal information.
                /// </summary>
                public TemporaryWithdrawalInfo temporaryWithdrawal;
                
                /// <summary>
                /// Grace ban information.
                /// </summary>
                public GraceBanInfo graceBan;
            }
        }

        public class TemporaryWithdrawalInfo
        {
            /// <summary>
            /// The grace expiration time.
            /// </summary>
            public long gracePeriodDate;
        }

        public class DataContainer
        {
            /// <summary>
            /// JSON string type data
            /// </summary>
            public string data;
        }

        public static class Terms
        {
            public class QueryTermsResult
            {
                /// <summary>
                /// Version of the Terms.
                /// This value is required when calling the updateTerms API.
                /// </summary>
                public string termsVersion;

                /// <summary>
                /// Unique identifier for terms and conditions.
                /// This value is required when calling the updateTerms API.
                /// </summary>
                public int termsSeq;

                /// <summary>
                /// Country code.
                /// </summary>
                public string termsCountryType;

                /// <summary>
                /// Terms and conditions details.
                /// </summary>
                public List<ContentDetail> contents;
            }

            public class ContentDetail
            {
                /// <summary>
                /// Unique identifier for terms and conditions.
                /// </summary>
                public int termsContentSeq;

                /// <summary>
                /// Terms of use name.
                /// </summary>
                public string name;

                /// <summary>
                /// Required consent.
                /// </summary>
                public bool required;

                /// <summary>
                /// Whether to consent to advertising push.
                /// </summary>
                public string agreePush;

                /// <summary>
                /// Whether the user agrees to the terms and conditions.
                /// </summary>
                public bool agreed;

                /// <summary>
                /// Step 1 item exposure order.
                /// </summary>
                public int node1DepthPosition;

                /// <summary>
                /// Step 2 item exposure order.
                /// </summary>
                public int node2DepthPosition = -1;

                /// <summary>
                /// URL where you can see details about the terms and conditions
                /// </summary>
                public string detailPageUrl;
            }
        }
    }    
}