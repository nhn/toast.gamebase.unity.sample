//
//  ToastSDK.h
//  ToastCore
//
//  Created by Hyup on 2017. 9. 13..
//  Copyright © 2017년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, ToastServiceZone) {
    ToastServiceZoneReal = 0,
    ToastServiceZoneAlpha = 1,
    ToastServiceZoneBeta = 2,
};

typedef NS_ENUM(NSInteger, ToastLogLevel) {
    ToastLogLevel_DEBUG = 0,
    ToastLogLevel_INFO = 1,
    ToastLogLevel_WARN = 2,
    ToastLogLevel_ERROR = 3,
    ToastLogLevel_FATAL = 4,
};

/**
 # ToastSDK
 
 */
@interface ToastSDK : NSObject

#pragma mark - init

/// ---------------------------------
/// @name Initialize
/// ---------------------------------

/**
 Gets the singletone instance of a ToastSDK.

 @return singletone instance of a ToastSDK
 */
+ (ToastSDK *)sharedInstance;

#pragma mark - Optional Setting
/// ---------------------------------
/// @name Optional Setting
/// ---------------------------------

/**
 Sets optional policies and send common collection indicator.

 @param array List of optional policies to set.
 */
+ (void)setOptionalPolicyWithArray:(nullable NSArray<NSString *> *)array;

/**
 Sets the user ID for ToastSDK.

 @param userID user ID for ToastSDK
 
 @note The UserID that is set is common to each module of ToastSDK.
 @note Every time you call ToastLogger's log sending API, the user ID you set is sent to the server along with the log.
 */
+ (void)setUserID:(nullable NSString *)userID;

/**
 User ID for ToastSDK.

 @return Currently configured user ID
 */
+ (nullable NSString *)userID;

#pragma mark - DebugMode Setting
/// ---------------------------------
/// @name DebugMode Setting
/// ---------------------------------

/**
 Sets whether to set debug mode.

 @param debugMode If `YES` the debugMode is enabled. If `NO` then the debugMode is disabled
 
 @warning When releasing an app, you must disable debug mode.
 
 */
+ (void)setDebugMode:(BOOL)debugMode;


/**
 Whether or not debugmode is enabled.

 @return If `YES` the debugMode is enabled. If `NO` then the debugMode is disabled
 */
+ (BOOL)isDebugMode;

#pragma mark - Version
+ (NSString *)version;

@end

NS_ASSUME_NONNULL_END
