//
//  ToastPushTokenInfo.h
//  ToastPush
//
//  Created by JooHyun Lee on 2018. 11. 30..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastPushConfiguration.h"
#import "ToastPushAgreement.h"

NS_ASSUME_NONNULL_BEGIN

/**
 
 A class that contains information about the token that is passed when using the token lookup request API.( [ToastPush requestTokenInfoForPushType:completionHandler:] )
 
 */
@interface ToastPushTokenInfo : NSObject <NSCoding, NSCopying>

/// ---------------------------------
/// @name Properties
/// ---------------------------------

/** Identifier of user. */
@property (nonatomic, copy, readonly) NSString *userID;

/** A globally unique token that identifies this device to APNs. */
@property (nonatomic, copy, readonly) NSString *deviceToken;

/** A short alphabetic or numeric geographical codes developed to represent countries and dependent areas. */
@property (nonatomic, copy, readonly) NSString *countryCode;

/** A code that assigns letters or numbers as identifiers or classifiers for languages.  */
@property (nonatomic, copy, readonly) NSString *languageCode;

/** The type of push.( APNs or VoIP ) */
@property (nonatomic, copy, readonly) ToastPushType pushType;

/** Whether to accept the notification. */
@property (nonatomic, readonly) BOOL allowNotifications __deprecated_msg("use agreement.allowNotifications instead.");

/** Whether to accept the advertising information notification. */
@property (nonatomic, readonly) BOOL allowAdvertisements __deprecated_msg("use agreement.allowAdvertisements instead.");

/** Whether to accept the advertising information notification when night */
@property (nonatomic, readonly) BOOL allowNightAdvertisements __deprecated_msg("use agreement.allowNightAdvertisements instead.");

/** The agreement of notification. */
@property (nonatomic, copy, readonly) ToastPushAgreement *agreement;

/** A region of the globe that observes a uniform standard time for legal, commercial and social purposes. */
@property (nonatomic, copy, readonly) NSString *timezone;

/** The latest updated date and time. */
@property (nonatomic, copy, readonly) NSString *updateDateTime;

@end


@interface ToastPushMutableTokenInfo : ToastPushTokenInfo

@property (nonatomic, copy, nullable) NSString *userID;

@property (nonatomic, copy, nullable) NSString *countryCode;

@property (nonatomic, copy, nullable) NSString *languageCode;

@property (nonatomic, copy, nullable) ToastPushAgreement *agreement;

@property (nonatomic, copy, nullable) NSString *timezone;

- (instancetype)initWithTokenInfo:(ToastPushTokenInfo *)tokenInfo;

@end

NS_ASSUME_NONNULL_END
