using Toast.Cef.Webview.Internal;

namespace Toast.Cef.Webview
{
    public static class CefWebviewCallback
    {
        public delegate void ErrorDelegate(CefWebviewError error);
        public delegate void DataDelegate<T>(T data);
        public delegate void CefWebviewDelegate<T>(T data, CefWebviewError error);

        public const string MESSAGE_CALLBACK_CAN_NOT_BE_NULL = "Callback cannot be null";

        public static void InvokeErrorDelegate(ErrorDelegate callback, CefWebviewError error = null)
        {
            if (callback == null)
            {
                CefWebviewLogger.Warn(MESSAGE_CALLBACK_CAN_NOT_BE_NULL, typeof(CefWebviewCallback));

                if (error == null)
                {
                    return;
                }

                CefWebviewLogger.Debug(error, typeof(CefWebviewCallback));
            }
            else
            {
                if (error == null)
                {
                    error = new CefWebviewError(CefWebviewErrorCode.SUCCESS);
                }

                callback(error);
            }
        }

        public static void InvokeCefWebviewDelegate<T>(CefWebviewDelegate<T> callback, T data, CefWebviewError error = null)
        {
            if (callback == null)
            {
                CefWebviewLogger.Warn(MESSAGE_CALLBACK_CAN_NOT_BE_NULL, typeof(CefWebviewCallback));

                if (error == null)
                {
                    return;
                }

                CefWebviewLogger.Debug(string.Format("data:{0}, error:{1}", data, error), typeof(CefWebviewCallback));
            }
            else
            {
                if (error == null)
                {
                    error = new CefWebviewError(CefWebviewErrorCode.SUCCESS);
                }

                callback(data, error);
            }
        }
    }
}