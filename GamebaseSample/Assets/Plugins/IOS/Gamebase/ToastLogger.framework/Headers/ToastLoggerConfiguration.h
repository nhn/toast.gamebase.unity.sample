//
//  ToastLoggerConfiguration.h
//  ToastLogger
//
//  Created by JooHyun Lee on 07/03/2019.
//  Copyright © 2019 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <ToastCore/ToastCore.h>
#import "ToastLoggerConfigurationSetting.h"

NS_ASSUME_NONNULL_BEGIN

/**
 # ToastLoggerConfiguration
 
 The configuration used to initialize the ToastLogger SDK.
 */
@interface ToastLoggerConfiguration : NSObject

/// ---------------------------------
/// @name Properties
/// ---------------------------------

/** Log & Crasyh Search app key on the toast console */
@property (nonatomic, copy, readonly) NSString *appKey;

/** configuration setting about Toast Logger */
@property (nonatomic, copy, readonly) ToastLoggerConfigurationSetting *setting;

/** Whether or not sending crash is enabled. */
@property (nonatomic) BOOL enableCrashReporter;

/** TOAST Cloud service zone(Real or Alpha) */
@property (nonatomic) ToastServiceZone serviceZone;


/**
 Initialize a configuration with the given appKey.

 @param appKey AppKey on the toast console
 @return A instance of ToastLoggerConfiguration
 */
+ (nullable instancetype)configurationWithAppKey:(NSString *)appKey;

/**
 Initialize a configuration with the given appKey and enableCrashReport.

 @param appKey AppKey on the toast console
 @param enableCrashReporter Whether or not sending crash is enabled
 @return A instance of ToastLoggerConfiguration
 */
+ (nullable instancetype)configurationWithAppKey:(NSString *)appKey
                             enableCrashReporter:(BOOL)enableCrashReporter;

/**
 Initialize a configuration with the given appKey, enableCrashReport and setting.

 @param appKey AppKey on the toast console
 @param enableCrashReporter Whether or not sending crash is enabled
 @param setting configuration setting about Toast Logger
 @return A instance of ToastLoggerConfiguration
 */
+ (nullable instancetype)configurationWithAppKey:(NSString *)appKey
                             enableCrashReporter:(BOOL)enableCrashReporter
                                         setting:(nullable ToastLoggerConfigurationSetting *)setting;

/**
 Initialize a configuration with the given appKey.
 
 @param appKey AppKey on the toast console
 @return A instance of ToastLoggerConfiguration
 */
- (nullable instancetype)initWithAppKey:(NSString *)appKey;

/**
 Initialize a configuration with the given appKey and enableCrashReport.
 
 @param appKey AppKey on the toast console
 @param enableCrashReporter Whether or not sending crash is enabled
 @return A instance of ToastLoggerConfiguration
 */
- (nullable instancetype)initWithAppKey:(NSString *)appKey
                    enableCrashReporter:(BOOL)enableCrashReporter;

/**
 Initialize a configuration with the given appKey, enableCrashReport and setting.
 
 @param appKey AppKey on the toast console
 @param enableCrashReporter Whether or not sending crash is enabled
 @param setting configuration setting about Toast Logger
 @return A instance of ToastLoggerConfiguration
 */
- (nullable instancetype)initWithAppKey:(NSString *)appKey
                    enableCrashReporter:(BOOL)enableCrashReporter
                                setting:(nullable ToastLoggerConfigurationSetting *)setting;

@end

NS_ASSUME_NONNULL_END
