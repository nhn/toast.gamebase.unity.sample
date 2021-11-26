#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
namespace Toast.Gamebase.Internal.Single
{
    public static class GamebaseServerErrorCode
    {
        #region Common
        /// <summary>
        /// parameter 는 int 형으로 선언되어 있는데, String 형 데이터가 전달됨
        /// </summary>
        public const int TYPE_MISS_MATCH                                = -4000001;

        /// <summary>
        /// 필수 parameter 가 생략되었거나 값이 없을때
        /// </summary>
        public const int ILLEGAL_ARGUMENT                               = -4000002;

        /// <summary>
        /// request body에 정의되지 않은 값이 전달된 경우
        /// </summary>
        public const int HTTP_MESSAGE_NOT_READABLE                      = -4000003;

        /// <summary>
        /// 필수 parameter 가 생략된 경우
        /// </summary>
        public const int MISSING_SERVLET_REQUEST_PARAMETER              = -4000004;

        /// <summary>
        /// 필수 파라미터가 생략되었거나, 부적절한 값으로 호출될 때
        /// </summary>
        public const int METHOD_ARGUMENT_NOT_VALID                      = -4000005;

        /// <summary>
        /// parameter 는 int 형으로 선언되어 있는데, String 형 데이터가 전달됨
        /// </summary>
        public const int METHOD_ARGUMENT_TYPE_MISMATCH                  = -4000006;

        /// <summary>
        /// 잘못된 appId가 호출 됨
        /// </summary>
        public const int INVALID_APP_ID                                 = -4010001;

        /// <summary>
        /// 잘못된 appKey가 호출 됨
        /// </summary>
        public const int INVALID_APP_KEY                                = -4010002;

        /// <summary>
        /// 인증되지 않은 커넥션에서, 인증이 필요한 API 호출
        /// </summary>
        public const int NOT_AUTHENTICATED                              = -4010003;

        /// <summary>
        /// 잘못된 secretKey가 호출 됨
        /// </summary>
        public const int INVALID_SECRET_KEY                             = -4010004;
        
        /// <summary>
        /// 계정이 삭제 또는 유실된 사용자
        /// </summary>
        public const int INVALID_MEMBER                                 = -4010204;

        /// <summary>
        /// 이용 정지 유저
        /// </summary>
        public const int BANNED_MEMBER                                  = -4010205;
        
        /// <summary>
        /// 내부 시스템 오류 발생
        /// </summary>
        public const int UNKNOWN_SYSTEM                                 = -5000001;

        /// <summary>
        /// 외부 연동 중(외부 api call 중), 오류 발생
        /// </summary>
        public const int REMOTE_SYSTEM                                  = -5000002;
        #endregion

        #region Lighthouse
        /// <summary>
        /// 인증되지 않은 커넥션에서 인증이 필요한 API 호출
        /// </summary>
        public const int LIGHT_HOUSE_NOT_AUTHENTICATED                  = -4010101;

        /// <summary>
        /// 등록되지 않은 API 호출
        /// </summary>
        public const int LIGHT_HOUSE_NO_SUCH_REQUEST_API                = -4040101;

        /// <summary>
        /// 요청한 JSON 문자열 포맷이 잘 못 됨
        /// </summary>
        public const int LIGHT_HOUSE_JSON_PARSING_ERROR                 = -4150101;

        /// <summary>
        /// GW 연결 실패
        /// 해당 오류가 발생한 경우 client에서는 `잠시 시스템 점겅중입니다. 잠시 후에 다시 접속해 주세요' 등의 메시지 출력
        /// </summary>
        public const int LIGHT_HOUSE_GATEWAY_CONNECTION_ERROR           = -5000101;
        #endregion

        #region Gateway
        /// <summary>
        /// JSON 파싱 에러
        /// </summary>
        public const int GATEWAY_JSON_PARSING_ERROR                     = -4000201;

        /// <summary>
        /// 필수 요청 파라미터가 빠짐
        /// </summary>
        public const int GATEWAY_MISSING_REQUEST_PARAMETER              = -4000202;

        /// <summary>
        /// 잘못된 appId 로 호출 / 유효하지 않은 appId
        /// </summary>
        public const int GATEWAY_INVALID_APP_ID                         = -4010202;

        /// <summary>
        /// gamebase-id(회원) 시스템 쪽에서 오류 발생 
        /// accessToken 이 유효하지 않음 등 traceError 객체를 살펴 봐야 함
        /// </summary>
        public const int GATEWAY_INVALID_ACCESS_TOKEN                   = -4010203;


        /// <summary>
        /// 잘못된 product id 로 호출
        /// </summary>
        public const int GATEWAY_PRODUCT_DATA_NOT_FOUND                 = -4040201;


        /// <summary>
        /// forcingMappingKey 가 없는 key 인 경우 발생.
        /// </summary>
        public const int MEMBER_FORCING_MAPPING_TICKET_NOT_EXIST        = -4040404;

        /// <summary>
        /// forcingMappingKey 가 이미 사용된 key 인 경우 발생.
        /// </summary>
        public const int MEMBER_CONSUMED_FORCING_MAPPING_TICKET         = -4100404;

        /// <summary>
        /// forcingMappingKey 가 만료된 key 인 경우 발생.
        /// </summary>
        public const int MEMBER_EXPIRED_FORCING_MAPPING_TICKET          = -4100405;

        /// <summary>
        /// 이전 addMapping 요청의 idp 와 강제 매핑 요청 idp 가 서로 다른 경우 발생.
        /// </summary>
        public const int MEMBER_IDP_MISS_MATCH_FOR_FORCING_MAPPING      = -4000410;

        /// <summary>
        /// 이전 addMapping 요청의 idp 와 강제 매핑 요청 IdP 는 동일하지만 authKey 값이 다른 경우 발생.
        /// </summary>
        public const int MEMBER_AUTHKEY_MISS_MATCH_FOR_FORCING_MAPPING  = -4000411;

        /// <summary>
        /// REAL 장비에 SANDBOX appId가 접속 혹은 그 반대
        /// </summary>
        public const int INVALID_APP_TYPE                               = -4120201;

        /// <summary>
        /// 잘못된 api id 로 호출
        /// </summary>
        public const int GATEWAY_REQUEST_API_NOT_FOUND                  = -4040202;
        
        /// <summary>
        /// 이미 로그인된 IdP에 대해 remove mapping을 시도 할 때 발생
        /// </summary>
        public const int GATEWAY_LOGGED_IN_IDP_COULD_NOT_DELETED        = -4050201;

        /// <summary>
        /// 내부 오류 발생
        /// </summary>
        public const int GATEWAY_UNKNOWN_SYSTEM                         = -5000201;

        /// <summary>
        /// 내부 Worker 쓰레드에서 발생한 정의되지 않은 오류 발생
        /// 서버측에 문의
        /// </summary>
        public const int GATEWAY_REQUEST_WORKER_ERROR                   = -5000202;

        /// <summary>
        /// 큐에서 데이터를 꺼냈지만, 이미 오랜 시간 경과하여 최종 목적지 product와 연결이 busy 한 상태
        /// </summary>
        public const int GATEWAY_QUEUE_TIME_OUT                         = -5040201;

        /// <summary>
        /// 큐가 이미 꽉차 있음
        /// </summary>
        public const int GATEWAY_QUEUE_CAPACITY_FULL                    = -5040202;

        /// <summary>
        /// gateway에서 launching 연결 실패
        /// </summary>
        public const int GATEWAY_GB_LNC_SYSTEM_ERROR                    = -5100201;

        /// <summary>
        /// gateway에서 gb-id 연결 실패
        /// gb-id 쪽은 throwPointErrorCode 도 살펴 봐야 함
        /// </summary>
        public const int GATEWAY_GB_ID_SYSTEM_ERROR                     = -5110201;

        /// <summary>
        /// 이미 탈퇴 요청중입니다.
        /// </summary>
        public const int MEMBER_CANNOT_TEMPORARY_WITHDRAW               = -4130404;

        /// <summary>
        /// 탈퇴 요청을 한 적이 없습니다.
        /// </summary>
        public const int MEMBER_NOT_TEMPORARY_WITHDRAW                  = -4130402;



        #endregion

        #region Launching
        /// <summary>
        /// TAP-ID ClientId 정보가 존재하지 않음
        /// </summary>
        public const int LAUNCHING_NOT_EXIST_CLIENT_ID                  = -4040301;

        /// <summary>
        /// 등록되지 않은 App(TC Project)
        /// </summary>
        public const int LAUNCHING_UNREGISTERED_APP                     = -4040302;

        /// <summary>
        /// 등록되지 않은 Client
        /// </summary>
        public const int LAUNCHING_UNREGISTERED_CLIENT                  = -4040303;
        #endregion

        #region Member
        /// <summary>
        /// AppId를 잘못 입력했을 때
        /// </summary>
        public const int MEMBER_APP_ID_MISS_MATCH                       = -4000401;

        /// <summary>
        /// UserId를 잘못 입력 했을 때
        /// </summary>
        public const int MEMBER_USER_ID_MISS_MATCH                      = -4000402;

        /// <summary>
        /// 잘못된 회원에 대한 요청일 때
        /// </summary>
        public const int MEMBER_INVALID_MEMBER                          = -4000403;

        /// <summary>
        /// 잘못된 Auth에 대한 요청일 때
        /// </summary>
        public const int MEMBER_INVALID_AUTH                            = -4000404;

        /// <summary>
        /// 이용 정지된 회원이 로그인 요청을 했을 때
        /// </summary>
        public const int BANNED_MEMBER_LOGIN                            = -4000405;

        /// <summary>
        /// addMapping 요청시 대상 authKey가 이용 정지된 사용자와 매핑되어 있을 경우
        /// </summary>
        public const int AUTHKEY_BELONG_TO_BANNED_MEMBER                = -4000406;

        /// <summary>
        /// 존재하지 않거나 탈퇴된 회원에 대한 요청일 때
        /// </summary>
        public const int MEMBER_NOT_EXIST                               = -4040401;
                  
        /// <summary>
        /// removeMapping을 호출하였으나 매핑되어 있는 OAuth 계정이 하나 일 때
        /// </summary>
        public const int MEMBER_LAST_MAPPED_IDP                         = -4060401;

        /// <summary>
        /// guest 이외에 다른 idPCode 를 가지고 있음 
        /// (guest idPCode 만 가지고 있는 user 가 transfer account 를 발급 받을 수 있음)
        /// </summary>
        public const int GATEWAY_USER_MUST_HAVE_ONLY_GUEST_IDP          = -4120202;

        /// <summary>
        /// 이미 탈퇴된 회원에 대한 요청일 때
        /// </summary>
        public const int MEMBER_ALREADY_WITHDRAWN                       = -4100401;

        /// <summary>
        /// transferaccount 계정을 이미 사용한 경우
        /// </summary>
        public const int MEMBER_TRANSFERACCOUNT_ALREADY_USED            = -4100402;

        /// <summary>
        /// 이미 다른 회원과 매핑된 계정에 대한 요청일 때
        /// </summary>
        public const int MEMBER_ALREADY_MAPPED_MEMBER                   = -4120401;

        /// <summary>
        /// 이미 다른 IdP와 매핑된 계정에 대한 요청일 때
        /// </summary>
        public const int MEMBER_ALREADY_MAPPED_IDP                      = -4120402;

        /// <summary>
        /// 템플릿 이름이 중복 되었을 때
        /// </summary>
        public const int DUPLICATED_TEMPLATE_NAME                       = -4120403;

        /// <summary>
        /// 이미 IdP 매핑이 되어 있는 사용자가 guest로 add mapping 시도 시 발생.
        /// </summary>
        public const int CAN_NOT_ADD_GUEST_IDP                          = -4120405;
        #endregion

        #region Transfer Account
        /// <summary>
        /// 이미 IdP 매핑이 되어 있는 사용자가 guest로 add mapping 시도 시 발생.
        /// </summary>
        public const int MEMBER_IS_NOT_GUEST                            = -4120404;

        /// <summary>
        /// block id.
        /// </summary>
        public const int MEMBER_BLOCK_ID                                = -4000901;

        /// <summary>
        /// NEOID 에서 계정 생성 또는 조회 실패.
        /// </summary>
        public const int MEMBER_NEOID_QUERY                             = -4000920;

        /// <summary>
        /// ID (자동)생성 또는 변경시 알수 없는 오류.
        /// </summary>
        public const int MEMBER_NEOID_SIGNUP_UNKNOWN                    = -4000921;

        /// <summary>
        /// 이미 IdP 매핑이 되어 있는 사용자가 guest로 add mapping 시도 시 발생.
        /// </summary>
        public const int MEMBER_NEOID_QUERY_INFO                        = -4000922;

        /// <summary>
        /// 이미 IdP 매핑이 되어 있는 사용자가 guest로 add mapping 시도 시 발생.
        /// </summary>
        public const int MEMBER_NEOID_CREATE_PASSWORD                   = -4000923;

        /// <summary>
        /// 로그인시 알수 없는 오류.
        /// </summary>
        public const int MEMBER_NEOID_CHECK_PASSWORD_UNKNOWN            = -4000924;

        /// <summary>
        /// 메뉴얼로 ID 변경시 중복 ID인 경우.
        /// </summary>
        public const int MEMBER_NEOID_SIGNUP_ALREADY_EXIST_ID           = -4000925;

        /// <summary>
        /// 로그인시 ID 가 존재하지 않을 때.
        /// </summary>
        public const int MEMBER_NEOID_CHECK_PASSWORD_NOT_EXIST_ID       = -4040920;

        /// <summary>
        /// 로그인시 PW 잘못 입력.
        /// </summary>
        public const int MEMBER_NEOID_CHECK_PASSWORD_INVALID_PASSWORD   = -4000927;

        /// <summary>
        /// NEOID 연결 오류.
        /// </summary>
        public const int MEMBER_NEOID_CONNECTION                        = -5110920;

        /// <summary>
        /// 기존에 발급 받은 transferAccount 가 존재하지 않거나, 기간 만료 되었음.
        /// </summary>
        public const int MEMBER_TRANSFERACCOUNT_EXPIRED                 = -4100403;

        /// <summary>
        /// transfer account 발급 조건이 설정되어 있지 않음. Console 에서 처음에 설정이 필요 함.
        /// </summary>
        public const int MEMBER_CONSOLE_NO_CONDITION                    = -4040901;

        /// <summary>
        /// 동일한 단말기에서 transfer를 요청한 경우.
        /// </summary>
        public const int MEMBER_SAME_REQUESTOR                          = -4000409;

        /// <summary>
        /// transfer 요청한 유저에게 발급 transferKey 가 없을 경우.
        /// </summary>
        public const int MEMBER_TRANSFERACCOUNT_NOT_EXIST               = -4040403;
        #endregion

        #region Purchase
        /// <summary>
        /// GamebaseProductID is not activated.
        /// </summary>
        public const int PURCHASE_NOT_USED_PRODUCT                      = -4001001;

        /// <summary>
        /// GamebaseProductID is not exist.
        /// </summary>
        public const int PURCHASE_NOT_FOUND_PRODUCT_ID                  = -4041001;
        #endregion
    }
}
#endif