//
//  ToastDictionary.h
//  ToastCommon
//
//  Created by JooHyun Lee on 01/04/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface ToastDictionary<__covariant KeyType, __covariant ObjectType> : NSDictionary <KeyType, ObjectType>

- (NSInteger)integerForKey:(nullable KeyType)aKey;

- (int)intForKey:(nullable KeyType)aKey;

- (long)longForKey:(nullable KeyType)aKey;

- (float)floatForKey:(nullable KeyType)aKey;

- (double)doubleForKey:(nullable KeyType)aKey;

- (BOOL)boolForKey:(nullable KeyType)aKey;

- (nullable ObjectType)objectForCaseInsensitiveKey:(nullable NSString *)aKey;

- (nullable ObjectType)objectForRecursiveKey:(nullable KeyType)aKey;

@end


@interface ToastMutableDictionary<__covariant KeyType, __covariant ObjectType> : ToastDictionary <KeyType, ObjectType>

@property (nonatomic, readonly) ObjectType defaultObject;

+ (instancetype)dictionaryWithDefaultObject:(ObjectType)defaultObject;

- (instancetype)initWithCapacity:(NSUInteger)capacity;

- (instancetype)initWithDefaultObject:(ObjectType)defaultObject;

- (void)setDefaultObject:(ObjectType)defaultObject;

- (void)setDictionary:(nullable NSDictionary<KeyType, ObjectType> *)otherDictionary;

- (void)setObject:(nullable ObjectType)anObject forKey:(nullable KeyType <NSCopying>)aKey;

- (void)setInteger:(NSInteger)integerValue forKey:(nullable KeyType <NSCopying>)aKey;

- (void)setInt:(int)intValue forKey:(nullable KeyType <NSCopying>)aKey;

- (void)setLong:(long)longValue forKey:(nullable KeyType <NSCopying>)aKey;

- (void)setFloat:(float)floatValue forKey:(nullable KeyType <NSCopying>)aKey;

- (void)setDouble:(double)doubleValue forKey:(nullable KeyType <NSCopying>)aKey;

- (void)setBool:(BOOL)boolValue forKey:(nullable KeyType <NSCopying>)aKey;

- (void)addEntriesFromDictionary:(nullable NSDictionary<KeyType, ObjectType> *)otherDictionary;

- (void)removeAllObjects;

- (void)removeObjectForKey:(nullable KeyType)aKey;

- (void)removeObjectsForKeys:(nullable NSArray<KeyType> *)keyArray;

@end

NS_ASSUME_NONNULL_END
