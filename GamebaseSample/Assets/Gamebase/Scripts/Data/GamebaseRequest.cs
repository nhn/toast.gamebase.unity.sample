using System;
using System.Collections.Generic;
using Toast.Gamebase.Internal;

namespace Toast.Gamebase
{
    public static class GamebaseRequest
    {
        /// <summary>
        /// When initializing Gamebase, you can change Gamebase settings using the GamebaseConfiguration object.
        /// </summary>
        public class GamebaseConfiguration
        {
            /// <summary>
            /// Project ID registered in TOAST.
            /// </summary>
            public string   appID;

            /// <summary>
            /// Client version registered in TOAST.
            /// </summary>
            public string   appVersion;

            [Obsolete("Do not use this member variable.")]
            public string   zoneType;

            /// <summary>
            /// The display language on the Gamebase UI and SystemDialog can be changed into another language, which is not set on a device, as the user wants.
            /// Gamebase displays messages which are included in a client or as received by a server.
            /// With DisplayLanguage, messages are displayed in an appropriate language for the language code (ISO-639) set by the user.
            /// Each language code is defined in <see cref="GamebaseDisplayLanguageCode"/>.
            /// </summary>
            public string   displayLanguageCode;

            /// <summary>
            /// When a game user cannot play games due to system maintenance or banned from use, reasons need to be displayed by pop-ups.
            /// This setting regards to applying default pop-ups provided by Gamebase SDK.
            /// <para/>True: Pop-ups are exposed depending on the setting of enableLaunchingStatusPopup and enableBanPopup.
            /// <para/>False: All pop-ups provided by Gamebase are not exposed.
            /// <para/>Default: false
            /// </summary>
            public bool enablePopup = false;

            /// <summary>
            /// This setting regards to applying default pop-ups provided by Gamebase, when the LaunchingStatus is disabled to play games.
            /// <para/>Default: true
            /// </summary>
            public bool enableLaunchingStatusPopup = true;

            /// <summary>
            /// This setting regards to applying default pop-ups provided by Gamebase, when the game user has been banned.
            /// <para/>Default: true
            /// </summary>
            public bool enableBanPopup = true;

            /// <summary>
            /// This setting regards to applying default pop-ups provided by Gamebase, when the game user has been kicked out.
            /// <para/>Default: true
            /// </summary>
            public bool enableKickoutPopup = true;

            /// <summary>
            /// Store information required to initialize In-App Purchase (IAP) of TOAST.
            /// <para/>App Store    | AS | only iOS
            /// <para/>Google Play  | GG | only Android
            /// <para/>One Store    | TS | only Android
            /// </summary>
            public string storeCode;

            /// <summary>
            /// Refer to the following setup guide for FCM push notification.
            /// <para/><see href="https://docs.toast.com/en/Game/Gamebase/en/upgrade-guide/#firebase-push">Firebase Push</see>
            /// </summary>
            [Obsolete("Do not use this member variable.")]
            public string fcmSenderId;

            /// <summary>
            /// Unity Standalone only
            /// Set whether or not to log in to WebView on a (Standalone) platform.
            /// </summary>
            public bool useWebViewLogin;
        }        

        public static class Auth
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
                    TransferAccountRenewConfiguration renewTransferAccount = new TransferAccountRenewConfiguration
                    {
                        renewalModeType = RENEWAL_MODE_MANUAL,
                        renewalTargetType = RenewalTargetType.ID_PASSWORD,
                        accountId = accountId,
                        accountPassword = accountPassword
                    };

                    return renewTransferAccount;
                }

                public static TransferAccountRenewConfiguration MakeManualRenewConfiguration(string accountPassword)
                {
                    TransferAccountRenewConfiguration renewTransferAccount = new TransferAccountRenewConfiguration
                    {
                        renewalModeType = RENEWAL_MODE_MANUAL,
                        renewalTargetType = RenewalTargetType.PASSWORD,
                        accountPassword = accountPassword
                    };

                    return renewTransferAccount;
                }

                public static TransferAccountRenewConfiguration MakeAutoRenewConfiguration(RenewalTargetType type)
                {
                    TransferAccountRenewConfiguration renewTransferAccount = new TransferAccountRenewConfiguration
                    {
                        renewalModeType = RENEWAL_MODE_AUTO,
                        renewalTargetType = type
                    };

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

        public static class Push
        {
            /// <summary>
            /// Parameter class for the initialization of the <see cref="Gamebase.Push.RegisterPush"/> API
            /// </summary>
            public class PushConfiguration
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

                /// <summary>
                /// The display language on the push notification UI.
                /// </summary>
                public string displayLanguageCode = string.Empty;
            }

            public class NotificationOptions
            {
                /// <summary>
                /// Whether to receive notifications when the application is foreground.
                /// <para/>Default: false
                /// </summary>
                public bool foregroundEnabled = false;

                /// <summary>
                /// Whether to use the badge icon.
                /// <para/>Default: true
                /// </summary>
                public bool badgeEnabled = true;

                /// <summary>
                /// iOS only
                /// Whether to use notification sound.
                /// <para/>Default: true
                /// </summary>
                public bool soundEnabled = true;

                /// <summary>
                /// Android only
                /// Notification priority.
                /// Priority is defined in <see cref="GamebaseNotificationPriority"/>.
                /// <para/>Default: GamebaseNotificationPriority.HIGH
                /// </summary>
                public int priority = GamebaseNotificationPriority.HIGH;

                /// <summary>
                /// Android only
                /// The name of the small icon resource, which will be used to represent the notification in the status bar.
                /// <para/>Default: string.Empty (Application icon)
                /// </summary>
                public string smallIconName = string.Empty;

                /// <summary>
                /// Android only: This is deprecated in API 26(Android 8.0 Oreo) but you can still use for below 26.
                /// If you specify the file name of mp3 or wav extension in the 'res/raw' folder, the notification sound changes.
                /// <para/>Default: string.Empty
                /// </summary>
                public string soundFileName = string.Empty;

                public NotificationOptions()
                {
                }

                public NotificationOptions(GamebaseResponse.Push.NotificationOptions options)
                {
                    foregroundEnabled = options.foregroundEnabled;
                    badgeEnabled = options.badgeEnabled;
                    soundEnabled = options.soundEnabled;
                    priority = options.priority;
                    smallIconName = options.smallIconName;
                    soundFileName = options.soundFileName;
                }
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
        }

        public static class Webview
        {
            /// <summary>
            /// Changes WebView layout by using GamebaseWebViewConfiguration.
            /// </summary>
            public class GamebaseWebViewConfiguration
            {
                /// <summary>
                /// Title of WebView
                /// </summary>
                public string title;

                /// <summary>
                /// Orientation is defined in <see cref="GamebaseScreenOrientation"/>.
                /// </summary>
                public int orientation;

                /// <summary>
                /// Color of Navigation Bar: Red
                /// </summary>
                public int colorR;

                /// <summary>
                /// Color of Navigation Bar: Green
                /// </summary>
                public int colorG;

                /// <summary>
                /// Color of Navigation Bar: Blue
                /// </summary>
                public int colorB;

                /// <summary>
                /// Color of Navigation Bar: Alpha
                /// </summary>
                public int colorA;

                /// <summary>
                /// Height of Navigation Bar
                /// </summary>
                public int barHeight;

                /// <summary>
                /// Activate/Deactivate Go Back Button
                /// </summary>
                public bool isBackButtonVisible;

                /// <summary>
                /// Image of Go Back Button
                /// </summary>
                public string backButtonImageResource;

                /// <summary>
                /// Image of Close Button
                /// </summary>
                public string closeButtonImageResource;

                /// <summary>
                /// iOS only
                /// Configure webview's content mode.
                /// ContentMode is defined in <see cref="GamebaseWebViewContentMode"/>.
                /// <para/>Default: GamebaseWebViewContentMode.RECOMMENDED
                /// </summary>
                public int contentMode = GamebaseWebViewContentMode.RECOMMENDED;
            }
        }

        public static class Analytics
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

        public static class Logger
        {
            /// <summary>
            /// The configuration used to initialize the ToastLogger SDK.
            /// </summary>
            public class Configuration
            {
                /// <summary>
                /// Appkey issued by Log &amp; Crash.
                /// </summary>
                public string appKey;

                /// <summary>
                /// Whether to enable crash logs.
                /// Crash logs are automatically sent by SDK when enableCrashReporter is true.
                /// <para/>Default: true
                /// </summary>
                public bool enableCrashReporter = true;

                /// <summary>
                /// Error log is excluded by default. Use it if you want to collect error logs.
                /// <para/>Default: false
                /// </summary>
                public bool enableCrashErrorLog = false;

                /// <summary>
                /// TOAST Cloud service zone (e.g. REAL or ALPHA)
                /// <para/>Default: REAL
                /// </summary>
                public string serviceZone = "REAL";

                public Configuration(string appKey)
                {
                    this.appKey = appKey;
                }
            }
        }

        public static class ImageNotice
        {
            public class Configuration
            {
                /// <summary>
                /// Color of Navigation Bar: Red
                /// </summary>
                public int colorR;

                /// <summary>
                /// Color of Navigation Bar: Green
                /// </summary>
                public int colorG;

                /// <summary>
                /// Color of Navigation Bar: Blue
                /// </summary>
                public int colorB;

                /// <summary>
                /// Color of Navigation Bar: Alpha
                /// <para/>Default: 128
                /// </summary>
                public int colorA = 128;

                /// <summary>
                /// Timeout.
                /// <para/>Default: 5000
                /// </summary>
                public long timeout = 5000;
            }
        }

        public static class Contact
        {
            public class Configuration
            {
                /// <summary>
                /// User's name used in the game.
                /// </summary>
                public string userName;

                /// <summary>
                /// Additional data you want to send to the server.
                /// </summary>
                public Dictionary<string, string> extraData;

                /// <summary>
                /// Data to be added when calling the customer center URL.
                /// </summary>
                public string additionalURL;
            }
        }

        public static class Terms
        {
            public class Content
            {
                /// <summary>
                /// Unique identifier for terms and conditions.
                /// </summary>
                public int termsContentSeq;

                /// <summary>
                /// Whether you have agreed to the terms of choice
                /// </summary>
                public bool agreed;
            }

            public class UpdateTermsConfiguration
            {
                /// <summary>
                /// Unique identifier for terms and conditions.
                /// You must pass the received value by calling queryTerms API.
                /// </summary>
                public int termsSeq;

                /// <summary>
                /// Version of the Terms
                /// You must pass the received value by calling queryTerms API.
                /// </summary>
                public string termsVersion;

                /// <summary>
                /// Information that the user agrees to the terms of choice
                /// </summary>
                public List<Content> contents;
            }
        }
    }    
}