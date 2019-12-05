//
//  TCGBPush.h
//  Gamebase
//
//  Created by NHN on 2016. 5. 31..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "TCGBPushConfiguration.h"
#import "TCGBPushDelegate.h"
#import "TCGBError.h"

@class TCGBError;

extern NSString* const kTCGBPushEnabledKeyname;          // keyname of pushEnabled property
extern NSString* const kTCGBPushADAgreementKeyname;      // keyname of ADAgreement property
extern NSString* const kTCGBPushADAgreementNightKeyname; // keyname of ADAgreementNight property
extern NSString* const kTCGBPushDisplayLanguageCodeKeyname;  // keyname of displayLanguageCode property

/** The TCGBPush class provides registering push token API to ToastCloud Push Server and querying push token API.
 */
@interface TCGBPush : NSObject

/**
 Register push token to ToastCloud Push Server.
 
 @param configuration The configuration which has pushEnabled, ADAgreement and AdAgreementNight.
 @param completion callback
 
 @see TCGBPushConfiguration
 @since Added 1.4.0.
 */
+ (void)registerPushWithPushConfiguration:(TCGBPushConfiguration *)configuration completion:(void(^)(TCGBError *error))completion;
;

/**
 Query push token to ToastCloud Push Server.
 
 @param completion callback, this callback has TCGBPushConfiguration information.
 
 @see TCGBPushConfiguration
 @since Added 1.4.0.
 */
+ (void)queryPushWithCompletion:(void(^)(TCGBPushConfiguration *configuration, TCGBError *error))completion;

/**
 Set SandboxMode.
 
 @param isSandbox `YES` if application is on the sandbox mode.
 @since Added 1.4.0.
 */
+ (void)setSandboxMode:(BOOL)isSandbox;

@end
