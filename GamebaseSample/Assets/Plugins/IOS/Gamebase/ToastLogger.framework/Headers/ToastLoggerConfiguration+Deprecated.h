//
//  ToastLoggerConfiguration+Deprecated.h
//  ToastLogger
//
//  Created by JooHyun Lee on 07/03/2019.
//  Copyright © 2019 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastLoggerConfiguration.h"

@interface ToastLoggerConfiguration (Deprecated)

/** Log & Crasyh Search project key on the toast console */
@property (strong, nonatomic, readonly) NSString *projectKey __deprecated_msg("use appKey instead.");

/**
 Initialize a configuration with the given projectKey.
 
 @param projectKey Project key on the toast console
 @return A instance of ToastLoggerConfiguration
 */
+ (instancetype)configurationWithProjectKey:(NSString *)projectKey __deprecated_msg("use configurationWithAppKey: instead.");

/**
 Initialize a configuration with the given projectKey and enableCrashReport.
 
 @param projectKey Project key on the toast console
 @param enableCrashReporter Whether or not sending crash is enabled
 @return A instance of ToastLoggerConfiguration
 */
+ (instancetype)configurationWithProjectKey:(NSString *)projectKey
                        enableCrashReporter:(BOOL)enableCrashReporter __deprecated_msg("use configurationWithAppKey:enableCrashReporter: instead.");

/**
 Initialize a configuration with the given projectKey, enableCrashReport and setting.
 
 @param projectKey Project key on the toast console
 @param enableCrashReporter Whether or not sending crash is enabled
 @param setting configuration setting about Toast Logger
 @return A instance of ToastLoggerConfiguration
 */
+ (instancetype)configurationWithProjectKey:(NSString *)projectKey
                        enableCrashReporter:(BOOL)enableCrashReporter
                                    setting:(ToastLoggerConfigurationSetting *)setting __deprecated_msg("use configurationWithAppKey:enableCrashReporter:setting: instead.");

/**
 Initialize a configuration with the given projectKey, enableCrashReport and serviceZone.
 
 @param projectKey Project key on the toast console
 @param enableCrashReporter Whether or not sending crash is enabled
 @param serviceZone service zone of the toast console
 @return A instance of ToastLoggerConfiguration
 */
+ (instancetype)configurationWithProjectKey:(NSString *)projectKey
                        enableCrashReporter:(BOOL)enableCrashReporter
                                serviceZone:(ToastServiceZone)serviceZone __deprecated_msg("use configurationWithAppKey:enableCrashReporter:serviceZone: instead.");

/**
 Initialize a configuration with the given projectKey, enableCrashReport, setting and serviceZone.
 
 @param projectKey Project key on the toast console
 @param enableCrashReporter Whether or not sending crash is enabled
 @param setting configuration setting about Toast Logger
 @param serviceZone service zone of the toast console
 @return A instance of ToastLoggerConfiguration
 */
+ (instancetype)configurationWithProjectKey:(NSString *)projectKey
                        enableCrashReporter:(BOOL)enableCrashReporter
                                    setting:(ToastLoggerConfigurationSetting *)setting
                                serviceZone:(ToastServiceZone)serviceZone __deprecated_msg("use configurationWithAppKey:enableCrashReporter:setting:serviceZone: instead.");

#pragma mark - ServiceZone
/// ---------------------------------
/// @name Set & Get service zone
/// ---------------------------------

/**
 Sets service zone of the toast console.
 
 @param serviceZone service zone of the toast console.(Real or Alpha or Beta)
 */
- (void)setLoggerServiceZone:(ToastServiceZone)serviceZone __deprecated_msg("use setServiceZone: instead.");

/**
 Gets service zone of the toast console.
 
 @return service zone of the toast console.(Real or Alpha or Beta)
 */
- (ToastServiceZone)loggerServiceZone __deprecated_msg("use serviceZone instead.");

@end
