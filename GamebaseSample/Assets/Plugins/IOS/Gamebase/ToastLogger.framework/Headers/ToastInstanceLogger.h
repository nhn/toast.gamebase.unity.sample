//
//  ToastInstanceLogger.h
//  ToastLogger
//
//  Created by Hyup on 2017. 9. 12..
//  Copyright © 2017년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "ToastLog.h"
#import "ToastLogFilter.h"
#import "ToastLoggerConfiguration.h"
#import "ToastLoggerDelegate.h"

NS_ASSUME_NONNULL_BEGIN

/**
 # ToastInstanceLogger
 
 The class responsible for sending log.
 */
@interface ToastInstanceLogger : NSObject

/// ---------------------------------
/// @name Properties
/// ---------------------------------

/** The delegate that following ToastLoggerDelegate protocol */
@property (weak, nonatomic, nullable) id<ToastLoggerDelegate> loggerDelegate;

/// ---------------------------------
/// @name Shared Instance
/// ---------------------------------

/**
 Gets the singletone instance of a ToastInstanceLogger.

 @param configuration Configuration about ToastLogger
 @return singletone The instance of a ToastInstanceLogger
 
 @note The parameter, configuration includes project key, crashEnabled and ToastLoggerConfigurationSetting
 
 */
+ (nullable ToastInstanceLogger *)sharedInstanceWithConfiguration:(nullable ToastLoggerConfiguration *)configuration;

/// ---------------------------------
/// @name Gets the Configuration
/// ---------------------------------

/**
 Gets the configuration set for the current instance.

 @return The configuration currently set
 */
- (nullable ToastLoggerConfiguration *)configuration;

/// ---------------------------------
/// @name Sets the user field
/// ---------------------------------

/**
 Sets the key and value of the user field to send when sending the log.

 @param value Value of the user field
 @param key Key of the user field
 */
- (void)setUserFieldWithValue:(nullable NSString *)value forKey:(NSString *)key;

#pragma mark - normal log
/// ---------------------------------
/// @name Normal Log
/// ---------------------------------

/**
 Sends a log message of level debug.

 @param message The message to send
 */
- (void)debug:(NSString *)message;
/**
 Sends a log message of level info.
 
 @param message The message to send
 */
- (void)info:(NSString *)message;
/**
 Sends a log message of level warn.
 
 @param message The message to send
 */
- (void)warn:(NSString *)message;
/**
 Sends a log message of level error.
 
 @param message The message to send
 */
- (void)error:(NSString *)message;
/**
 Sends a log message of level fatal.
 
 @param message The message to send
 */
- (void)fatal:(NSString *)message;

#pragma mark - normal log (messageFormat)
/// ---------------------------------
/// @name Normal Log (messageFormat)
/// ---------------------------------

/**
 Sends a log message with format of level debug.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
- (void)debugWithFormat:(NSString *)format, ...;

/**
 Sends a log message with format of level info.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
- (void)infoWithFormat:(NSString *)format, ...;

/**
 Sends a log message with format of level warn.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
- (void)warnWithFormat:(NSString *)format, ...;

/**
 Sends a log message with format of level error.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
- (void)errorWithFormat:(NSString *)format, ...;

/**
 Sends a log message with format of level fatal.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
- (void)fatalWithFormat:(NSString *)format, ...;

#pragma mark - normal log (userLogField)
/// ---------------------------------
/// @name Normal Log (userLogField)
/// ---------------------------------

/**
 Sends a log message with user fields of level debug.

 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)debug:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with user fields of level info.
 
 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)info:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with user fields of level warn.
 
 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)warn:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with user fields of level error.
 
 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)error:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with user fields of level fatal.
 
 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)fatal:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

#pragma mark - Type Setting log
/// ---------------------------------
/// @name Type Setting Log
/// ---------------------------------

/**
 Sends a log message with logType of level debug.

 @param logType Type of log
 @param message The message to send
 */
- (void)debugWithLogType:(NSString *)logType message:(NSString *)message;

/**
 Sends a log message with logType of level info.
 
 @param logType Type of log
 @param message The message to send
 */
- (void)infoWithLogType:(NSString *)logType message:(NSString *)message;

/**
 Sends a log message with logType of level warn.
 
 @param logType Type of log
 @param message The message to send
 */
- (void)warnWithLogType:(NSString *)logType message:(NSString *)message;

/**
 Sends a log message with logType of level error.
 
 @param logType Type of log
 @param message The message to send
 */
- (void)errorWithLogType:(NSString *)logType message:(NSString *)message;

/**
 Sends a log message with logType of level fatal.
 
 @param logType Type of log
 @param message The message to send
 */
- (void)fatalWithLogType:(NSString *)logType message:(NSString *)message;

#pragma mark - Type Setting log (userLogField)
/// ---------------------------------
/// @name Type Setting Log (userLogField)
/// ---------------------------------

/**
 Sends a log message with logType of level debug.

 @param logType Type of log
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
- (void)debugWithLogType:(NSString *)logType format:(NSString *)format, ...;

/**
Sends a log message with logType of level info.

@param logType Type of log
@param format The message string to send
@param ... A comma separated list of arguments to substitute into the format.
*/
- (void)infoWithLogType:(NSString *)logType format:(NSString *)format, ...;

/**
 Sends a log message with logType of level warn.
 
 @param logType Type of log
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
- (void)warnWithLogType:(NSString *)logType format:(NSString *)format, ...;

/**
 Sends a log message with logType of level error.
 
 @param logType Type of log
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
- (void)errorWithLogType:(NSString *)logType format:(NSString *)format, ...;

/**
 Sends a log message with logType of level fatal.
 
 @param logType Type of log
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
- (void)fatalWithLogType:(NSString *)logType format:(NSString *)format, ...;

#pragma mark - Type Setting log (messageFormat)
/// ---------------------------------
/// @name Type Setting Log (messageFormat)
/// ---------------------------------

/**
 Sends a log message with logType and user fields of level debug.

 @param logType Type of log
 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)debugWithLogType:(NSString *)logType message:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with logType and user fields of level info.
 
 @param logType Type of log
 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)infoWithLogType:(NSString *)logType message:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with logType and user fields of level warn.
 
 @param logType Type of log
 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)warnWithLogType:(NSString *)logType message:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with logType and user fields of level error.
 
 @param logType Type of log
 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)errorWithLogType:(NSString *)logType message:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with logType and user fields of level fatal.
 
 @param logType Type of log
 @param message The message to send
 @param userFields You have additional information to send.
 */
- (void)fatalWithLogType:(NSString *)logType message:(NSString *)message userFields:(nullable NSDictionary<NSString *, NSString *> *)userFields;

#pragma mark - User Log - private
/// ---------------------------------
/// @name User Log - private
/// ---------------------------------

/**
 Used to sends log generated manually.

 @param log ToastLog to send
 */
- (void)userLog:(nullable ToastLog *)log;

#pragma mark - Version
/// ---------------------------------
/// @name Version
/// ---------------------------------

/**
 Gets the ToastSDK version

 @return Version of ToastSDK
 */
+ (NSString *)version;

@end

NS_ASSUME_NONNULL_END
