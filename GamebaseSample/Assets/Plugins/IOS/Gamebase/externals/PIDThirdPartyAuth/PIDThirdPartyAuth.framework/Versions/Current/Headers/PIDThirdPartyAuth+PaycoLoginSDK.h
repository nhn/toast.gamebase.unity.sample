/*
 *
 * Copyright © 2016년 NHN Entertainment Corp. All rights reserved.
 *
 */

#import <Foundation/Foundation.h>
#import "PIDThirdPartyAuthDelegate.h"
#import "PIDMemberProfile.h"

NS_ASSUME_NONNULL_BEGIN

/// @brief SDK에서 로그인 시 넘겨지는 정보의 딕셔너리에 접근하는 키값입니다.
extern NSString *const PIDAccessTokenKey;
extern NSString *const PIDExpiresKey;
extern NSString *const PIDIDKey DEPRECATED_MSG_ATTRIBUTE("To get the UserID, use memberProfileWithClientID:accessToken:completionHandler: please.");
extern NSString *const PIDServiceExtraKey;

/// @brief 서버 타입으로 알파, 데모, 리얼이 있습니다. 테스트시 데모, 실제 적용시 리얼로 설정해야합니다. (알파는 NHN Ent. 사내용입니다.)
extern NSString *const PIDServerTypeDemo;
extern NSString *const PIDServerTypeAlpha;
extern NSString *const PIDServerTypeReal;

/// @brief 다국어 설정으로 Default는 한국어, English는 영어이며 설정을 안할 경우 자동으로 한국어로 설정됩니다.
extern NSString *const PIDLocaleTypeDefault;
extern NSString *const PIDLocaleTypeEnglish;


/// @brief SDK 사용 정보를 갖는 오브젝트입니다.
@interface PIDAuthThirdConfig : NSObject

@property (nonatomic, retain) NSString *clientID;
@property (nonatomic, retain) NSString *secret;
@property (nonatomic, retain) NSString *serviceCode;
@property (nonatomic, retain) NSString *serviceName;
@property (nonatomic, retain) NSString *serverType;
@property (nonatomic, assign, nullable) NSString *localeType;
@property (nonatomic, retain, nullable) NSString *targetScheme;
@property (nonatomic, assign) BOOL showDebugLog;
@property (nonatomic, weak, nullable)   id <PIDThirdPartyAuthDelegate> delegate;

@end


/// @brief SDK 사용 정보를 설정하고 필요한 기능의 메서드를 호출합니다.
@interface PIDThirdPartyAuth : NSObject

/// @brief SDK 사용에 필요한 정보를 설정합니다.
+ (void)setup:(PIDAuthThirdConfig *)aConfig;

/// @brief 페이코앱으로 로그인 시 AppDelegate에서 scheme을 통해 들어온 로그인 정보를 처리합니다.
+ (BOOL)handleOpenURL:(NSURL *)aURL;

/// @brief 로그인 페이지를 호출합니다. 네비게이션바가 있을 경우 push, 없을 경우 present로 페이지를 띄웁니다.
+ (void)openLoginURL;

/// @brief 회원가입 페이지를 호출합니다. 네비게이션바가 있을 경우 push, 없을 경우 present로 페이지를 띄웁니다.
+ (void)openJoinURL;

/// @brief 로그인 페이지를 호출합니다. 네비게이션바 유무와 관계없이 무조건 present로 페이지를 띄웁니다.
+ (void)openLoginView;

/// @brief 회원가입 페이지를 호출합니다. 네비게이션바 유무와 관계없이 무조건 present로 페이지를 띄웁니다.
+ (void)openJoinView;

/// @brief SDK의 버전 정보를 호출합니다.
+ (NSString *)version;

/// @brief 로그아웃을 시도합니다. access token 정보를 파라미터로 넘겨야 합니다.
+ (void)logout:(NSString *)aAccessToken;

/// @brief 사용자 정보를 조회합니다.
+ (void)memberProfileWithAccessToken:(NSString *)accessToken
                   completionHandler:(nonnull PIDMemberProfileHandler)completionHandler;

@end
NS_ASSUME_NONNULL_END
