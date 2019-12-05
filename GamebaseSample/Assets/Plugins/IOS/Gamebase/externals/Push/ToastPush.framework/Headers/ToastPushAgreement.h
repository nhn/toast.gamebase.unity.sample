//
//  ToastPushAgreement.h
//  ToastPush
//
//  Created by JooHyun Lee on 2018. 12. 12..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

/**
 # ToastPushAgreement
 
 A class that has the user's response as to whether or not to accept the notification.
 
 */
@interface ToastPushAgreement : NSObject <NSCoding, NSCopying>

/// ---------------------------------
/// @name Properties
/// ---------------------------------

/** Whether to accept the notification. */
@property (nonatomic) BOOL allowNotifications;

/** Whether to accept the advertising information notification. */
@property (nonatomic) BOOL allowAdvertisements;

/** Whether to accept the advertising information notification when night. */
@property (nonatomic) BOOL allowNightAdvertisements;

/// ---------------------------------
/// @name Initialize
/// ---------------------------------

/**
 Initialize  ToastPushAgreement with allowNotifications.
 
 @param allowNotifications  Whether to accept the notification.
 @return The instance of ToastPushAgreement.
 */
+ (instancetype)agreementWithAllowNotifications:(BOOL)allowNotifications;


/**
 Initialize  ToastPushAgreement with allowNotifications.

 @param allowNotifications  Whether to accept the notification.
 @return The instance of ToastPushAgreement.
 */
- (instancetype)initWithAllowNotifications:(BOOL)allowNotifications;

@end

NS_ASSUME_NONNULL_END
