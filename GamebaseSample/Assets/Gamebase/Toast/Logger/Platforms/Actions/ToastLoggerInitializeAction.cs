using System;
using Toast.Logger;

namespace Toast.Internal
{
    //InitializeAction
    public class ToastLoggerInitializeAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://logger/initialize";

        protected override string GetUri()
        {
            return ACTION_URI;
        }

        protected override string Action(JSONObject payload)
        {
            if (payload == null)
            {
                return ToastNativeMessage.CreateErrorMessage(this.GetUri(),
                                                             this.GetTransactionId(),
                                                             false,
                                                             ToastNativeCommonErrorCode.InvalidParameter.Code,
                                                             this.GetUri() + " action not found").ToJsonString();
            }

            string projectKey = payload["projectKey"].Value;
            string serviceZone = payload["serviceZone"].Value;
            bool enableCrash = payload["enableCrashReporter"].AsBool;

            if (string.IsNullOrEmpty(projectKey))
            {
                return ToastNativeMessage.CreateErrorMessage(this.GetUri(),
                                                             this.GetTransactionId(),
                                                             false,
                                                             ToastNativeCommonErrorCode.InvalidParameter.Code,
                                                             "Project key does not exist").ToJsonString();
            }

            ToastLoggerConfiguration loggerConfiguration = new ToastLoggerConfiguration();
            loggerConfiguration.AppKey = projectKey;
            loggerConfiguration.ServiceZone = (ToastServiceZone)Enum.Parse(typeof(ToastServiceZone), serviceZone);
            loggerConfiguration.EnableCrashReporter = enableCrash;
            ToastLoggerSdk.Instance.NativeLogger.Initialize(loggerConfiguration);

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
