using System.Reflection;

namespace Toast.Internal
{
    public static class ToastSDK
    {
        private static bool _isInitialized = false;
        private static string SDK_INITIALIZE = "SdkInitialize";

        public static string CallMessage(string message)
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            ToastUnityMessage unityMessage = new ToastUnityMessage(message);
            string uri = unityMessage.GetUri();

            ToastUnityAction action = ToastActionHandler.GetAction(uri);

            if (action == null)
            {
                ToastLog.Error("Not supported uri: " + uri);

                ToastNativeMessage toastNative =
                    ToastNativeMessage.CreateErrorMessage(uri,
                                                          unityMessage.TransactionId,
                                                          false,
                                                          ToastNativeCommonErrorCode.NotSupportedUri.Code,
                                                          uri + " action not found");

                ToastLog.Debug(toastNative.ToJsonString());
                return toastNative.ToString();
            }

            return action.OnMessage(unityMessage);
        }

        private static void Initialize()
        {
            ToastReflectionHelper.invokeStatic("Toast.Internal.ToastCoreSdk",
                                               SDK_INITIALIZE,
                                               BindingFlags.NonPublic | BindingFlags.Static);

            ToastReflectionHelper.invokeStatic("Toast.Internal.ToastInstanceLoggerSdk",
                                               SDK_INITIALIZE,
                                               BindingFlags.NonPublic | BindingFlags.Static);

            ToastReflectionHelper.invokeStatic("Toast.Internal.ToastLoggerSdk",
                                               SDK_INITIALIZE,
                                               BindingFlags.NonPublic | BindingFlags.Static);

            ToastReflectionHelper.invokeStatic("Toast.Internal.ToastIapSdk",
                                               SDK_INITIALIZE,
                                               BindingFlags.NonPublic | BindingFlags.Static);

            _isInitialized = true;
        }

        public static bool IsInitialized()
        {
            return _isInitialized;
        }
    }
}