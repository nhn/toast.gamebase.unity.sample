public class ToastLoggerError
{
    public int Code { get; private set; }

    public ToastLoggerError(int code)
    {
        int convertedCode = ConvertLoggerErrorCode(code);
        Code = convertedCode;
    }

    public int ConvertLoggerErrorCode(int code)
    {
        int convertedCode = code;

#if UNITY_IOS

#elif UNITY_ANDROID

#endif

        return convertedCode;
    }
}

public static class ToastLoggerErrorCode
{
    public static readonly ToastLoggerError NotInitalizedLogger = new ToastLoggerError(20000);
    public static readonly ToastLoggerError NotInitalizedCrash = new ToastLoggerError(20001);
    public static readonly ToastLoggerError InvalidUserKey = new ToastLoggerError(20002);
    public static readonly ToastLoggerError NotFoundCrashReport = new ToastLoggerError(20003);
}