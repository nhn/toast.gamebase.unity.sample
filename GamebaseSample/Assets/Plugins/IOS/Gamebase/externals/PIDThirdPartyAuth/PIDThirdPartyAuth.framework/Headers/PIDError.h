/*
 *
 * Copyright © 2016년 NHN Entertainment Corp. All rights reserved.
 *
 */

#import <Foundation/Foundation.h>

extern NSString *const PIDErrorKey;

/// @brief SDK 사용 중 발생한 에러타입을 정의합니다.
typedef NS_ENUM(NSInteger, PIDErrorType)
{
    PIDErrorTypeGWAuthFail = 24,                    /* 게이트웨이에서 발생한 에러로 토큰이 만료되었을 경우 발생합니다. */
    PIDErrorFailToCreateOnetimeToken = -1,          /* 일시적으로 인증에 사용될 onetime token의 생성을 실패할 경우 발생합니다. */
    PIDErrorFailToGetToken = -2,                    /* access token을 받아오는데 실패할 경우 발생합니다. */
    PIDErrorInvalidPage = -3,                       /* 잘못된 페이지를 호출할 경우 발생합니다. */
    PIDErrorCancelLogin = -4,                       /* 사용자가 로그인이나 회원가입 동작을 취소하였을 경우 발생합니다. */
    PIDErrorNotSupportAdditionalParameterType = -5, /* ExtraInfo에 지원하지 않는 타입의 값을 파라미터로 넣었을 경우 발생합니다. */
    PIDErrorInvalidParameter = -6,                  /* 사용자가 API호출 시 잘못된 파라미터를 입력했을 경우(null or empty) */
    PIDErrorGetMemberProfileFail = -7               /* getMemberProfile api 호출 시에 통신 에러는 아닌 데 결과값 중에 필수 파라미터가 없는 경우 */
};

/// @brief SDK에서 발생한 에러들을 정의한 오브젝트입니다.
@interface PIDError : NSError

/// @brief NSError의 값들을 PIDError로 wrap해서 반환합니다.
+ (PIDError *)createWrapperWithNSError:(NSError *)error;

/// @brief SDK에서 지정한 error code와 메시지를 PIDError로 wrap해서 반환합니다.
+ (PIDError *)createPIDError:(PIDErrorType)errorCode message:(NSString *)message;

/// @brief PIDError를 넘겨진 파라미터의 notification 키값으로 호출함과 동시에 PIDError를 넘깁니다.
+ (void)informError:(PIDError*)error notification:(NSString *)notification;

/// @brief PIDError에 담긴 에러 메시지를 반환합니다.
- (NSString *)errorMessage;

/// @brief 디버깅을 위한 PIDError의 에러 메시지를 반환합니다.
- (NSString *)errorMessageForDebug;

/// @brief alert view로 넘겨진 텍스트를 보여줍니다.
- (void)alertMessage:(NSString *)title;

/// @brief alert view로 넘겨진 텍스트를 보여주고 확인을 눌렀을 때 동작을 블럭에 넣어서 처리합니다.
- (void)alertMessage:(NSString *)title withCompletion:(void (^)(void))completion;

/// @brief 에러의 domain이 SDK에서 지정한 PIDError의 종류인지 확인합니다.
- (BOOL)isPIDError;

/// @brief 에러의 domain이 SDK에서 지정했으며 그 중에 게이트웨이에서 발생한 에러인지 확인합니다.
- (BOOL)isGWAuthFailError;

@end
