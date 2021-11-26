namespace Toast.Iap.Ongate
{
    /// <summary>
    /// IAP 에러타입을 나타내는 enum
    /// </summary>
    public enum IAPErrorType
    {
        /// <summary>
        /// 정상 성공
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// 네트워크 오류 </summary>
        NETWORK_TIMEOUT_ERROR = 100,
        /// <summary>
        /// 유저ID 미등록 </summary>
        AUTHORIZATION_ERRROR = 101,
        /// <summary>
        /// 지원하지 않는 디바이스 </summary>
        UNSUPPORTED_DEVICE_ERROR = 102,
        /// <summary>
        /// 지원하지 않는 마켓 </summary>
        UNSUPPORTED_STORE_ERROR = 103,
        /// <summary>
        /// 유저 취소
        /// </summary>
        USER_CANCELED_ERROR = 104,
        /// <summary>
        /// SDK 초기화 실패 </summary>
        FAILED_INITIALIZED_ERROR = 105,
        /// <summary>
        /// HTTP Response Status 에러 </summary>
        SERVER_UNKNOWN_ERROR = 106,
        /// <summary>
        /// API 요청 실패 </summary>
        RESPONSE_ERROR = 107,
        /// <summary>
        /// 마켓 연동 라이브러리 초기화 실패 </summary>
        INAPP_INITIALIZED_ERROR = 108,
        /// <summary>
        /// 마켓 결제 오류 - 구매 요청 </summary>
        INAPP_PURCHASE_ERROR = 109,
        /// <summary>
        /// 마켓 결제 오류 - 서명 검증 </summary>
        INAPP_VERIFY_SIGNATURE_ERROR = 110,
        /// <summary>
        /// 마켓 결제 오류 - 결제 내역 소모 </summary>
        INAPP_CONSUME_ERROR = 111,
        /// <summary>
        /// 마켓 결제 오류 - 영수증 검증 </summary>
        INAPP_VERIFY_CONSUME_ERROR = 112,
        /// <summary>
        /// 서버 네트워크 에러
        /// </summary>
        INAPP_SERVER_NETWORK_FAIL = 113,
        /// <summary>
        /// 영수증 리프레쉬 요청
        /// </summary>
        INAPP_RECEIPT_REFRESH = 114,
        /// <summary>
        /// 온게이트 캐시 부족
        /// </summary>
        INAPP_ONGATE_CASH_INSUFFICIENT = 115,
        /// <summary>
        /// iOS SKPayment Queue Refresh 요청.
        /// </summary>
        INAPP_APPLE_CLEAR_PAYMENT_QUEUE = 116,
        /// <summary>
        /// SDK 초기화 안됨 </summary>
        FAILED_NOT_INITIALIZED_ERROR = 200,
    }

    /// <summary>
    /// 입앱 결제를 위한 API를 제공 합니다.
    /// </summary>
    public class ToastIapOngate
    {
        public static readonly int API_SUCCESS_CODE = 0;

        /// <summary>
        /// 유니티 플러그인을 초기화 합니다. </summary>
        public static Result Initialize()
        {
            return new Result(true, API_SUCCESS_CODE, null);
        }

        /// <summary>
        /// 디버그를 위한 로그 활성 상태를 설정 한다.
        /// </summary>
        /// <param name="isDebuggable">true 일때, 로그 정보를 활성화 한다.</param>
        public static void SetDebugMode(bool isDebuggable)
        {
            IAPManager.Instance.NativePlugin.SetDebugMode(isDebuggable);
        }

        /// <summary>
        /// 어플리케이션에서 유저 인증 이후에 유저 식별이 가능한 값을 등록 한다.
        /// </summary>
        /// <returns>API 요청에 대한 결과를 반환한다.</returns>
        /// <param name="userId">유저를 식별 할 수 있는 identifier 값으로, userId 는 변하지 않는 고유한 값 이여야만 한다.</param>
        public static Result SetOngateUserId(string userId)
        {
            bool isSuccessful = IAPManager.Instance.NativePlugin.SetOngateUserId(userId);
            if (isSuccessful)
                return new Result(isSuccessful, API_SUCCESS_CODE, userId);
            else
                return new Result(isSuccessful, (int)IAPErrorType.FAILED_INITIALIZED_ERROR, null);
        }

        /// <summary>
        /// 인앱 결제 요청을 한다. 결제 요청에 대한 응답은 OnResponsePurchase delegate를 통해 전달 받는다.
        /// </summary>
        /// <remarks>The Call occur on the UI Thread.</remarks>
        /// <description>
        /// [result data]
        /// - paymentSeq - generated payment id
        /// - itemSeq - represent item id
        /// - purchaseToken - represent token for validation.
        /// - currency - represent item currency.
        /// - price - represent item price.
        /// </description>
        /// <param name="itemId">Web Console에서 등록한 아이템번호 </param>
        /// <param name="aCallback">API 요청 결과에 대한 응답 정보를 전달받는 delegate</param>
        public static void Purchase(string itemId, IAPCallbackHandler.OnResponsePurchase aCallback)
        {
            IAPManager.Instance.NativePlugin.Purchase(itemId, aCallback);
        }

        /// <summary>
        /// 미소비된 결제 내역 리스트를 조회한다.
        /// </summary>
        /// <remarks>The Call occur on the UI Thread.</remarks>
        /// <description>
        /// [result data of list]
        /// - paymentSeq - generated payment id
        /// - itemSeq - represent item id
        /// - purchaseToken - represent token for validation.
        /// - currency - represent item currency.
        /// - price - represent item price.
        /// </description>
        /// <param name="aCallback">API 요청 결과에 대한 응답 정보를 전달받는 delegate</param>
        public static void RequestConsumablePurchases(IAPCallbackHandler.OnResponsePurchase aCallback)
        {
            IAPManager.Instance.NativePlugin.RequestConsumablePurchases(aCallback);
        }

        /// <summary>
        /// 구매 가능한 상품 내역 리스트를 조회한다.
        /// </summary>
        /// <remarks>The Call occur on the UI Thread.</remarks>
        /// <description>
        /// [result data of list]
        /// - itemSeq - represent item id.
        /// - itemName - represent item name.
        /// - marketItemId - represent market item id.
        /// - price - represent item price.
        /// - currency - represent item currency.
        /// </description>
        /// <param name="aCallback">API 요청 결과에 대한 응답 정보를 전달받는 delegate</param>
        public static void RequestProductDetails(IAPCallbackHandler.OnResponsePurchase aCallback)
        {
            IAPManager.Instance.NativePlugin.RequestProductDetails(aCallback);
        }

        /// <summary>
        /// SetUp AppId for Unity WebGL SDK. WebGl-Application must have called at once.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result SetAppId(long appId)
        {
            bool isSuccessful = StandaloneWebGLResourceManager.Instance.SetAppId(appId);
            if (isSuccessful)
                return new Result(isSuccessful, API_SUCCESS_CODE, "Success Setup appId : " + appId);
            else
                return new Result(isSuccessful, (int)IAPErrorType.FAILED_INITIALIZED_ERROR, null);
        }

        public static void SetServiceZone(ServerPhase zone)
        {
            StandaloneWebGLResourceManager.Instance.ServerPhase = zone;
        }
    }

    /// <summary>
    /// API의 응답 결과를 나타냅니다.
    /// </summary>
    public class Result
    {
        public bool isSuccessful;
        public int resultCode;
        public string resultString;

        public Result(bool isSuccessful, int resultCode, string resultString)
        {
            this.isSuccessful = isSuccessful;
            this.resultCode = resultCode;
            this.resultString = resultString;
        }

        /// <summary>
        /// API 의 성공 / 실패 여부를 반환합니다. true 일때 API 응답 성공
        /// </summary>
        /// <value><c>true</c> if this instance is successful; otherwise, <c>false</c>.</value>
        public bool IsSuccessful
        {
            get
            {
                return isSuccessful;
            }
        }

        /// <summary>
        /// API 호출 후 에러 발생시 에러 코드를 반환 합니다.
        /// </summary>
        /// <value>에러 코드를 반환 합니다.</value>
        public int ResultCode
        {
            get
            {
                return resultCode;
            }
        }

        /// <summary>
        /// API 호출 후 에러 발생시 에러에 대한 상세한 정보를 반환 합니다.
        /// </summary>
        /// <value>에러 상세 메세지</value>
        public string ResultString
        {
            get
            {
                return resultString;
            }
        }

        public override string ToString()
        {
            return "isSuccessful=" + isSuccessful + ", resultCode=" + resultCode + ", resultString=" + resultString;
        }
    }
}