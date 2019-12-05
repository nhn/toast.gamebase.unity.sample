//
//  TCGBConstants.h
//  Gamebase
//
//  Created by NHN on 2016. 5. 25..
//  © NHN Corp. All rights reserved.
//

#ifndef TCGBConstants_h
#define TCGBConstants_h

#define TCGB_SUCCESS   nil

#pragma mark - TCGBAppDelegate
extern NSString* const kTCGBAppDelegateNotificationDidBecomeActive;
extern NSString* const KTCGBAppDelegateNotificationWillFinishLaunchingWithOptions;
extern NSString* const kTCGBAppDelegateNotificationWillEnterForeground;
extern NSString* const kTCGBAppDelegateNotificationWillResignActive;
extern NSString* const kTCGBAppDelegateNotificationDidEnterBackground;
extern NSString* const kTCGBAppDelegateNotificationWillTerminate;
extern NSString* const kTCGBAppDelegateNotificationOpenURL;


#pragma mark - TCGBAuthIDPs
extern NSString* const kTCGBAuthGuest;
extern NSString* const kTCGBAuthPayco;
extern NSString* const kTCGBAuthFacebook;
extern NSString* const kTCGBAuthiOSGameCenter;
extern NSString* const kTCGBAuthNaver;
extern NSString* const kTCGBAuthTwitter;
extern NSString* const kTCGBAuthGoogle;
extern NSString* const kTCGBAuthLine;
extern NSString* const kTCGBAuthAppleID;


#pragma mark - TCGBAuthAdditionalInfos
extern NSString* const kTCGBAuthAdditionalInfoFacebookAccessToken;
extern NSString* const kTCGBAuthConsoleAuthKeyname;


#pragma mark - Credential Information for loginWithCredential
extern NSString* const kTCGBAuthLoginWithCredentialProviderNameKeyname;
extern NSString* const kTCGBAuthLoginWithCredentialAccessTokenKeyname;
extern NSString* const kTCGBAuthLoginWithCredentialAccessTokenSecretKeyname;
extern NSString* const kTCGBAuthLoginWithCredentialRefreshTokenKeyname;
extern NSString* const kTCGBAuthLoginWithCredentialAuthExtraKeyname;


#pragma mark - WebView Event Notification
extern NSString* const kTCGBWebViewControllerNotificationClose;

#pragma mark - DisplayLanguageCode
extern NSString* const kTCGBDisplayLanguageCodeGerman;
extern NSString* const kTCGBDisplayLanguageCodeEnglish;
extern NSString* const kTCGBDisplayLanguageCodeSpanish;
extern NSString* const kTCGBDisplayLanguageCodeFinish;
extern NSString* const kTCGBDisplayLanguageCodeFrench;
extern NSString* const kTCGBDisplayLanguageCodeIndonesian;
extern NSString* const kTCGBDisplayLanguageCodeItalian;
extern NSString* const kTCGBDisplayLanguageCodeJapanese;
extern NSString* const kTCGBDisplayLanguageCodeKorean;
extern NSString* const kTCGBDisplayLanguageCodePortuguese;
extern NSString* const kTCGBDisplayLanguageCodeRussian;
extern NSString* const kTCGBDisplayLanguageCodeThai;
extern NSString* const kTCGBDisplayLanguageCodeVietnamese;
extern NSString* const kTCGBDisplayLanguageCodeMalay;
extern NSString* const kTCGBDisplayLanguageCodeChineseSimplified;
extern NSString* const kTCGBDisplayLanguageCodeChineseTraditional;

#pragma mark - Server Push Types
extern NSString* const kTCGBServerPushNotificationTypeAppKickout;       // Kickout by TOAST Console

#pragma mark - Observer Message Types
extern NSString* const kTCGBObserverMessageTypeNetwork;                 // Network Monitoring (NotReachable, ReachableViaWifi, ReachableViaWWAN);
extern NSString* const kTCGBObserverMessageTypeLaunching;               // Launching Status (TCGBLaunchingStatus)
extern NSString* const kTCGBObserverMessageTypeHeartbeat;               // Heartbeat (Banned User: TCGB_ERROR_BANNED_MEMBER)


#pragma mark - TCGBError Codes
typedef NS_ENUM(NSInteger, TCGBErrorCode) {
    // Common
    TCGB_ERROR_NOT_INITIALIZED                           = 1,
    TCGB_ERROR_NOT_LOGGED_IN                             = 2,
    TCGB_ERROR_INVALID_PARAMETER                         = 3,
    TCGB_ERROR_INVALID_JSON_FORMAT                       = 4,
    TCGB_ERROR_USER_PERMISSION                           = 5,
    TCGB_ERROR_INVALID_MEMBER                            = 6,
    TCGB_ERROR_BANNED_MEMBER                             = 7,
    TCGB_ERROR_SAME_REQUESTOR                            = 8,
    TCGB_ERROR_NOT_GUEST_OR_HAS_OTHERS                   = 9,
    
    TCGB_ERROR_NOT_SUPPORTED                             = 10,
    TCGB_ERROR_NOT_SUPPORTED_ANDROID                     = 11,              // <UNITY ONLY>
    TCGB_ERROR_NOT_SUPPORTED_IOS                         = 12,              // <UNITY ONLY>
    TCGB_ERROR_NOT_SUPPORTED_UNITY_EDITOR                = 13,              // <UNITY ONLY>
    TCGB_ERROR_NOT_SUPPORTED_UNITY_STANDALONE            = 14,              // <UNITY ONLY>
    TCGB_ERROR_NOT_SUPPORTED_UNITY_WEBGL                 = 15,              // <UNITY ONLY>
    TCGB_ERROR_ANDROID_ACTIVITY_DESTROYED                = 31,              // <ANDROID ONLY>
    TCGB_ERROR_ANDROID_ACTIVEAPP_NOT_CALLED              = 32,              // <ANDROID ONLY>
    TCGB_ERROR_IOS_GAMECENTER_DENIED                     = 51,              // <IOS ONLY>
    
    // Network (Socket)
    TCGB_ERROR_SOCKET_RESPONSE_TIMEOUT                   = 101,
    
    TCGB_ERROR_SOCKET_ERROR                              = 110,
    
    // Network (Socket)
    TCGB_ERROR_UNKNOWN_ERROR                             = 999,
    
    // Launching
    TCGB_ERROR_LAUNCHING_SERVER_ERROR                    = 2001,
    TCGB_ERROR_LAUNCHING_NOT_EXIST_CLIENT_ID             = 2002,
    TCGB_ERROR_LAUNCHING_UNREGISTERED_APP                = 2003,
    TCGB_ERROR_LAUNCHING_UNREGISTERED_CLIENT             = 2004,

    // Auth
    // common
    TCGB_ERROR_AUTH_USER_CANCELED                        = 3001,
    TCGB_ERROR_AUTH_NOT_SUPPORTED_PROVIDER               = 3002,
    TCGB_ERROR_AUTH_NOT_EXIST_MEMBER                     = 3003,
    
    TCGB_ERROR_AUTH_EXTERNAL_LIBRARY_ERROR               = 3009,
    TCGB_ERROR_AUTH_ALREADY_IN_PROGRESS_ERROR            = 3010,
    
    // TransferAccount
    TCGB_ERROR_AUTH_TRANSFERACCOUNT_EXPIRED              = 3041,
    TCGB_ERROR_AUTH_TRANSFERACCOUNT_BLOCK                = 3042,
    TCGB_ERORR_AUTH_TRANSFERACCOUNT_INVALID_ID           = 3043,
    TCGB_ERORR_AUTH_TRANSFERACCOUNT_INVALID_PASSWORD     = 3044,
    TCGB_ERROR_AUTH_TRANSFERACCOUNT_CONSOLE_NO_CONDITION = 3045,
    TCGB_ERROR_AUTH_TRANSFERACCOUNT_NOT_EXIST            = 3046,
    TCGB_ERROR_AUTH_TRANSFERACCOUNT_ALREADY_EXIST_ID     = 3047,
    TCGB_ERROR_AUTH_TRANSFERACCOUNT_ALREADY_USED         = 3048,
    
    // tokenLogin
    TCGB_ERROR_AUTH_TOKEN_LOGIN_FAILED                     = 3101,
    TCGB_ERROR_AUTH_TOKEN_LOGIN_INVALID_TOKEN_INFO         = 3102,
    TCGB_ERROR_AUTH_TOKEN_LOGIN_INVALID_LAST_LOGGED_IN_IDP = 3103,
    // idPLogin
    TCGB_ERROR_AUTH_IDP_LOGIN_FAILED                     = 3201,
    TCGB_ERROR_AUTH_IDP_LOGIN_INVALID_IDP_INFO           = 3202,
    // addMapping
    TCGB_ERROR_AUTH_ADD_MAPPING_FAILED                         = 3301,
    TCGB_ERROR_AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER = 3302,
    TCGB_ERROR_AUTH_ADD_MAPPING_ALREADY_HAS_SAME_IDP           = 3303,
    TCGB_ERROR_AUTH_ADD_MAPPING_INVALID_IDP_INFO               = 3304,
    TCGB_ERROR_AUTH_ADD_MAPPING_CANNOT_ADD_GUEST_IDP           = 3305,
    // forceMapping
    TCGB_ERROR_AUTH_ADD_MAPPING_FORCIBLY_NOT_EXIST_KEY     = 3311,
    TCGB_ERROR_AUTH_ADD_MAPPING_FORCIBLY_ALREADY_USED_KEY  = 3312,
    TCGB_ERROR_AUTH_ADD_MAPPING_FORCIBLY_EXPIRED_KEY       = 3313,
    TCGB_ERROR_AUTH_ADD_MAPPING_FORCIBLY_DIFFERENT_IDP     = 3314,
    TCGB_ERROR_AUTH_ADD_MAPPING_FORCIBLY_DIFFERENT_AUTHKEY = 3315,
    // removeMapping
    TCGB_ERROR_AUTH_REMOVE_MAPPING_FAILED                = 3401,
    TCGB_ERROR_AUTH_REMOVE_MAPPING_LAST_MAPPED_IDP       = 3402,
    TCGB_ERROR_AUTH_REMOVE_MAPPING_LOGGED_IN_IDP         = 3403,
    // logout
    TCGB_ERROR_AUTH_LOGOUT_FAILED                        = 3501,
    // withdraw
    TCGB_ERROR_AUTH_WITHDRAW_FAILED                      = 3601,
    // status not playable
    TCGB_ERROR_AUTH_NOT_PLAYABLE                         = 3701,            // 상태 별 로직은 launchingStatus를 확인하여 처리
    // unknown
    TCGB_ERROR_AUTH_UNKNOWN_ERROR                        = 3999,
    
    // Purchase
    TCGB_ERROR_PURCHASE_NOT_INITIALIZED                  = 4001,
    TCGB_ERROR_PURCHASE_USER_CANCELED                    = 4002,
    TCGB_ERROR_PURCHASE_NOT_FINISHED_PREVIOUS_PURCHASING = 4003,
    TCGB_ERROR_PURCHASE_NOT_ENOUGH_CASH                  = 4004,
    
    TCGB_ERROR_PURCHASE_NOT_SUPPORTED_MARKET             = 4010,
    
    TCGB_ERROR_PURCHASE_EXTERNAL_LIBRARY_ERROR           = 4201,
    
    TCGB_ERROR_PURCHASE_UNKNOWN_ERROR                    = 4999,
    
    // Push
    TCGB_ERROR_PUSH_EXTERNAL_LIBRARY_ERROR               = 5101,
    TCGB_ERROR_PUSH_ALREADY_IN_PROGRESS_ERROR            = 5102,
    
    TCGB_ERROR_PUSH_UNKNOWN_ERROR                        = 5999,
    
    // UI
    TCGB_ERROR_UI_UNKNOWN_ERROR                          = 6999,
    
    // WebView
    TCGB_ERROR_WEBVIEW_INVALID_URL                       = 7001,
    TCGB_ERROR_WEBVIEW_UNKNOWN_ERROR                     = 7999,              // <UNITY ONLY>
    
    // Server
    TCGB_ERROR_SERVER_INTERNAL_ERROR                     = 8001,
    TCGB_ERROR_SERVER_REMOTE_SYSTEM_ERROR                = 8002,
    
    TCGB_ERROR_SERVER_UNKNOWN_ERROR                      = 8999,
    
    // Platform specified errors
    TCGB_ERROR_INVALID_INTERNAL_STATE                    = 11001,
    TCGB_ERROR_NOT_CALLABLE_STATE                        = 11002,
};

typedef NS_ENUM(NSInteger, NetworkStatus) {
    ReachabilityIsNotDefined = -100,
    NotReachable = -1,
    ReachableViaWWAN,
    ReachableViaWifi,
};

typedef NS_ENUM(NSInteger, GamebaseToastLength) {
    GamebaseToastLengthShort = 0,
    GamebaseToastLengthLong = 1,
};

#endif /* TCGBConstants_h */
