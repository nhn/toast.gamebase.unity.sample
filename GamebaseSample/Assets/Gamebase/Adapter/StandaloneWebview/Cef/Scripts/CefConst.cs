namespace Toast.Cef.Webview
{
    #region Enumerator
    public enum WEB_CURSOR_TYPE
    {
        CT_POINTER = 1,
        CT_CROSS,
        CT_HAND,
        CT_IBEAM,
        CT_WAIT,
        CT_HELP,
        CT_EASTRESIZE,
        CT_NORTHRESIZE,
        CT_NORTHEASTRESIZE,
        CT_NORTHWESTRESIZE,
        CT_SOUTHRESIZE,
        CT_SOUTHEASTRESIZE,
        CT_SOUTHWESTRESIZE,
        CT_WESTRESIZE,
        CT_NORTHSOUTHRESIZE,
        CT_EASTWESTRESIZE,
        CT_NORTHEASTSOUTHWESTRESIZE,
        CT_NORTHWESTSOUTHEASTRESIZE,
        CT_COLUMNRESIZE,
        CT_ROWRESIZE,
        CT_MIDDLEPANNING,
        CT_EASTPANNING,
        CT_NORTHPANNING,
        CT_NORTHEASTPANNING,
        CT_NORTHWESTPANNING,
        CT_SOUTHPANNING,
        CT_SOUTHEASTPANNING,
        CT_SOUTHWESTPANNING,
        CT_WESTPANNING,
        CT_MOVE,
        CT_VERTICALTEXT,
        CT_CELL,
        CT_CONTEXTMENU,
        CT_ALIAS,
        CT_PROGRESS,
        CT_NODROP,
        CT_COPY,
        CT_NONE,
        CT_NOTALLOWED,
        CT_ZOOMIN,
        CT_ZOOMOUT,
        CT_GRAB,
        CT_GRABBING,
        CT_CUSTOM,
    };

    public enum WEB_NAVIGATION_DIRECTION
    {
        HOME    = 0,
        BACK    = 1,
        FORWARD = 2,
    };

    public enum LOAD_ERROR_CODE
    {
        ERR_NONE                                            = 0,
        ERR_FAILED                                          = -2,
        ERR_ABORTED                                         = -3,
        ERR_INVALID_ARGUMENT                                = -4,
        ERR_INVALID_HANDLE                                  = -5,
        ERR_FILE_NOT_FOUND                                  = -6,
        ERR_TIMED_OUT                                       = -7,
        ERR_FILE_TOO_BIG                                    = -8,
        ERR_UNEXPECTED                                      = -9,
        ERR_ACCESS_DENIED                                   = -10,
        ERR_NOT_IMPLEMENTED                                 = -11,
        ERR_CONNECTION_CLOSED                               = -100,
        ERR_CONNECTION_RESET                                = -101,
        ERR_CONNECTION_REFUSED                              = -102,
        ERR_CONNECTION_ABORTED                              = -103,
        ERR_CONNECTION_FAILED                               = -104,
        ERR_NAME_NOT_RESOLVED                               = -105,
        ERR_INTERNET_DISCONNECTED                           = -106,
        ERR_SSL_PROTOCOL_ERROR                              = -107,
        ERR_ADDRESS_INVALID                                 = -108,
        ERR_ADDRESS_UNREACHABLE                             = -109,
        ERR_SSL_CLIENT_AUTH_CERT_NEEDED                     = -110,
        ERR_TUNNEL_CONNECTION_FAILED                        = -111,
        ERR_NO_SSL_VERSIONS_ENABLED                         = -112,
        ERR_SSL_VERSION_OR_CIPHER_MISMATCH                  = -113,
        ERR_SSL_RENEGOTIATION_REQUESTED                     = -114,
        ERR_CERT_COMMON_NAME_INVALID                        = -200,
        ERR_CERT_BEGIN                                      = ERR_CERT_COMMON_NAME_INVALID,
        ERR_CERT_DATE_INVALID                               = -201,
        ERR_CERT_AUTHORITY_INVALID                          = -202,
        ERR_CERT_CONTAINS_ERRORS                            = -203,
        ERR_CERT_NO_REVOCATION_MECHANISM                    = -204,
        ERR_CERT_UNABLE_TO_CHECK_REVOCATION                 = -205,
        ERR_CERT_REVOKED                                    = -206,
        ERR_CERT_INVALID                                    = -207,
        ERR_CERT_WEAK_SIGNATURE_ALGORITHM                   = -208,
        // -209 is available: was ERR_CERT_NOT_IN_DNS.
        ERR_CERT_NON_UNIQUE_NAME                            = -210,
        ERR_CERT_WEAK_KEY                                   = -211,
        ERR_CERT_NAME_CONSTRAINT_VIOLATION                  = -212,
        ERR_CERT_VALIDITY_TOO_LONG                          = -213,
        ERR_CERT_END = ERR_CERT_VALIDITY_TOO_LONG,
        ERR_INVALID_URL                                     = -300,
        ERR_DISALLOWED_URL_SCHEME                           = -301,
        ERR_UNKNOWN_URL_SCHEME                              = -302,
        ERR_TOO_MANY_REDIRECTS                              = -310,
        ERR_UNSAFE_REDIRECT                                 = -311,
        ERR_UNSAFE_PORT                                     = -312,
        ERR_INVALID_RESPONSE                                = -320,
        ERR_INVALID_CHUNKED_ENCODING                        = -321,
        ERR_METHOD_NOT_SUPPORTED                            = -322,
        ERR_UNEXPECTED_PROXY_AUTH                           = -323,
        ERR_EMPTY_RESPONSE                                  = -324,
        ERR_RESPONSE_HEADERS_TOO_BIG                        = -325,
        ERR_CACHE_MISS                                      = -400,
        ERR_INSECURE_RESPONSE                               = -501,
    };

    public enum JS_DIALOG_TYPE
    {
        ALERT = 0,
        CONFIRM,
        PROMPT,
    };

    public enum INPUT_ELEMENT_TYPE
    {
        PASSWORD  = 1,
        ELSE      = 100,
    };
    #endregion


}