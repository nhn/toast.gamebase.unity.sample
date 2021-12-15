using System.Collections.Generic;

namespace Toast.Internal
{
    public class ToastLoggerReportAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://logger/report";

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

            string logType = payload["logType"].Value;
            string level = payload["logLevel"].Value;
            string message = payload["message"].Value;
            string dmpData = payload["dmpData"].ToString();
            
            JSONObject jsonUserFields = payload["userFields"] as JSONObject;
            Dictionary<string, string> userFields = new Dictionary<string, string>();

            if (jsonUserFields != null)
            {
                foreach (string key in jsonUserFields.Keys)
                {
                    string value = jsonUserFields[key].ToString();
                    userFields.Add(key, value);
                }
            }

            ToastLoggerSdk.Instance.NativeLogger.Report(logType, level, message, dmpData, userFields);

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
