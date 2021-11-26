using System.Collections.Generic;

namespace Toast.Internal
{
    //InitializeAction
    public class ToastInstanceLoggerLogAction : ToastUnityAction
    {
        // Unity Standalone의 ACTION_URI은 프로토콜을 따르지 않는다.
        // 사유는 아래 두레이를 참고한다.
        // https://nhnent.dooray.com/project/posts/2924651715329648388
        public static string ACTION_URI = "toast://instancelogger/log";

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

            string type = payload["type"].Value;
            string level = payload["level"].Value;
            string message = payload["message"].Value;

            if (string.IsNullOrEmpty(type))
            {
                type = "NORMAL";
            }

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

            ToastInstanceLoggerSdk.Instance.NativeInstanceLogger.Log(type, level, message, userFields);

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
