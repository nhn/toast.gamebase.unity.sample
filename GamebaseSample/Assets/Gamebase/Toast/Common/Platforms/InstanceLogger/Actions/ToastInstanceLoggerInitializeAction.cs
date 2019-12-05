using System;

namespace Toast.Internal
{
    //InitializeAction
    public class ToastInstanceLoggerInitializeAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://instancelogger/initalize";

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

            if (string.IsNullOrEmpty(projectKey))
            {
                return ToastNativeMessage.CreateErrorMessage(this.GetUri(),
                                                             this.GetTransactionId(),
                                                             false,
                                                             ToastNativeCommonErrorCode.InvalidParameter.Code,
                                                             "Project key does not exist").ToJsonString();
            }

            ServiceZone zone = (ServiceZone)System.Enum.Parse(typeof(ServiceZone), serviceZone, true);
            ToastInstanceLoggerSdk.Instance.NativeInstanceLogger.Initialize(zone);

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
