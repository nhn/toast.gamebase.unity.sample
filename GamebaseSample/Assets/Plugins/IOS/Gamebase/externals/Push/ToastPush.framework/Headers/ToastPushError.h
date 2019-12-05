//
//  ToastPushError.h
//  ToastPush
//
//  Created by JooHyun Lee on 2018. 11. 30..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <ToastCommon/ToastCommon.h>

static NSString *const ToastPushErrorDomain = @"com.toast.push";

/**
 The error used in ToastPush. 
 */
typedef NS_ENUM(NSUInteger, ToastPushErrorCode) {
    ToastPushErrorUnknown               = 0,    // 알수 없음
    ToastPushErrorNotInitialize         = 1,    // 초기화하지 않음
    ToastPushErrorUserInvalid           = 2,    // 사용자 아이디 미설정
    ToastPushErrorPermissionDenied      = 3,    // 권한 획득 실패
    ToastPushErrorSystemFailed          = 4,    // 시스템에 의한 실패
    ToastPushErrorTokenInvalid          = 5,    // 토큰 값이 없거나 유효하지 않음
    ToastPushErrorAlreadyInProgress     = 6,    // 이미 진행중
    ToastPushErrorParameterInvalid      = 7,    // 매계변수 오류
    ToastPushErrorNotSupported          = 8,    // 지원하지 않는 기능
    
    // 네트워크 사용 불가
    ToastPushErrorNetworkNotAvailable __deprecated_msg("use ToastHttpErrorNetworkNotAvailable instead.")    = ToastHttpErrorNetworkNotAvailable,
    // HTTP Status Code 가 200이 아니거나 서버에서 요청을 제대로 읽지 못함
    ToastPushErrorNetworkFailed __deprecated_msg("use ToastHttpErrorRequestFailed instead.")                = ToastHttpErrorRequestFailed,
    // 타임아웃
    ToastPushErrorTimeout __deprecated_msg("use ToastHttpErrorRequestTimeout instead.")                     = ToastHttpErrorRequestTimeout,
    // 서버 응답 오류
    ToastPushErrorResponseInvalid __deprecated_msg("use ToastHttpErrorResponseInvalid instead.")            = ToastHttpErrorResponseInvalid,
};
