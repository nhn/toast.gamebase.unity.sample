//
//  TCGBLogger.h
//  Gamebase
//
//  Created by NHNEnt on 30/05/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#ifndef TCGBLogger_h
#define TCGBLogger_h

#import <Foundation/Foundation.h>
#import "TCGBError.h"
#import "TCGBConstants.h"
#import "TCGBLog.h"
#import "TCGBLoggerDelegate.h"
#import "TCGBLogFilter.h"
#import "TCGBLoggerConfiguration.h"

@interface TCGBLogger : NSObject

/**
 Initialize the singletone instance of a TCGBLogger.
 
 @param configuration Log & Crash Configuration.
 */
+ (void)initializeWithConfiguration:(TCGBLoggerConfiguration *)configuration;

/**
 Initialize the singletone instance of a TCGBLogger with a configuration in Info.plist.
 Keys are TCGBLoggerAppKey, TCGBLoggerEnableCrashReporter and TCGBLoggerServiceZone.
 */
+ (void)initializeWithCompletion:(void (^)(TCGBError *))completion;

/**
 Sets a TCGBLoggerDelegate.
 
 @param delegate The delegate following TCGBLoggerDelegate protocol
 */
+ (void)setDelegate:(id<TCGBLoggerDelegate>)delegate;

/**
 Sets the key and value of the user field to send when sending the log.
 
 @param value Value of the user field
 @param key Key of the user field
 */
+ (void)setUserFieldWithValue:(NSString *)value forKey:(NSString *)key;

/**
 Sets the handler to be executed after a crash.
 
 @param handler The handler to be executed after a crash
 */
+ (void)setCrashHandler:(void (^)(void))handler;

/**
 Sends a log message of level debug.
 
 @param message The message to send
 */
+ (void)debug:(NSString *)message;

/**
 Sends a log message of level info.
 
 @param message The message to send
 */
+ (void)info:(NSString *)message;

/**
 Sends a log message of level warn.
 
 @param message The message to send
 */
+ (void)warn:(NSString *)message;

/**
 Sends a log message of level error.
 
 @param message The message to send
 */
+ (void)error:(NSString *)message;

/**
 Sends a log message of level fatal.
 
 @param message The message to send
 */
+ (void)fatal:(NSString *)message;

/**
 Sends a log message with user fields of level debug.
 
 @param message The message to send
 @param userFields You have additional information to send.
 */
+ (void)debug:(NSString *)message userFields:(NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with user fields of level info.
 
 @param message The message to send
 @param userFields You have additional information to send.
 */
+ (void)info:(NSString *)message userFields:(NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with user fields of level warn.
 
 @param message The message to send
 @param userFields You have additional information to send.
 */
+ (void)warn:(NSString *)message userFields:(NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with user fields of level error.
 
 @param message The message to send
 @param userFields You have additional information to send.
 */
+ (void)error:(NSString *)message userFields:(NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with user fields of level fatal.
 
 @param message The message to send
 @param userFields You have additional information to send.
 */
+ (void)fatal:(NSString *)message userFields:(NSDictionary<NSString *, NSString *> *)userFields;

/**
 Sends a log message with format of level debug.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
+ (void)debugWithFormat:(NSString *)format, ...;

/**
 Sends a log message with format of level info.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
+ (void)infoWithFormat:(NSString *)format, ...;

/**
 Sends a log message with format of level warn.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
+ (void)warnWithFormat:(NSString *)format, ...;

/**
 Sends a log message with format of level error.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
+ (void)errorWithFormat:(NSString *)format, ...;

/**
 Sends a log message with format of level fatal.
 
 @param format The message string to send
 @param ... A comma separated list of arguments to substitute into the format.
 */
+ (void)fatalWithFormat:(NSString *)format, ...;

@end

#endif /* TCGBLogger_h */
