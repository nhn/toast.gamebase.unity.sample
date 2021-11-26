
public class ToastNativeError
{
    public int Code { get; private set; }

    public ToastNativeError(int code)
    {
        int convertedCode = ConvertNativeCommonErrorCode(code);
        Code = convertedCode;
    }

    public int ConvertNativeCommonErrorCode(int code)
    {
        int convertedCode = code;

#if UNITY_IOS
        switch (code)
        {
            case 100:
                convertedCode = ToastNativeCommonErrorCode.NotAvailableNetwork.Code;
                break;
            case 101:
                convertedCode = ToastNativeCommonErrorCode.NetworkFailed.Code;
                break;
            case 102:
                convertedCode = ToastNativeCommonErrorCode.NetworkTimeout.Code;
                break;
            case 103:
                convertedCode = ToastNativeCommonErrorCode.InvalidParameter.Code;
                break;
            case 105:
                convertedCode = ToastNativeCommonErrorCode.InvalidResponse.Code;
                break;
        }
#elif UNITY_ANDROID

#endif

        return convertedCode;
    }
}

public static class ToastNativeCommonErrorCode
{
    public static readonly ToastNativeError InvalidParameter = new ToastNativeError(10000);

    public static readonly ToastNativeError NotAvailableNetwork = new ToastNativeError(10002);
    public static readonly ToastNativeError NetworkFailed = new ToastNativeError(10003);
    public static readonly ToastNativeError NetworkTimeout = new ToastNativeError(10004);
    public static readonly ToastNativeError InvalidResponse = new ToastNativeError(10005);
    public static readonly ToastNativeError InvalidCallback = new ToastNativeError(10007);

    public static readonly ToastNativeError NotSupportedUri = new ToastNativeError(60000);

    public static readonly ToastNativeError UnknownError = new ToastNativeError(99999);
}
