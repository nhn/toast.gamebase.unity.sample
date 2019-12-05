//
//  ToastPush+SDK.h
//  ToastPush
//
//  Created by JooHyun Lee on 2018. 11. 30..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UserNotifications/UserNotifications.h>
#import "ToastPushConfiguration.h"
#import "ToastPushAgreement.h"
#import "ToastPushTokenInfo.h"
#import "ToastPushMessage.h"
#import "ToastPushAction.h"

NS_ASSUME_NONNULL_BEGIN

@protocol ToastPushDelegate;

/**
 # ToastPush
 
 SDK to manage push notification.
 
 ## Initialize Precautions
 * ToastPush SDK uses the UserID set in ToastSDK.
 * After you set the UserID, you need to initialize the ToastPush Module.
 * If the user ID is not set, the token registration and inquiry function will be disabled.
 * It is recommended to perform initialization from `application:didFinishLaunchingWithOptions:` function to receive a smooth message.
 * Once you register your delegate, you can do additional tasks after registering the token or after receiving the message / action.
 * When calling initWithConfiguration: and setDelegate: separately, please call the setDelegate: function first. If the initWithConfiguration: function is called without a delegate setting, you can not receive callbacks to register token and receive push.
 
 */
@interface ToastPush : NSObject

/// ---------------------------------
/// @name Initialize
/// ---------------------------------

/**
 Sets Delegate with a given delegate that following ToastPushDelegate

 @param delegate The delegate that following ToastPushDelegate
*/
+ (void)setDelegate:(nullable id<ToastPushDelegate>)delegate;

/**
 Initialize SDK
 
 @param configuration The configuration about Push
*/
+ (void)initWithConfiguration:(ToastPushConfiguration *)configuration;

/**
 Initialize SDK

 @param configuration The configuration about Push
 @param delegate The delegate to be executed according to the token registration result and upon receiving the notification.
 */
+ (void)initWithConfiguration:(ToastPushConfiguration *)configuration
                     delegate:(nullable id<ToastPushDelegate>)delegate;

/// ---------------------------------
/// @name Push Methods
/// ---------------------------------

// 토큰 등록 요청
/**
 Request token registration.
 
 Registers to the OS according to the push type set at initialization, and registers the issued token information to the toast cloud server.
 
 The token registration result is passed through the delegate set at initialization.
 
 @param agreement Whether or not to accept the notification.
 */
+ (void)registerWithAgreement:(ToastPushAgreement *)agreement;

// 카테고리 설정
/**
 Set the notification categories.

 @param categories NSSet with object type UNNotificationCategory(iOS 10.0+) or UIUserNotificationCategory(iOS 8.0-10.0).
 */
+ (void)setCategories:(nullable NSSet *)categories;

// 알림 옵션 설정
/**
 Set the options for displaying notification.
 
 @param options Constants indicating how the app alerts the user when push notification arrives.
                UNAuthorizationOptions(iOS 10.0+) or UIUserNotificationType(iOS 8.0-10.0).
 */
+ (void)setOptions:(NSInteger)options;

// 토큰 조회 요청
/**
 Request token lookup.
 
 View the most recent token and configuration information registered with the current user ID.
 
 @param type The type of push. (APNs or VoIP)
 @param completionHandler The handler to run after token lookup is complete.
 */
+ (void)requestTokenInfoForPushType:(ToastPushType)type
                  completionHandler:(nullable void (^) (ToastPushTokenInfo * _Nullable tokenInfo, NSError * _Nullable error))completionHandler;


// 토큰 삭제
/**
 Unregister token.
 
 It unregisters the token according to the pushTypes and sandbox values registered in the configuration that was set at initialization.
 
 */
+ (void)unregisterToken;

// 언어코드 업데이트 요청
/**
 Update token's language code.
 
 @param tokenInfo The token info to update.
 @param completionHandler The handler to run after update token is complete.
 */
+ (void)updateTokenInfo:(ToastPushTokenInfo *)tokenInfo
      completionHandler:(nullable void (^) (NSArray<ToastPushTokenInfo *> * _Nullable results, NSError * _Nullable error))completionHandler;

// SDK 버전 획득
/**
 Gets the version of SDK.
 
 @return The version of SDK
 */
+ (NSString *)version;

@end


/**
 The delegate to be executed according to the token registration result and upon receiving the notification.
 */
@protocol ToastPushDelegate <NSObject>

@optional

// 토큰 등록 성공
/**
 Called after the token registration has been successfully.

 @param deviceToken A successfully registered device token.
 @param type Registered push type.
 */
- (void)didRegisterWithDeviceToken:(NSString *)deviceToken
                           forType:(ToastPushType)type;

// 토큰 등록 실패
/**
 Called after the token registration has been failure.

 @param type The push type that trying to register.
 @param error The error about the cause of the token registration failure.
 */
- (void)didFailToRegisterForType:(ToastPushType)type
                       withError:(NSError *)error;

// 토큰 해제 성공
/**
 Called after the token unregistration has been successfully.
 
 @param deviceToken A successfully unregistered device token.
 @param type The unregistered push type.
 */
- (void)didUnregisterWithDeviceToken:(nullable NSString *)deviceToken
                             forType:(ToastPushType)type;

// 토큰 해제 실패
/**
 Called after the token unregistration has been failure.
 
 @param deviceToken The deviceToken that trying to unregister.
 @param type The push type that trying to unregister.
 @param error The error about the cause of the token unregistration failure.
 */
- (void)didFailToUnregisterWithDeviceToken:(NSString *)deviceToken
                                   forType:(ToastPushType)type
                                     error:(NSError *)error;

// 푸쉬 메세지 수신
/**
 Called after the push notification has been received successfully.
 
 @param message The received message contents.
 @param type The type of push. (APNs or VoIP)
 */
- (void)didReceivePushMessage:(ToastPushMessage *)message
                      forType:(ToastPushType)type;

- (void)didReceivePushWithPayload:(NSDictionary *)payload
                          forType:(ToastPushType)type __deprecated_msg("use didReceivePushMessage:forType: instead.");


// 알림 액션 수신
/**
 Called after the push action has been received successfully
 
 @param action The received action.
 */
- (void)didReceivePushAction:(ToastPushAction *)action;

- (void)didReceiveNotificationActionWithIdentifier:(NSString *)actionIdentifier
                                categoryIdentifier:(NSString *)categoryIdentifier
                                           payload:(NSDictionary *)payload
                                          userText:(nullable NSString *)userText __deprecated_msg("use didReceivePushAction: instead.");

// 알림에 의한 앱 실행
/**
 Called after the push action has been received successfully
 
 @param message The message contents of touched notification.
 */
- (void)didReceiveNotificationResponseForPushMessage:(ToastPushMessage *)message;

@end

NS_ASSUME_NONNULL_END
