using System.Collections.Generic;

namespace Toast.Internal
{
    public class ToastLoggerLogAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://logger/log";

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


            string level = payload["level"].Value;
            string message = payload["message"].Value;

            if (string.IsNullOrEmpty(level) || (message == null))
            {
                return ToastNativeMessage.CreateErrorMessage(this.GetUri(),
                                                             this.GetTransactionId(),
                                                             false,
                                                             ToastNativeCommonErrorCode.InvalidParameter.Code,
                                                             "Log must have loglevel, message parameters").ToJsonString();
            }


            JSONObject jsonUserFields = payload["userFields"] as JSONObject;
            Dictionary<string, string> userFields = new Dictionary<string, string>();

            if (jsonUserFields != null)
            {
                foreach (string key in jsonUserFields.Keys)
                {
                    string value = jsonUserFields[key].Value;
                    userFields.Add(key, value);
                }
            }

            ToastLoggerSdk.Instance.NativeLogger.Log(level, message, userFields);

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
