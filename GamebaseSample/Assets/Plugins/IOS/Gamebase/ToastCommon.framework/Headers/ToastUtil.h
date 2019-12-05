//
//  ToastUtil.h
//  ToastCommon
//
//  Created by Hyup on 2017. 9. 13..
//  Copyright © 2017년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface ToastUtil : NSObject

extern NSString *const ToastUnknownString;
extern NSString *const ToastNotApplicableString;
extern NSString *const ToastDefaultDateFormat;

#pragma mark - Time Util
+ (uint64_t)unixEpochTime;

#pragma mark - Network Util
+ (NSString *)URLEncodedString:(NSString *)string;
+ (NSString *)URLEncodedStringWithData:(NSData *)data;

#pragma mark - Data Util 
+ (NSString *)hash_SHA1:(NSString *)str;
+ (NSString *)hash_MD5:(NSString *)str;

+ (NSString *)base64Encode:(id)sourc;
+ (NSData *)base64Decode:(NSString *)source;

+ (NSData *)AESGenerateInitializationVector;

+ (NSData *)AES128EncryptedDataWithKey:(NSData *)key source:(NSData *)source;
+ (NSData *)AES128DecryptedDataWithKey:(NSData *)key source:(NSData *)source;

+ (NSData *)AES128EncryptedDataWithKey:(NSData *)key source:(NSData *)source iv:(NSData *)iv;
+ (NSData *)AES128DecryptedDataWithKey:(NSData *)key source:(NSData *)source iv:(NSData *)iv;

+ (NSData *)AES128GenerateKey;

+ (NSData *)AES256EncryptedDataWithKey:(NSData*)key source:(NSData *)source;
+ (NSData *)AES256DecryptedDataWithKey:(NSData*)key source:(NSData *)source;

+ (NSData *)AES256EncryptedDataWithKey:(NSData *)key source:(NSData *)source iv:(NSData *)iv;
+ (NSData *)AES256DecryptedDataWithKey:(NSData *)key source:(NSData *)source iv:(NSData *)iv;

+ (NSData *)AES256GenerateKey;

+ (NSData *)zipCompress:(id)source;
+ (NSData *)zipDecompress:(NSData *)source;

#pragma mark - Search Util
+ (BOOL)containsValueWithKey:(NSString *)key value:(id)value sourceArray:(NSArray *)sourceArray;
+ (NSArray *)containsArrayValueWithKey:(NSString *)key value:(id)value sourceArray:(NSArray *)sourceArray;
+ (id)objectForCaseInsensitiveKey:(NSString *)key fromDictionary:(NSDictionary *)dictionary;
+ (id)objectForRecursiveKey:(NSString *)key fromDictionary:(NSDictionary *)dictionary;

#pragma mark - String Util
+ (NSString *)createFileName:(NSString *)name key:(NSString *)key;
+ (NSString *)emptyStringToUnknown:(NSString *)value;
+ (NSString *)nilToEmptyString:(NSString *)value;
+ (NSString *)emptyStringToNA:(NSString *)value;

+ (BOOL)isEmptyString:(NSString *)string;
+ (BOOL)isUnknownString:(NSString *)string;

+ (const char *)NSStringToCString:(NSString *)str;
+ (NSString *)CStringToNSString:(const char*)str;

+ (NSString *)hexStringFromData:(NSData *)sourceData;
+ (NSData *)dataFromHexString:(NSString *)string;

+ (const char *)createThreadLabel:(NSString *)threadName projectKey:(NSString *)projectKey;

#pragma mark - Object Util
+ (NSString *)stringWithJSONObject:(id)object;
+ (id)jsonStringToObject:(NSString *)jsonString;
+ (id)jsonDataToObject:(NSData *)data;

+ (BOOL)isEmptyObject:(id)object;
+ (BOOL)setObjectSafety:(id)object forKey:(NSString *)key in:(NSMutableDictionary *)dictionary;
+ (id)validateObject:(id)object withDefault:(id)defaultObject;

#pragma mark - Date Util
+ (NSString *)dateStringWithDate:(NSDate *)date format:(NSString *)format;
+ (NSString *)dateStringWithTimeInterval:(NSTimeInterval)timeInterval format:(NSString *)format;
+ (NSTimeInterval)timeIntervalWithUnixTimestamp:(long long)timestamp;

#pragma mark - Type Search Util
+ (BOOL)checkDictionaryInNSStringType:(NSDictionary *)dictionary;

#pragma mark - Class Check Util (use replection)
+ (BOOL)hasExternalSDKWithArray:(NSArray*)classNameArray;

#pragma mark - Create UUID
+ (NSString *)generateUUID;
+ (NSString *)generateUUIDRef;
+ (NSString *)UUIDWithLength:(NSUInteger)length;

#pragma mark - InternalField
+ (NSArray *)getCommonCollectionDataKey;
+ (NSArray *)getAppDetailCommonCollectionDataKey;
+ (NSArray *)getCrashDataKey;

#pragma mark - enum to String ToastLogLevel
+ (NSString *)logLevelToString:(NSInteger)level;
+ (NSInteger)logLevelStringToLogLevel:(NSString *)level;
+ (NSString *)serviceZoneToString:(NSInteger)serviceZone;

#pragma mark - Generate NSError
+ (NSError *)errorWithDomain:(NSString *)domain
                        code:(NSInteger)code
                 description:(NSString *)description
               failureReason:(NSString *)failureReason
          recoverySuggestion:(NSString *)recoverySuggestion;

+ (NSError *)errorWithDomain:(NSString *)domain
                        code:(NSInteger)code
                 description:(NSString *)format, ...;

+ (NSError *)errorWithDomain:(NSString *)domain
                        code:(NSInteger)code
                       error:(NSError *)error;

+ (NSError *)errorWithDomain:(NSString *)domain
                        code:(NSInteger)code
                 description:(NSString *)description
                       error:(NSError *)error;

#pragma mark - Thread
+ (BOOL)isMainThread;
+ (void)runOnMainThread:(void (^) (void))block;
+ (void)runOnWorkerThread:(void (^) (void))block;

#pragma mark - Version check
+ (BOOL)isVersion:(NSString *)version equalTo:(NSString *)targetVersion;
+ (BOOL)isVersion:(NSString *)version greaterThan:(NSString *)targetVersion;
+ (BOOL)isVersion:(NSString *)version greaterThanOrEqualTo:(NSString *)targetVersion;
+ (BOOL)isVersion:(NSString *)version lessThan:(NSString *)targetVersion;
+ (BOOL)isVersion:(NSString *)version lessThanOrEqualTo:(NSString *)targetVersion;

@end
