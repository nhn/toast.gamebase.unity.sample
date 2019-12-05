#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeRequest
    {
        public class SDK
        {
            public class Initialize
            {
                public string gameObjectName;
                public List<string> pluginList;
            }

            public class IsDebugMode
            {
                public bool isDebugMode;
            }
        }

        public class Launching
        {

        }

        public class Auth
        {
            public class Login
            {
                public string providerName;
            }

            public class LoginWithAdditionalInfo
            {
                public string providerName;
                public Dictionary<string, object> additionalInfo;
            }            
            
            public class AddMapping
            {
                public string providerName;
            }

            public class AddMappingWithAdditionalInfo
            {
                public string providerName                          = string.Empty;
                public Dictionary<string, object> additionalInfo    = null;
            }

            public class AddMappingForcibly
            {
                public string providerName                          = string.Empty;
                public string forcingMappingKey                     = string.Empty;
            }

            public class AddMappingForciblyWithCredentialInfo
            {
                public string forcingMappingKey                     = string.Empty;
                public Dictionary<string, object> credentialInfo    = null;
            }

            public class AddMappingForciblyWithAdditionalInfo
            {
                public string providerName;
                public string forcingMappingKey;
                public Dictionary<string, object> additionalInfo;
            }

            public class RemoveMapping
            {
                public string providerName;
            }

            public class IssueTransferKey
            {
                public long expiresIn;
            }

            public class RequestTransfer
            {
                public string transferKey;
            }

            public class TransferAccount
            {
                public string accountId;
                public string accountPassword;
            }
        }

        public class Purchase
        {
            public class PurchaseItemSeq
            {
                public long itemSeq;
            }
            public class PurchaseMarketItemId
            {
                public string marketItemId;
            }
        }

        public class Push
        {
            public class RegisterPush
            {
                public Dictionary<string, object> options;
            }

            public class Enable
            {
                public bool enable;
            }

            public class IsSandboxMode
            {
                public bool isSandbox;
            }
        }

        public class Webview
        {
            public class WebviewConfiguration
            {
                public string url;
                public GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration;                
            }
        }

        public class Util
        {
            public class AlertDialog
            {
                public string title;
                public string message;
                public int duration;
            }
        }
    }
}
#endif