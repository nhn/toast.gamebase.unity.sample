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
                /// <summary>
                /// Represents the type of an event.
                /// The value of the GamebaseEventCategory class is assigned.
                /// </summary>
                public string category;

                /// <summary>
                /// JSON String data that can be converted into a VO that is appropriate for each category.
                /// </summary>
                public string data;
            }

            public class GamebaseEventServerPushData
            {
                /// <summary>
                /// Information of the popup to display.
                /// </summary>
                public ServerPushPopup popup;

                /// <summary>
                /// A reserved field for additional information.
                /// </summary>
                public string extras;

                public class ServerPushPopup
                {
                    /// <summary>
                    /// If device language does not exist in the message list, the default language is displayed.
                    /// </summary>
                    public string defaultLanguage;

                    /// <summary>
                    /// A list of messages by language.
                    /// </summary>
                    public Dictionary<string, ServerPushPopupMessage> messages;
            
                    public class ServerPushPopupMessage
                    {
                        /// <summary>
                        /// The title of the popup.
                        /// </summary>
                        public string title;

                        /// <summary>
                        /// The detaile message of the popup.
                        /// </summary>
                        public string message;
                    }
                }

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="jsonString">Extracts object instance from this JSON string.</param>
                /// <returns>Returns an instantiated object.</returns>
                public static GamebaseEventServerPushData From(string jsonString)
                {
                    GamebaseEventServerPushData serverPushData = JsonMapper.ToObject<GamebaseEventServerPushData>(jsonString);
                    return serverPushData;
                }
            }

            public class GamebaseEventObserverData
            {
                /// <summary>
                /// This information represents the status value.
                /// </summary>
                public int code;

                /// <summary>
                /// This information shows the message about status.
                /// </summary>
                public string message;

                /// <summary>
                /// A reserved field for additional information.
                /// </summary>
                public string extras;

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="jsonString">Extracts object instance from this JSON string.</param>
                /// <returns>Returns an instantiated object.</returns>
                public static GamebaseEventObserverData From(string jsonString)
                {
                    GamebaseEventObserverData observerData = JsonMapper.ToObject<GamebaseEventObserverData>(jsonString);
                    return observerData;
                }
            }

            public class GamebaseEventLoggedOutData
            {
                /// <summary>
                /// This information shows the message about status.
                /// </summary>
                public string message;

                /// <summary>
                /// A reserved field for additional information.
                /// </summary>
                public string extras;

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="jsonString">Extracts object instance from this JSON string.</param>
                /// <returns>Returns an instantiated object.</returns>
                public static GamebaseEventLoggedOutData From(string jsonString)
                {
                    GamebaseEventLoggedOutData loggedOutData = JsonMapper.ToObject<GamebaseEventLoggedOutData>(jsonString);
                    return loggedOutData;
                }
            }
            
            public class GamebaseEventIdPRevokedData
            {
                /// <summary>
                /// Indicates the GamebaseIdPRevokedCode value.
                /// <para/><see cref="GamebaseIdPRevokedCode"/>
                /// </summary>
                public int code;

                /// <summary>
                /// Indicates the revoked IdP type.
                /// </summary>
                public string idPType;

                /// <summary>
                /// Indicates the list of IdPs mapped to the current account.
                /// </summary>
                public List<string> authMappingList;

                /// <summary>
                /// A reserved field for additional information.
                /// </summary>
                public string extras;

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="jsonString">Extracts object instance from this JSON string.</param>
                /// <returns>Returns an instantiated object.</returns>
                public static GamebaseEventIdPRevokedData From(string jsonString)
                {
                    GamebaseEventIdPRevokedData idPRevokedData = JsonMapper.ToObject<GamebaseEventIdPRevokedData>(jsonString);
                    return idPRevokedData;
                }
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
                /// The input payload is delivered when the purchase is completed.
                /// </summary>
                public string payload;

                /// <summary>
                /// The time the product was purchased.
                /// </summary>
                public long purchaseTime;

                /// <summary>
                /// Expiry time.
                /// </summary>
                public long expiryTime;
                
                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="jsonString">Extracts object instance from this JSON string.</param>
                /// <returns>Returns an instantiated object.</returns>
                public static PurchasableReceipt From(string jsonString)
                {
                    PurchasableReceipt purchasableReceipt = JsonMapper.ToObject<PurchasableReceipt>(jsonString);
                    return purchasableReceipt;
                }
            }

            public class PushMessage
            {
                /// <summary>
                /// The unique ID of a message.
                /// </summary>
                public string id;

                /// <summary>
                /// The title of the push message.
                /// </summary>
                public string title;

                /// <summary>
                /// The body of the push message.
                /// </summary>
                public string body;

                /// <summary>
                /// You can check the custom information sent when sending a push in JSON format.
                /// </summary>
                public string extras;

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="jsonString">Extracts object instance from this JSON string.</param>
                /// <returns>Returns an instantiated object.</returns>
                public static PushMessage From(string jsonString)
                {
                    PushMessage pushMessage = JsonMapper.ToObject<PushMessage>(jsonString);
                    return pushMessage;
                }
            }

            public class PushAction
            {
                /// <summary>
                /// Button action type.
                /// </summary>
                public string actionType;

                /// <summary>
                /// PushMessage data.
                /// </summary>
                public PushMessage message;

                /// <summary>
                /// User text typed in Push console.
                /// </summary>
                public string userText;

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="jsonString">Extracts object instance from this JSON string.</param>
                /// <returns>Returns an instantiated object.</returns>
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
            /// This LaunchingInfo object contains settings of the NHN Cloud Gamebase Console and game status.
            /// </summary>
            public class LaunchingInfo
            {
                /// <summary>
                /// Launching information of Gamebase.
                /// </summary>
                public GamebaseLaunching launching;
                
                /// <summary>
                /// Appkey of NHN Cloud Products linked to Gamebase.
                /// </summary>
                public TCProductInfo tcProduct;
                
                /// <summary>
                /// IAP store information registered on NHN Cloud console.
                /// </summary>
                public List<TCIAPInfo> tcIap;

                /// <summary>
                /// Launching Information entered by users through NHN Cloud Launching Console.
                /// Retrieve what user entered from NHN Cloud Console in JSON string format.
                /// Refer to the following guide for detail configuration of NHN Cloud Launching.
                /// <para/><see href="https://docs.toast.com/en/Game/Gamebase/en/oper-management/#config">Gamebase and NHN Cloud integration</see>
                /// </summary>
                public string tcLaunching;

                /// <summary>
                /// Launching information of Gamebase.
                /// </summary>
                public class GamebaseLaunching
                {
                    /// <summary>
                    /// Status information of game app version set in the Gamebase Unity SDK initialization.
                    /// </summary>
                    public LaunchingStatus status;

                    /// <summary>
                    /// User information are registered in the Gamebase Console.
                    /// </summary>
                    public User user;

                    /// <summary>
                    /// App information registered in the NHN Cloud Console.
                    /// </summary>
                    public APP app;

                    /// <summary>
                    /// Maintenance information registered in the NHN Cloud Console is as follows.
                    /// </summary>
                    public Maintenance maintenance;

                    /// <summary>
                    /// Following notices are registered in the Gamebase Console.
                    /// </summary>
                    public LaunchingNotice notice;

                    public class User
                    {
                        /// <summary>
                        /// Test device.
                        /// </summary>
                        public TestDevice testDevice;

                        public class TestDevice
                        {
                            /// <summary>
                            /// Whether the test device matches.
                            /// </summary>
                            public bool matchingFlag;
                            /// <summary>
                            /// matched type. (e.g. deviceKey, ip)
                            /// </summary>
                            public List<string> matchingTypes;
                        }
                    }

                    /// <summary>
                    /// App information registered in the NHN Cloud Console.
                    /// <para/>NHN Cloud Console > Game > Gamebase > App
                    /// </summary>
                    public class APP
                    {
                        /// <summary>
                        /// Information set in the console app.
                        /// </summary>
                        public AccessInfo accessInfo;

                        /// <summary>
                        /// In-app URL to be used within the app.
                        /// </summary>
                        public RelatedURLs relatedUrls;

                        /// <summary>
                        /// App Installation information.
                        /// </summary>
                        public Install install;

                        /// <summary>
                        /// Authentication information.
                        /// <para/>NHN Cloud Console > Game > Gamebase > App > Authentication Information
                        /// </summary>
                        public Dictionary<string, LaunchingIDPInfo> idP;

                        /// <summary>
                        /// Gamebase environment. (e.g. REAL or SANDBOX)
                        /// </summary>
                        public string typeCode;

                        /// <summary>
                        /// The login URL to use within the app.
                        /// </summary>
                        public LoginUrls loginUrls;

                        /// <summary>
                        /// Customer service information.
                        /// </summary>
                        public CustomerService customerService;
                        
                        public class AccessInfo
                        {
                            /// <summary>
                            /// Server URL.
                            /// <para/>NHN Cloud Console > Game > Gamebase > App > Server URL
                            /// </summary>
                            public string serverAddress;
                        }

                        public class RelatedURLs
                        {
                            /// <summary>
                            /// Terms of Use.
                            /// <para/>NHN Cloud Console > Game > Gamebase > App > InApp URL > Terms of Use
                            /// </summary>
                            public string termsUrl;

                            /// <summary>
                            /// Punishment Provision.
                            /// <para/>NHN Cloud Console > Game > Gamebase > App > InApp URL > Punishment Provision
                            /// </summary>
                            public string punishRuleUrl;

                            /// <summary>
                            /// Personal Info Agreement.
                            /// <para/>NHN Cloud Console > Game > Gamebase > App > InApp URL > Personal Info Agreement
                            /// </summary>
                            public string personalInfoCollectionUrl;
                        }

                        public class Install
                        {
                            /// <summary>
                            /// Install URL.
                            /// <para/>NHN Cloud Console > Game > Gamebase > App > Install URL
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
                            /// <para/>NHN Cloud Console > Game > Gamebase > App > Authentication Information > Client ID
                            /// </summary>
                            public string clientId;

                            /// <summary>
                            /// Secret Key.
                            /// <para/>NHN Cloud Console > Game > Gamebase > App > Authentication Information > Secret Key
                            /// </summary>
                            public string clientSecret;

                            /// <summary>
                            /// Additional Info.
                            /// <para/>NHN Cloud Console > Game > Gamebase > App > Authentication Information > Additional Info 
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
                    /// Maintenance information registered in the NHN Cloud Console.
                    /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Maintenance > Maintenance Register
                    /// </summary>
                    public class Maintenance
                    {
                        /// <summary>
                        /// Detailed maintenance URL.
                        /// Displayed when 'External link' type is selected in the Maintenance Page.
                        /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > URL
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
                        /// <para/>DEFAULT_HTML: The html registered in the NHN Cloud Console.
                        /// <para/>URL: The external url.
                        /// <para/>URL_PARAM: The external url with maintenance information parameter.
                        /// </summary>
                        public string pageTypeCode;

                        /// <summary>
                        /// Provide reasons for maintenance simply.
                        /// This message is not shown to game users.
                        /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Reason
                        /// </summary>
                        public string reason;

                        /// <summary>
                        /// Maintenance message.
                        /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Message
                        /// </summary>
                        public string message;

                        /// <summary>
                        /// Standard timezone.
                        /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Time > Timezone
                        /// </summary>
                        public string timezone;

                        /// <summary>
                        /// Start time of maintenance. (ISO 8601)
                        /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Time > Start Date
                        /// </summary>
                        public string beginDate;

                        /// <summary>
                        /// End time of maintenance. (ISO 8601)
                        /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Maintenance > Maintenance Register > Time > End Date
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

                        /// <summary>
                        /// Display Date/Time on maintenance page.
                        /// </summary>
                        public bool hideDate;
                    }

                    /// <summary>
                    /// Notice information registered in the NHN Cloud Console.
                    /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Notice > Notice Register
                    /// </summary>
                    public class LaunchingNotice
                    {
                        /// <summary>
                        /// Detailed message.
                        /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Notice > Notice Register > Message
                        /// </summary>
                        public string message;

                        /// <summary>
                        /// Title of notice.
                        /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Notice > Notice Register > Title
                        /// </summary>
                        public string title;

                        /// <summary>
                        /// The url of a page.
                        /// Displayed when 'Close + More' is selected in the Bottom button type.
                        /// <para/>NHN Cloud Console > Game > Gamebase > Operation > Notice > Notice Register > Bottom button type
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
                /// IAP store information registered in the NHN Cloud Console.
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
                    /// Check the <see cref="GamebaseStoreCode"/> class for store code types.
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
                /// <summary>
                /// Market URL to update the application.
                /// </summary>
                public string installUrl;

                /// <summary>
                /// This message indicates to the user that an update is needed.
                /// </summary>
                public string message;

                /// <summary>
                /// This url is displayed in the web view when the 'Show Details' button is clicked.
                /// </summary>
                public string detailUrl;

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
            /// <summary>
            /// The AuthToken represents an authenticated token and user information for using services.
            /// </summary>
            public class AuthToken
            {
                /// <summary>
                /// Token information
                /// </summary>
                public Token token;
                
                /// <summary>
                /// User information
                /// </summary>
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
                /// <para/>NHN Cloud Console > Game > Gamebase > Ban > Register Ban > Message
                /// </summary>
                public string message;

                /// <summary>
                /// Customer center information. (e.g. Customer center email or contact)
                /// <para/>NHN Cloud Console > Game > Gamebase > App > Customer center information
                /// </summary>
                public string csInfo;

                /// <summary>
                /// Service center. (e.g. The URL of the customer center web page.)
                /// <para/>NHN Cloud Console > Game > Gamebase > App > InApp URL > Service center
                /// </summary>
                public string csUrl;

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="error">Extracts object instance from this error.</param>
                /// <returns>Returns an instantiated object.</returns>
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
                    /// <summary>
                    /// Issued ID.
                    /// </summary>
                    public string id;

                    /// <summary>
                    /// Issued password.
                    /// </summary>
                    public string password;
                }

                public class Condition
                {
                    /// <summary>
                    /// Issued transfer account type.
                    /// </summary>
                    public string transferAccountType;

                    /// <summary>
                    /// Account expiration type.
                    /// </summary>
                    public string expirationType;

                    /// <summary>
                    /// Account expiration date.
                    /// </summary>
                    public long expirationDate;
                }

                /// <summary>
                /// Issued type.
                /// </summary>
                public string issuedType;

                /// <summary>
                /// Issued account information.
                /// </summary>
                public Account account;

                /// <summary>
                /// Issued account condition.
                /// </summary>
                public Condition condition;
            }

            public class ForcingMappingTicket
            {
                /// <summary>
                /// User ID.
                /// </summary>
                public string userId;
                
                /// <summary>
                /// User ID that can be cleared by force mapping.
                /// </summary>
                public string mappedUserId;

                /// <summary>
                /// The status of mapped user.
                /// Use this value if you want to restrict addMapping according to user status.
                /// 
                /// <para/><see href="https://docs.toast.com/ko/Game/Gamebase/ko/api-guide/#member-valid-code"/>
                /// In ForcingMappingTicket, mappedUserValid cannot be 'D' or 'M'.
                /// 
                /// - Y : Normal user. ('Y'es)
                /// - D : Withdrawn user. ('D'eleted)
                /// - B : 'B'anned user.
                /// - T : Withdrawal-suspended user. ('T'emporaryWithdrawn)
                /// - P : Ban-suspended user. ('P'ostpone)
                /// - M : 'M'issing account
                /// </summary>
                public string mappedUserValid;

                /// <summary>
                /// IdP code passed when calling addMapping.
                /// </summary>
                public string idPCode;

                /// <summary>
                /// Issued forced mapping key.
                /// </summary>
                public string forcingMappingKey;

                /// <summary>
                /// Expiration date of issued ticket.
                /// </summary>
                public long expirationDate;

                /// <summary>
                /// Gamebase accessToken issued to the idP passed when calling addMapping.
                /// </summary>
                public string accessToken;
                
                [Obsolete("As of release 2.9.0, use GamebaseResponse.Auth.ForcingMappingTicket.From instead.")]
                public static ForcingMappingTicket MakeForcingMappingTicket(GamebaseError error)
                {
                    return GetForcingMappingTicket(error);
                }

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="error">Extracts object instance from this error.</param>
                /// <returns>Returns an instantiated object.</returns>
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
                /// <summary>
                /// App ID.
                /// </summary>
                public string appId = string.Empty;

                /// <summary>
                /// Blocked user ID.
                /// </summary>
                public string id = string.Empty;

                /// <summary>
                /// Value provided only if blocked. (always the value is 'B')
                /// </summary>
                public string status = string.Empty;

                /// <summary>
                /// Number of failures.
                /// </summary>
                public int failCount = 0;

                /// <summary>
                /// Block end date.
                /// </summary>
                public long blockEndDate = 0;

                /// <summary>
                /// Block registration date.
                /// </summary>
                public long regDate = 0;

                [Obsolete("As of release 2.9.0, use GamebaseResponse.Auth.TransferAccountFailInfo.From instead.")]
                public static TransferAccountFailInfo MakeTransferAccountFailInfo(GamebaseError error)
                {
                    return GetTransferAccountFailInfo(error);
                }

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="error">Extracts object instance from this error.</param>
                /// <returns>Returns an instantiated object.</returns>
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

                /// <summary>
                /// Store code.
                /// Check the <see cref="GamebaseStoreCode"/> class for store code types.
                /// </summary>
                public string storeCode;
                
                /// <summary>
                /// Whether promotion purchase
                /// </summary>
                public bool isPromotion;
                
                /// <summary>
                /// Whether test purchase
                /// </summary>
                public bool isTestPurchase;
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

            public class PurchasableSubscriptionStatus
            {
                /// <summary>
                /// The code defined internally by Gamebase for the store where the app is installed.
                /// </summary>
                public string storeCode;

                /// <summary>
                /// The payment identifier of a store.
                /// </summary>
                public string paymentId;

                /// <summary>
                /// PaymentId is changed whenever product subscription is renewed.
                /// This field shows the paymentId that was used when a subscription product was first purchased.
                /// This value does not guarantee to be always valid, as it can have no value depending on the store
                /// depending on the store from which the user made a purchase and the status of the payment server.
                /// </summary>
                public string originalPaymentId;

                /// <summary>
                /// Payment unique identifier.
                /// </summary>
                public string paymentSeq;

                /// <summary>
                /// The product ID of a purchased item.
                /// </summary>
                public string marketItemId;

                /// <summary>
                /// Item unique identifier in IAP web console.
                /// </summary>
                public long itemSeq;

                /// <summary>
                /// The product type which can have the following values:
                /// * UNKNOWN: An unknown type. Either update Gamebase SDK or contact Gamebase Customer Center.
                /// * CONSUMABLE: A consumable product.
                /// * AUTO_RENEWABLE: A subscription product.
                /// </summary>
                public string productType;

                /// <summary>
                /// This is a user ID with which a product is purchased.
                /// If a user logs in with a user ID that is not used to purchase a product, the user cannot obtain the product they purchased.
                /// </summary>
                public string userId;

                /// <summary>
                /// Price of product.
                /// </summary>
                public float price;

                /// <summary>
                /// Currency information.
                /// </summary>
                public string currency;

                /// <summary>
                /// Payment identifier.
                /// This is an important piece of information used to call 'Consume' server API with paymentSeq.
                /// In Consume API, the parameter must be named 'accessToken' to be passed.
                ///
                /// Consume API : https://docs.toast.com/en/Game/Gamebase/en/api-guide/#purchase-iap
                /// </summary>
                public string purchaseToken;

                /// <summary>
                /// This value is used when making a purchase on Google, which can have the following values.
                /// However, if the verification logic is temporarily disabled by Gamebase payment server due to error on Google server,
                /// it returns only null, so please remember that it does not guarantee a valid return value at all times.
                /// * null : Normal payment
                /// * Test : Test payment
                /// * Promo : Promotion payment
                /// </summary>
                public string purchaseType;

                /// <summary>
                /// The time when the product was purchased.(epoch time)
                /// </summary>
                public long purchaseTime;

                /// <summary>
                /// The time when the subscription expires.(epoch time)
                /// </summary>
                public long expiryTime;

                /// <summary>
                /// It is the value passed to payload when calling Gamebase.Purchase.requestPurchase API.
                ///
                /// This field can be used to hold a variety of additional information.
                /// For example, this field can be used to separately handle purchase
                /// and provision of the products purchased using the same user ID and sort them by game channel or character.
                /// </summary>
                public string payload;

                /// <summary>
                /// Subscription status.
                /// Refer to the following document for the entire status code.
                /// https://docs.nhncloud.com/en/TOAST/en/toast-sdk/iap-unity/#iapsubscriptionstatusstatus
                /// </summary>
                public int statusCode;

                /// <summary>
                /// Description of subscription status.
                /// </summary>
                public string statusDescription;

                /// <summary>
                /// The product ID that is registered with the Gamebase console.
                /// This is used when a product is purchased using Gamebase.Purchase.requestPurchase API.
                /// </summary>
                public string gamebaseProductId;
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
                /// Android only
                /// This field automatically calls the requestPermission("android.permission.POST_NOTIFICATIONS") after a successful registerPush call on Android 13 or higher OS.
                /// </summary>
                public bool requestNotificationPermission = true;
                
                /// <summary>
                /// iOS only
                /// If set to `true', the token will be registered even if you don't have permission to grant notifications. The default value is `false`.
                /// </summary>
                public bool alwaysAllowTokenRegistration = false;
                
                /// <summary>
                /// The display language on the push notification UI.
                /// </summary>
                public string displayLanguageCode = null;

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="dataContainer">Extracts object instance from this data container.</param>
                /// <returns>Returns an instantiated object.</returns>
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

            public class ShowTermsViewResult
            {
                /// <summary>
                /// This field indicates whether the user has agreed to the Terms and Conditions agreement popup displayed.
                /// </summary>
                public bool isTermsUIOpened;
                
                /// <summary>
                /// This field allows you to check the PushConfiguraion settings as a result of agreeing to the terms view.
                /// If this field is not null, call Gamebase.Push.RegisterPush(PushConfiguration, GamebaseCallback).
                /// </summary>
                public Push.PushConfiguration pushConfiguration;

                /// <summary>
                /// A factory method that instantiates an object.
                /// </summary>
                /// <param name="dataContainer">Extracts object instance from this data container.</param>
                /// <returns>Returns an instantiated object.</returns>
                public static ShowTermsViewResult From(DataContainer dataContainer)
                {
                    return (dataContainer != null) ? From(dataContainer.data) : null;
                }
                
                private static ShowTermsViewResult From(string jsonString)
                {
                    if (string.IsNullOrEmpty(jsonString) == true)
                    {
                        return null;
                    }
 
                    return JsonMapper.ToObject<ShowTermsViewResult>(jsonString);
                }

            }
        }
    }    
}