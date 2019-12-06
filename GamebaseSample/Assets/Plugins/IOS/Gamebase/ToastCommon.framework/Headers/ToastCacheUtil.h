//
//  ToastCacheUtil.h
//  ToastCommon
//
//  Created by JooHyun Lee on 2018. 1. 5..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

/**
 # Cache Util
 
 A utility used for caching. It is used as a singleton object.
 */
@interface ToastCacheUtil : NSObject

/// ---------------------------------
/// @name Set & Get Methods
/// ---------------------------------

/**
 Cache the object in the memory with the key.

 @param object Object to cache.
 @param key Key to cache.
 */
+ (void)setObject:(id)object forKey:(id)key;

+ (void)setInteger:(NSInteger)value forKey:(id)key;

+ (void)setInt:(int)value forKey:(id)key;

+ (void)setFloat:(float)value forKey:(id)key;

+ (void)setDouble:(double)value forKey:(id)key;

+ (void)setBool:(BOOL)value forKey:(id)key;


/**
 Return the cached object in the memory.

 @param key The key of th cached object
 @return The object that is cached with key
 */
+ (id)objectForKey:(id)key;

+ (NSInteger)integerForKey:(id)key;

+ (int)intForKey:(id)key;

+ (float)floatForKey:(id)key;

+ (double)doubleForKey:(id)key;

+ (BOOL)boolForKey:(id)key;

/**
 Remove the cached object in memory.
 
 @param key The key of th cached object
 */
+ (void)removeObjectForKey:(id)key;

@end

NS_ASSUME_NONNULL_END
