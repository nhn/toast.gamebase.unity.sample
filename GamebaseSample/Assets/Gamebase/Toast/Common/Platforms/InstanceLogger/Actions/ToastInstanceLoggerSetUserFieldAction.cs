namespace Toast.Internal
{
    public class ToastInstanceLLoggerSetUserFiledAction : ToastUnityAction
    {
        // Unity Standalone의 ACTION_URI은 프로토콜을 따르지 않는다.
        // 사유는 아래 두레이를 참고한다.
        // https://nhnent.dooray.com/project/posts/2924651715329648388
        public static string ACTION_URI = "toast://instancelogger/setUserField";

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

            string key = payload["key"].Value;
            string value = payload["value"].Value;

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                return ToastNativeMessage.CreateErrorMessage(this.GetUri(),
                                                             this.GetTransactionId(),
                                                             false,
                                                             ToastNativeCommonErrorCode.InvalidParameter.Code,
                                                             "Invalid Parameter").ToJsonString();
            }

            ToastInstanceLoggerSdk.Instance.NativeInstanceLogger.SetUserField(key, value);

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
