namespace Toast.Gamebase.Internal
{
    public static class GamebaseStrings
    {
        //----------------------------------------
        //  Common
        //----------------------------------------
        public const string NOT_FOUND_GAMEOBJECT                                = "The GameObject for Gamebase Unity SDK not found.";
        public const string NOT_INITIALIZED                                     = "The Gamebase SDK must be initialized";
        public const string NOT_LOGGED_IN                                       = "Call this API after log in";
        public const string INVALID_PARAMETER                                   = "Invalid parameter";
        public const string INVALID_JSON_FORMAT                                 = "Invalid JSON format";
        public const string USER_PERMISSION                                     = "Permission error";
        public const string INVALID_MEMBER                                      = "Invalid member";
        public const string BANNED_MEMBER                                       = "Banned member";
        public const string SAME_REQUESTOR                                      = "The issued TransferKey was used on a same device";
        public const string NOT_GUEST_OR_HAS_OTHERS                             = "Transfer was tried on a non-Guest account or the account is interfaced with a non-Guest IdP";
        public const string NOT_SUPPORTED                                       = "Not supported";
        public const string NOT_SUPPORTED_ANDROID                               = "Not supported on Android";
        public const string NOT_SUPPORTED_IOS                                   = "Not supported on iOS";
        public const string NOT_SUPPORTED_UNITY_EDITOR                          = "Not supported on UnityEditor";
        public const string NOT_SUPPORTED_UNITY_EDITOR_WIN                      = "Not supported on UnityEditor of Windows";
        public const string NOT_SUPPORTED_UNITY_EDITOR_OSX                      = "Not supported on UnityEditor of OSX";
        public const string NOT_SUPPORTED_UNITY_STANDALONE                      = "Not supported on UnityStandalone";
        public const string NOT_SUPPORTED_UNITY_STANDALONE_WIN                  = "Not supported on UnityStandalone of Windows";
        public const string NOT_SUPPORTED_UNITY_STANDALONE_OSX                  = "Not supported on UnityStandalone of OSX";
        public const string NOT_SUPPORTED_UNITY_WEBGL                           = "Not supported on UnityWebGL";
        public const string UNKNOWN_ERROR                                       = "Unknown error";
        public const string ALREADY_LOGGED_IN                                   = "You are already logged in";
        public const string LOCALIZED_STRING_NOT_FOUND                          = "The localizedstring file not found";
        public const string LOCALIZED_STRING_LOAD_FAILED                        = "Failed to load the localizedstring";
        public const string LOCALIZED_STRING_LOAD_SUCCEEDED                     = "Succeeded to load the localizedstring";
        public const string LOCALIZED_STRING_EMPTY                              = "The localizedstring is empty";
        public const string LOCALIZED_STRING_KEY_NOT_FOUND                      = "key not found in localizedstring";
        public const string LOCALIZED_STRING_KEY_NOT_SUPPORTED                  = "key not supported";
        public const string DISPLAY_LANGUAGE_CODE_NOT_FOUND                     = "Set displayLanguageCode to deviceLanguageCode because languageCode is not defined in LocalizedString";
        public const string SET_CONSOLE_DEFAULT_DISPLAY_LANGUAGE_CODE           = "Set displayLanguageCode to consoleLanguageCode because languageCode is not defined in LocalizedString";
        public const string SET_DEFAULT_DISPLAY_LANGUAGE_CODE                   = "Set displayLanguageCode to 'en' because displayLanguageCode and deviceLanguageCode is not defined in LocalizedString";
        public const string ADD_OBSERVER_FAILED                                 = "Failed to add observer";
        public const string REMOVE_OBSERVER_FAILED                              = "Failed to remove observer";
        public const string ADD_SERVER_PUSH_FAILED                              = "Failed to add serverPush";
        public const string REMOVE_SERVER_PUSH_FAILED                           = "Failed to remove serverPush";
        public const string ADD_EVENT_HANDLER_FAILED                            = "Failed to add eventHandler";
        public const string REMOVE_EVENT_HANDLER_FAILED                         = "Failed to remove eventHandler";

        //----------------------------------------
        //  Network (Socket)
        //----------------------------------------
        public const string SOCKET_RESPONSE_TIMEOUT                             = "Socket response timeout";
        public const string SOCKET_CONNECTION_TIMEOUT                           = "Socket connection timeout";
        public const string SOCKET_CONNECTION_FAILED                            = "Socket connection failed";
        public const string SOCKET_ERROR                                        = "Socket error";
        public const string SOCKET_SEND_FAIL                                    = "Send failed.";
        public const string SOCKET_NO_INTERNET_CONNECTION                       = "No internet connection";

        //----------------------------------------
        //  Launching
        //----------------------------------------
        public const string LAUNCHING_SERVER_ERROR                              = "Launching server error";
        public const string LAUNCHING_NOT_EXIST_CLIENT_ID                       = "The client ID does not exist";
        public const string LAUNCHING_UNREGISTERED_APP                          = "This app is not registered";
        public const string LAUNCHING_UNREGISTERED_CLIENT                       = "This client is not registered";

        //----------------------------------------
        //  Auth
        //----------------------------------------
        public const string AUTH_USER_CANCELED                                  = "User cancel";
        public const string AUTH_NOT_SUPPORTED_PROVIDER                         = "Not supported provider";
        public const string AUTH_NOT_EXIST_MEMBER                               = "Not exist member";
        public const string AUTH_EXTERNAL_LIBRARY_INITIALIZATION_ERROR          = "Failed to initialize external library.";
        public const string AUTH_EXTERNAL_LIBRARY_ERROR                         = "External library error";
        public const string AUTH_ALREADY_IN_PROGRESS_ERROR                      = "Previous authentication process is not finished yet";

        public const string AUTH_TRANSFERKEY_EXPIRED                            = "TransferKey has been expired";
        public const string AUTH_TRANSFERKEY_CONSUMED                           = "TransferKey has already been used";
        public const string AUTH_TRANSFERKEY_NOT_EXIST                          = "TransferKey is not valid";

        public const string AUTH_TOKEN_LOGIN_FAILED                             = "Login failed";
        public const string AUTH_TOKEN_LOGIN_INVALID_TOKEN_INFO                 = "Invalid token info";
        public const string AUTH_TOKEN_LOGIN_INVALID_LAST_LOGGED_IN_IDP         = "Invalid last logged in provider";
        public const string AUTH_IDP_LOGIN_FAILED                               = "IdP login failed";
        public const string AUTH_IDP_LOGIN_INVALID_IDP_INFO                     = "Invalid IdP information";
        public const string AUTH_ADD_MAPPING_FAILED                             = "Mapping failed";
        public const string AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER     = "Already mapped to other member";
        public const string AUTH_ADD_MAPPING_ALREADY_HAS_SAME_IDP               = "Already has same IdP";
        public const string AUTH_ADD_MAPPING_INVALID_IDP_INFO                   = "Invalid IdP information";
        public const string AUTH_ADD_MAPPING_CANNOT_ADD_GUEST_IDP               = "Mapping with guest IdP is unavailable";
        public const string AUTH_REMOVE_MAPPING_FAILED                          = "Remove mapping failed";
        public const string AUTH_REMOVE_MAPPING_LAST_MAPPED_IDP                 = "Remove mapping failed because this provider is last mapped IdP";
        public const string AUTH_REMOVE_MAPPING_LOGGED_IN_IDP                   = "Currently logged-in provider can not be remove mapping";
        public const string AUTH_LOGOUT_FAILED                                  = "Logout failed";
        public const string AUTH_WITHDRAW_FAILED                                = "Withdraw failed";
        public const string AUTH_WITHDRAW_ALREADY_TEMPORARY_WITHDRAW            = "You are already applying for withdrawal.";
        public const string AUTH_WITHDRAW_NOT_TEMPORARY_WITHDRAW                = "You have never asked to opt-out.";
        public const string AUTH_NOT_PLAYABLE                                   = "Not playable";
        public const string AUTH_UNKNOWN_ERROR                                  = "Auth unknown error";

        public const string AUTH_ADAPTER_NOT_FOUND                              = "Unity Authentication adapter not found.";
        public const string AUTH_ADAPTER_NOT_FOUND_NEED_SETUP                   = "Authentication adapter not found. Please Set up the Unity Authentication adapter for Gamebase";

        //----------------------------------------
        //  Purchase
        //----------------------------------------
        public const string PURCHASE_NOT_INITIALIZED                            = "Purchase not initialized";
        public const string PURCHASE_USER_CANCELED                              = "User canceled purchase";
        public const string PURCHASE_NOT_FINISHED_PREVIOUS_PURCHASING           = "Not finished previous purchasing yet";
        public const string PURCHASE_NOT_SUPPORTED_MARKET                       = "Store code is invalid";
        public const string PURCHASE_EXTERNAL_LIBRARY_ERROR                     = "External library error";
        public const string PURCHASE_UNKNOWN_ERROR                              = "Purchase unknown error";
        public const string PURCHASE_ADAPTER_NOT_FOUND                          = "Purchase adapter not found. Please Set up the Unity Purchase adapter for Gamebase";
        public const string PURCHASE_NOT_ENOUGH_CASH                            = "Not enough cash";
        //----------------------------------------
        //  Push
        //----------------------------------------
        public const string PUSH_NOT_REGISTERED                                 = "Push.registerPush() has not been called yet";
        public const string PUSH_EXTERNAL_LIBRARY_ERROR                         = "External library error";
        public const string PUSH_UNKNOWN_ERROR                                  = "Purchase unknown error";

        //----------------------------------------
        // Webview
        //----------------------------------------        
        public const string WEBVIEW_ADAPTER_NOT_FOUND                           = "Webview adapter not found. Please Set up the Unity Webview adapter for Gamebase";
        public const string WEBVIEW_INVALID_URL                                 = "The url is invalid. Please check it first.";
        public const string WEBVIEW_TIMEOUT                                     = "Timeout occurred while displaying web view.";
        public const string WEBVIEW_HTTP_ERROR                                  = "Display of the webview failed due to an HTTP error.";
        public const string WEBVIEW_OPENED_NEW_BROWSER_BEFORE_CLOSE             = "A new webview is opened before closing the browser type webview.";
        public const string WEBVIEW_UNKNOWN_ERROR                               = "WebView unknown error.";

        //----------------------------------------
        // UI
        //----------------------------------------
        public const string LOGGER_NOT_INITIALIZED                              = "Logger not initialized.";
        public const string LOGGER_EXTERNAL_LIBRARY_ERROR                       = "Logger external library error";
        public const string LOGGER_UNKNOWN_ERROR                                = "Logger unknown error";
        public const string UI_CONTACT_FAIL_INVALID_URL                         = "Service center URL generation failed.";
        public const string UI_CONTACT_FAIL_ISSUE_SHORT_TERM_TICKET             = "Failed to create ShortTermTicket.";
        public const string UI_CONTACT_FAIL_ANDROID_DUPLICATED_VIEW             = "Service center is already open.";
        public const string UI_TERMS_USER_CANCELED                              = "User canceled without agreeing to the terms and conditions.";
        public const string UI_TERMS_NOT_EXIST_IN_CONSOLE                       = "The terms and conditions information is not registered on the console.";
        public const string UI_TERMS_NOT_EXIST_FOR_DEVICE_COUNTRY               = "The terms and conditions information corresponding to the device country code is not registered on the console.";
        public const string UI_TERMS_UNREGISTERED_SEQ                           = "The unregistered terms Seq value.";
        public const string UI_TERMS_ALREADY_IN_PROGRESS_ERROR                  = "Terms API is already in progress.";
        public const string UI_TERMS_ANDROID_DUPLICATED_VIEW                    = "The terms and conditions web view is already open.";
    }
}