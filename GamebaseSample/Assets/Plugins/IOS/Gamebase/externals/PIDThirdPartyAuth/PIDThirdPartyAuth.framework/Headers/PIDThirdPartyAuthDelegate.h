/*
 *
 * Copyright © 2016년 NHN Entertainment Corp. All rights reserved.
 *
 */

@class PIDError;

@protocol PIDThirdPartyAuthDelegate<NSObject>

/// @brief 로그인 성공시 호출되며, user info 딕셔너리에 user id, access token과 추가 정보가 포함되어 전달됩니다.
- (void)didSuccessLogin:(NSDictionary *)userInfo;

/// @brief 로그인 실패시 호출되며, PIDError를 통해서 에러 종류 확인이 가능합니다.
- (void)didFailLogin:(PIDError *)error;

/// @brief 로그아웃 성공시 호출되며, 호출 후에 기존 프로젝트에서 관리하고 있던 access token 정보를 삭제하셔야 합니다.
- (void)didSuccessLogout;

/// @brief 로그아웃 실패시 호출되며, PIDError를 통해서 에러 종류 확인이 가능합니다.
- (void)didFailLogout:(PIDError *)error;

@end
