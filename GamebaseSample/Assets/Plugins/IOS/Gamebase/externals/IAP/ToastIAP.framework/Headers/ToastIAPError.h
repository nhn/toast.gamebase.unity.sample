//
//  ToastIAPError.h
//  ToastIAP
//
//  Created by JooHyun Lee on 2018. 9. 12..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <ToastCommon/ToastCommon.h>

static NSString *const ToastIAPErrorDomain = @"com.toast.iap";

typedef NS_ENUM(NSUInteger, ToastIAPErrorCode) {
    ToastIAPErrorUnknown = 0,                       // 알수 없음
    
    ToastIAPErrorNotInitialized = 1,                // 초기화 하지 않음
    ToastIAPErrorStoreNotAvailable = 2,             // 스토어 사용 불가
    ToastIAPErrorProductNotAvailable = 3,           // 상품 정보 획득 실패
    ToastIAPErrorProductInvalid = 4,                // 원결제의 상품 아이디와 현재 상품 아이디 불일치
    ToastIAPErrorAlreadyOwned = 5,                  // 이미 소유한 상품
    ToastIAPErrorAlreadyInProgress = 6,             // 이미 진행중인 요청 있음
    ToastIAPErrorUserInvalid = 7,                   // 현재 사용자 아이디가 결제 사용자 아이디와 불일치
    ToastIAPErrorPaymentInvalid = 8,                // 결제 추가정보(ApplicationUsername) 획득 실패
    ToastIAPErrorPaymentCancelled = 9,              // 스토어 결제 취소
    ToastIAPErrorPaymentFailed = 10,                // 스토어 결제 실패
    ToastIAPErrorVerifyFailed = 11,                 // 영수증 검증 실패
    ToastIAPErrorChangePurchaseStatusFailed = 12,   // 구매 상태 변경 실패
    ToastIAPErrorPurchaseStatusInvalid = 13,        // 구매 진행 불가 상태
    ToastIAPErrorExpired = 14,                      // 구독 만료
    ToastIAPErrorRenewalPaymentNotFound = 15,       // 영수증내에 갱신 결제와 일치하는 결제 정보가 없음
    ToastIAPErrorRestoreFailed = 16,                // 복원 실패
    ToastIAPErrorPaymentNotAvailable = 17,          // 구매 진행 불가 상태 (e.g. 앱 내 구입 제한 설정)
    
    // 네트워크 사용 불가
    ToastIAPErrorNetworkNotAvailable __deprecated_msg("use ToastHttpErrorNetworkNotAvailable instead.") = ToastHttpErrorNetworkNotAvailable,
    // HTTP Status Code 가 200이 아니거나 서버에서 요청을 제대로 읽지 못함
    ToastIAPErrorNetworkFailed __deprecated_msg("use ToastHttpErrorRequestFailed instead.") = ToastHttpErrorRequestFailed,
    // 타임아웃
    ToastIAPErrorTimeout __deprecated_msg("use ToastHttpErrorRequestTimeout instead.") = ToastHttpErrorRequestTimeout,
    // 잘못된 요청 (파라미터 오류 등)
    ToastIAPErrorParameterInvalid __deprecated_msg("use ToastHttpErrorRequestInvalid instead.") = ToastHttpErrorRequestInvalid,
    // 서버 응답 오류
    ToastIAPErrorResponseInvalid __deprecated_msg("use ToastHttpErrorResponseInvalid instead.") = ToastHttpErrorResponseInvalid,
};
