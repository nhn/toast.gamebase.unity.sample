
public class ToastGamebaseIapError
{
    public int Code { get; private set; }
    public string Message { get; private set; }

    public ToastGamebaseIapError(int code, string message)
    {
        Code = code;
        Message = message;
    }
}

public static class ToastGamebaseIapErrorCode
{
    public static readonly ToastGamebaseIapError StoreNotMatched = new ToastGamebaseIapError(10001, "Store not matched!!");
    public static readonly ToastGamebaseIapError IncollectStoreCode = new ToastGamebaseIapError(10002, "Incollect store code!!");
    public static readonly ToastGamebaseIapError NotInitialized = new ToastGamebaseIapError(10003, "Not initalized!!");

    public static readonly ToastGamebaseIapError NotSupportedMethod = new ToastGamebaseIapError(10008, "Not supported method!!");
    public static readonly ToastGamebaseIapError DuplicatePurchase = new ToastGamebaseIapError(100009, "Purchase call during purchase!!");

    public static readonly ToastGamebaseIapError OngateCashInsufficient = new ToastGamebaseIapError(60001, "Ongate cash insufficient!!");
    public static readonly ToastGamebaseIapError OngateNotDefined = new ToastGamebaseIapError(60002, "To use Ongate, you must define USE_ONGATE!!");
}
