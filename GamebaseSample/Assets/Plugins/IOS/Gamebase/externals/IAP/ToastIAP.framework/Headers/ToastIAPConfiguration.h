//
//  ToastIAPConfiguration.h
//  ToastIAP
//
//  Created by Hyup on 2018. 9. 14..
//  Copyright © 2018년 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <ToastCore/ToastCore.h>

NS_ASSUME_NONNULL_BEGIN

/**
 # ToastIAPConfiguration
 
 Configuration for ToastIAP SDK initialization.
 
 */
@interface ToastIAPConfiguration : NSObject <NSCoding, NSCopying>

/// ---------------------------------
/// @name Properties
/// ---------------------------------

/** The app key of Toast Console Project Key */
@property (nonatomic, copy, readonly) NSString *appKey;

/** The service zone(Real or Alpha) */
@property (nonatomic) ToastServiceZone serviceZone;

/// ---------------------------------
/// @name Initialize
/// ---------------------------------

/**
 Initialize ToastIAPConfiguration with a given app key.

 @param appKey The app key of Toast Console Project key
 @return The instance of ToastIAPConfiguration
 */
+ (nullable instancetype)configurationWithAppKey:(NSString *)appKey;

/**
 Initialize ToastIAPConfiguration with a given appKey.
 
 @param appKey The app key of Toast Console Project key.
 @return The instance of ToastIAPConfiguration.
 */
- (nullable instancetype)initWithAppKey:(NSString *)appKey;

@end

NS_ASSUME_NONNULL_END
