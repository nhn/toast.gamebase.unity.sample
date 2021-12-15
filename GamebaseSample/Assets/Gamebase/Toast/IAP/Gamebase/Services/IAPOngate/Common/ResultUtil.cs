namespace Toast.Iap.Ongate
{
    public class ResultFactory
    {
        public static Result CreateErrorResult(IAPErrorType errorType)
        {
            string message = DefaultMessageToIAPErrorType(errorType);
            return new Result(false, (int)errorType, message);
        }

        public static Result CreateSuccessResult()
        {
            return new Result(true, 0, null);
        }

        public static Result CreateResultFromServerResponse(IapApiResult response)
        {
            IAPErrorType errorType = MappingServerErrorCodeToClientErrorCode(response.GetCode());
            if (errorType == IAPErrorType.SERVER_UNKNOWN_ERROR)
            {
                return new Result(false, response.GetCode(), response.GetMessage());
            }
            else
            {
                string message = DefaultMessageToIAPErrorType(errorType);
                return new Result(response.IsSuccess(), (int)errorType, message);
            }
        }

        private static IAPErrorType MappingServerErrorCodeToClientErrorCode(int serverErrorCode)
        {
            const int SERVER_CODE_SUCCESS = 0;
            const int SERVER_CODE_ONGATE_CASH_INSUFFICIENT = 3402;
            const int SERVER_CODE_CONNECTION_FAIL = 2110;
            const int SERVER_CODE_REFRESH_REQUEST = 5014;

            switch (serverErrorCode)
            {
                case SERVER_CODE_SUCCESS:
                    return IAPErrorType.SUCCESS;
                case SERVER_CODE_ONGATE_CASH_INSUFFICIENT:
                    return IAPErrorType.INAPP_ONGATE_CASH_INSUFFICIENT;
                case SERVER_CODE_CONNECTION_FAIL:
                    return IAPErrorType.INAPP_SERVER_NETWORK_FAIL;
                case SERVER_CODE_REFRESH_REQUEST:
                    return IAPErrorType.INAPP_RECEIPT_REFRESH;
                default:
                    return IAPErrorType.SERVER_UNKNOWN_ERROR;

            }
        }

        private static string DefaultMessageToIAPErrorType(IAPErrorType errorType)
        {
            switch (errorType)
            {
                case IAPErrorType.SUCCESS:
                    return "Success";
                case IAPErrorType.NETWORK_TIMEOUT_ERROR:
                    return "Http protocol could not response (ex. timeout)";
                case IAPErrorType.AUTHORIZATION_ERRROR:
                    return "Authentication is invalid, Please register userId";
                case IAPErrorType.UNSUPPORTED_DEVICE_ERROR:
                    return "Unsupported device";
                case IAPErrorType.UNSUPPORTED_STORE_ERROR:
                    return "Unsupported market";
                case IAPErrorType.USER_CANCELED_ERROR:
                    return "User has been canceled";
                case IAPErrorType.FAILED_INITIALIZED_ERROR:
                    return "Confirm Mobill configurations";
                case IAPErrorType.SERVER_UNKNOWN_ERROR:
                    return "Mobill server HTTP response is not 20x";
                case IAPErrorType.RESPONSE_ERROR:
                    return "Mobill server response on exception. confirm message";
                case IAPErrorType.INAPP_INITIALIZED_ERROR:
                    return "In-app library could not be initialized";
                case IAPErrorType.INAPP_PURCHASE_ERROR:
                    return "In-app purchase flow is on failed";
                case IAPErrorType.INAPP_VERIFY_SIGNATURE_ERROR:
                    return "In-app purchase signature verification flow in on failed";
                case IAPErrorType.INAPP_CONSUME_ERROR:
                    return "In-app consumption flow is on failed";
                case IAPErrorType.INAPP_VERIFY_CONSUME_ERROR:
                    return "In-app consumed purchase verification flow in on failed";
                case IAPErrorType.INAPP_SERVER_NETWORK_FAIL:
                    return "Server network error";
                case IAPErrorType.INAPP_RECEIPT_REFRESH:
                case IAPErrorType.INAPP_ONGATE_CASH_INSUFFICIENT:
                    return "In-app ONGATE Cash was insufficient";
                case IAPErrorType.INAPP_APPLE_CLEAR_PAYMENT_QUEUE:
                    return "clear paymentQueue, retry purchase.";
                default:
                    return "Unknown";
            }
        }
    }
}
