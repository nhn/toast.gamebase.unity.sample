public class ToastIapError
{
    public int Code { get; private set; }

    public ToastIapError(int code)
    {
        int convertedCode = ConvertIapErrorCode(code);
        Code = convertedCode;
    }

    public int ConvertIapErrorCode(int code)
    {
        int convertedCode = code;

#if UNITY_ANDROID
        switch (code)
        {
            case -2:
                convertedCode = ToastIapErrorCode.NotInitalized.Code;
                break;
            case -1:
                convertedCode = ToastIapErrorCode.NotAvailableStore.Code;
                break;
            case 1:
                convertedCode = ToastIapErrorCode.CanceledPayment.Code;
                break;
            case 2:
                convertedCode = ToastIapErrorCode.NotAvailableStore.Code;
                break;
            case 3:
                convertedCode = ToastIapErrorCode.NotAvailableStore.Code;
                break;
            case 4:
                convertedCode = ToastIapErrorCode.NotAvailableProduct.Code;
                break;
            case 5:
                convertedCode = ToastIapErrorCode.UnknownError.Code;
                break;
            case 6:
                convertedCode = ToastIapErrorCode.UnknownError.Code;
                break;
            case 7:
                convertedCode = ToastIapErrorCode.AlreadyOwned.Code;
                break;
            case 8:
                convertedCode = ToastIapErrorCode.NotOwnedProduct.Code;
                break;
            case 9:
                convertedCode = ToastIapErrorCode.UserInvalid.Code;
                break;
            case 101:
                convertedCode = ToastIapErrorCode.InactiveApplication.Code;
                break;
            case 102:
                convertedCode = ToastIapErrorCode.NotAvailableNetwork.Code;
                break;
            case 103:
                convertedCode = ToastIapErrorCode.VerifyFailed.Code;
                break;
            case 104:
                convertedCode = ToastIapErrorCode.ConsumedPurchase.Code;
                break;
            case 105:
                convertedCode = ToastIapErrorCode.RefundedPurchase.Code;
                break;
            case 106:   // PURCHASE_LIMIT_EXCEEDED
                convertedCode = ToastIapErrorCode.LimitExceededPurchase.Code;
                break;
            case 301:
                convertedCode = ToastIapErrorCode.OnestoreNotLoggedIn.Code;
                break;
            case 302:
                convertedCode = ToastIapErrorCode.OnestoreNotUpdateOrInstalled.Code;
                break;
            case 303:
                convertedCode = ToastIapErrorCode.OnestoreSecurityError.Code;
                break;
            case 304:
                convertedCode = ToastIapErrorCode.OnestoreFailedPurchase.Code;
                break;
            case 401:
                convertedCode = ToastIapErrorCode.RedbeanNotLoggedIn.Code;
                break;
            case 402:
                convertedCode = ToastIapErrorCode.RedbeanNotUpdated.Code;
                break;
            case 403:
                convertedCode = ToastIapErrorCode.RedbeanFailedPurchase.Code;
                break;
            case 404:
                convertedCode = ToastIapErrorCode.RedbeanNotArrivedPurchaseCallback.Code;
                break;
            case 501:
                convertedCode = ToastIapErrorCode.GalaxyNotLoggedIn.Code;
                break;
            case 502:
                convertedCode = ToastIapErrorCode.GalaxyNotUpdate.Code;
                break;
            case 503:
                convertedCode = ToastIapErrorCode.GalaxyFailedPurchase.Code;
                break;
            case 504:
                convertedCode = ToastIapErrorCode.GalaxyServiceDenied.Code;
                break;
            case 9999:
                convertedCode = ToastIapErrorCode.UnknownError.Code;
                break;
        }
#elif UNITY_IOS
        switch (code)
        {
            case 0:
                convertedCode = ToastIapErrorCode.UnknownError.Code;
                break;
            case 1:
                convertedCode = ToastIapErrorCode.NotInitalized.Code;
                break;
            case 2:
                convertedCode = ToastIapErrorCode.NotAvailableStore.Code;
                break;
            case 3:
            case 4:
                convertedCode = ToastIapErrorCode.NotAvailableProduct.Code;
                break;
            case 5:
                convertedCode = ToastIapErrorCode.AlreadyOwned.Code;
                break;
            case 6:
                convertedCode = ToastIapErrorCode.AlreadyInprogress.Code;
                break;
            case 7:
                convertedCode = ToastIapErrorCode.UserInvalid.Code;
                break;
            case 9:
                convertedCode = ToastIapErrorCode.CanceledPayment.Code;
                break;
            case 10:
            case 14:
            case 15:
                convertedCode = ToastIapErrorCode.FailedPayment.Code;
                break;
            case 8:
            case 11:
                convertedCode = ToastIapErrorCode.VerifyFailed.Code;
                break;
            case 12:
                convertedCode = ToastIapErrorCode.FailedChangePurchaseStatus.Code;
                break;
            case 13:
                convertedCode = ToastIapErrorCode.InvalidPurchaseStatus.Code;
                break;
            case 16:
                convertedCode = ToastIapErrorCode.FailedRestore.Code;
                break;
            case 17:
                convertedCode = ToastIapErrorCode.NotAvailablePayment.Code;
                break;
            case 18:    // ToastIAPErrorPurchaseLimitExceeded
                convertedCode = ToastIapErrorCode.LimitExceededPurchase.Code;
                break;
            case 100:
                convertedCode = ToastIapErrorCode.NotAvailableNetwork.Code;
                break;
            case 101:
                convertedCode = ToastIapErrorCode.NetworkFailed.Code;
                break;
            case 102:
                convertedCode = ToastIapErrorCode.NetworkTimeout.Code;
                break;
            case 103:
                convertedCode = ToastIapErrorCode.InvalidParameter.Code;
                break;
            case 105:
                convertedCode = ToastIapErrorCode.InvalidResponse.Code;
                break;
        }
#endif
        return convertedCode;
    }
}

public static class ToastIapErrorCode
{
    public static readonly ToastIapError InvalidParameter = new ToastIapError(10000);
    public static readonly ToastIapError NotAvailableNetwork = new ToastIapError(10002);
    public static readonly ToastIapError NetworkFailed = new ToastIapError(10003);
    public static readonly ToastIapError NetworkTimeout = new ToastIapError(10004);
    public static readonly ToastIapError InvalidResponse = new ToastIapError(10005);

    public static readonly ToastIapError InactiveApplication = new ToastIapError(10010);

    public static readonly ToastIapError NotInitalized = new ToastIapError(50000);
    public static readonly ToastIapError NotAvailableStore = new ToastIapError(50002);
    public static readonly ToastIapError NotAvailableProduct = new ToastIapError(50003);
    public static readonly ToastIapError AlreadyOwned = new ToastIapError(50004);
    public static readonly ToastIapError AlreadyInprogress = new ToastIapError(50005);
    public static readonly ToastIapError UserInvalid = new ToastIapError(50006);
    public static readonly ToastIapError CanceledPayment = new ToastIapError(50007);
    public static readonly ToastIapError FailedPayment = new ToastIapError(50008);
    public static readonly ToastIapError VerifyFailed = new ToastIapError(50009);
    public static readonly ToastIapError FailedChangePurchaseStatus = new ToastIapError(50010);
    public static readonly ToastIapError FailedRenewSubscription = new ToastIapError(50011);
    public static readonly ToastIapError InvalidPurchaseStatus = new ToastIapError(50012);
    public static readonly ToastIapError FailedRestore = new ToastIapError(50013);
    public static readonly ToastIapError NotAvailablePayment = new ToastIapError(50014);
    public static readonly ToastIapError NotOwnedProduct = new ToastIapError(50015);

    public static readonly ToastIapError InactivatedApp = new ToastIapError(50100);
    public static readonly ToastIapError NotConnectedNetwork = new ToastIapError(50101);
    public static readonly ToastIapError FailedVerifyPurchase = new ToastIapError(50102);
    public static readonly ToastIapError ConsumedPurchase = new ToastIapError(50103);
    public static readonly ToastIapError RefundedPurchase = new ToastIapError(50104);
    public static readonly ToastIapError LimitExceededPurchase = new ToastIapError(50105);

    public static readonly ToastIapError OnestoreNotLoggedIn = new ToastIapError(51000);
    public static readonly ToastIapError OnestoreNotUpdateOrInstalled = new ToastIapError(51001);
    public static readonly ToastIapError OnestoreSecurityError = new ToastIapError(51002);
    public static readonly ToastIapError OnestoreFailedPurchase = new ToastIapError(51003);

    public static readonly ToastIapError RedbeanNotLoggedIn = new ToastIapError(52000);
    public static readonly ToastIapError RedbeanNotUpdated = new ToastIapError(52001);
    public static readonly ToastIapError RedbeanFailedPurchase = new ToastIapError(52003);
    public static readonly ToastIapError RedbeanNotArrivedPurchaseCallback = new ToastIapError(52004);

    public static readonly ToastIapError GalaxyNotLoggedIn = new ToastIapError(53000);
    public static readonly ToastIapError GalaxyNotUpdate = new ToastIapError(53001);
    public static readonly ToastIapError GalaxyFailedPurchase = new ToastIapError(53002);
    public static readonly ToastIapError GalaxyServiceDenied = new ToastIapError(53003);

    public static readonly ToastIapError UnknownError = new ToastIapError(59999);
}