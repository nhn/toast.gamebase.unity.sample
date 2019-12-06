//
//  ToastLoggerConfigurationSetting.h
//  ToastLogger
//
//  Created by JooHyun Lee on 07/03/2019.
//  Copyright © 2019 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <ToastCore/ToastCore.h>
#import <ToastCommon/ToastCommon.h>
#import "ToastLog.h"

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, ToastLoggerConfigurationType) {
    ToastLoggerConfigurationTypeDefault = 0,
    ToastLoggerConfigurationTypeConsole = 1,
    ToastLoggerConfigurationTypeUser = 2,
};

/**
 # ToastLoggerConfigurationSetting
 
 The configuration setting about ToastLogger.
 
 There are three kinds of settings.
 
 * default setting (Set to the default setting.)
 * console setting (Get and set the settings you set in the Toast Console.)
 * user setting (Set it as you want.)
 
 */
@interface ToastLoggerConfigurationSetting : NSObject <NSCoding, NSCopying>

@property (nonatomic, readonly) ToastLoggerConfigurationType configurationType;


#pragma mark - Log
/** Whether or not sending normal log is enabled. **/
@property (nonatomic, getter=isEnableNormalLog) BOOL enableNormalLog;

/** Whether or not sending crash log is enabled. **/
@property (nonatomic, getter=isEnableCrashLog) BOOL enableCrashLog;

/** Whether or not duplicate log filter is enabled. **/
@property (nonatomic, getter=isEnableLogDuplicateFilter) BOOL enableLogDuplicateFilter;


#pragma mark - Filter
/** The time to check for duplicate logs **/
@property (nonatomic, copy, nullable) NSNumber *filterLogDuplicateExpiredTime;

/** Whether or not log level filter is enabled. **/
@property (nonatomic, getter=isEnableLogLevelFilter) BOOL enableLogLevelFilter;

/** The log level to filter. **/
@property (nonatomic) ToastLogLevel filterLogLevel;

/** Whether or not log type filter is enabled. **/
@property (nonatomic, getter=isEnableLogTypeFilter) BOOL enableLogTypeFilter;

/** the log types to filter. The types in this array are not sent. **/
@property (nonatomic, copy, nullable) NSArray<NSString *> *filterLogType;


#pragma mark - Network Insight
@property (nonatomic, getter=isEnableNetworkInsight) BOOL enableNetworkInsight;

@property (nonatomic, copy, nullable) NSArray<NSString *> *networkInsightsURLs;

@property (nonatomic, copy, nullable) NSString *networkInsightsVersion;



/// ---------------------------------
/// @name Gets the setting
/// ---------------------------------

/**
 Gets the instance of ToastLoggerConfigurationSetting with the type set to the default.

 @return The instance of ToastLoggerConfigurationSetting with the type set to the default.
 */
+ (instancetype)defaultSetting;

/**
 Gets the instance of ToastLoggerConfigurationSetting with the type set to the console.
 
 @return The instance of ToastLoggerConfigurationSetting with the type set to the console.
 */
+ (instancetype)consoleSetting;


/**
 Gets the instance of ToastLoggerConfigurationSetting with the user's setting values.

 @param enableNormalLog If `YES`, enables sending normal log. If `NO`, disable it.
 @param enableCrashLog If `YES`, enables sending crash log. If `NO`, disable it.
 @param enableLogLevelFilter If `YES`, enables filtering by log level. If `NO`, disable it.
 @param filterLogLevel The log level to filter. (If this value set to 'warn', only warn, error and fatal can be sent)
 @param enableLogTypeFilter If `YES`, enables filtering by log type. If `NO`, disable it.
 @param filterLogTypeArray The log types to filter. The types in this array are not sent.
 @param enableLogDuplicateFilter If `YES`, enables filtering the same log. If `NO`, disable it.
 @param filterLogDuplicateExpiredTime Time to check for duplicate logs.(2 - 120 second)
 @return The instance of ToastLoggerConfigurationSetting with the type set to the user values.
 */
+ (instancetype)userSettingWithEnableNormalLog:(BOOL)enableNormalLog
                                enableCrashLog:(BOOL)enableCrashLog
                          enableLogLevelFilter:(BOOL)enableLogLevelFilter
                                filterLogLevel:(ToastLogLevel)filterLogLevel
                           enableLogTypeFilter:(BOOL)enableLogTypeFilter
                            filterLogTypeArray:(nullable NSArray *)filterLogTypeArray
                      enableLogDuplicateFilter:(BOOL)enableLogDuplicateFilter
                 filterLogDuplicateExpiredTime:(nullable NSNumber *)filterLogDuplicateExpiredTime;

/**
 Gets the instance of ToastLoggerConfigurationSetting with the user's setting values.
 
 @param enableNormalLog If `YES`, enables sending normal log. If `NO`, disable it.
 @param enableCrashLog If `YES`, enables sending crash log. If `NO`, disable it.
 @param enableLogLevelFilter If `YES`, enables filtering by log level. If `NO`, disable it.
 @param filterLogLevel The log level to filter. (If this value set to 'warn', only warn, error and fatal can be sent)
 @param enableLogTypeFilter If `YES`, enables filtering by log type. If `NO`, disable it.
 @param filterLogTypeArray The log types to filter. The types in this array are not sent.
 @param enableLogDuplicateFilter If `YES`, enables filtering the same log. If `NO`, disable it.
 @param filterLogDuplicateExpiredTime Time to check for duplicate logs.(2 - 120 second)
 @return The instance of ToastLoggerConfigurationSetting with the type set to the user values.
 */
- (instancetype)initWithEnableNormalLog:(BOOL)enableNormalLog
                         enableCrashLog:(BOOL)enableCrashLog
                   enableLogLevelFilter:(BOOL)enableLogLevelFilter
                         filterLogLevel:(ToastLogLevel)filterLogLevel
                    enableLogTypeFilter:(BOOL)enableLogTypeFilter
                     filterLogTypeArray:(nullable NSArray *)filterLogTypeArray
               enableLogDuplicateFilter:(BOOL)enableLogDuplicateFilter
          filterLogDuplicateExpiredTime:(nullable NSNumber *)filterLogDuplicateExpiredTime;

@end

NS_ASSUME_NONNULL_END
