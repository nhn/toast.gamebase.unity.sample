//
//  ToastArray.h
//  ToastCommon
//
//  Created by JooHyun Lee on 01/04/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface ToastArray<__covariant ObjectType> : NSArray <ObjectType>

- (NSInteger)integerAtIndex:(NSUInteger)index;

- (int)intAtIndex:(NSUInteger)index;

- (long)longAtIndex:(NSUInteger)index;

- (float)floatAtIndex:(NSUInteger)index;

- (double)doubleAtIndex:(NSUInteger)index;

- (BOOL)boolAtIndex:(NSUInteger)index;

@end


@interface ToastMutableArray<__covariant ObjectType> : ToastArray <ObjectType>

@property (nonatomic, readonly) ObjectType defaultObject;

+ (instancetype)arrayWithDefaultObject:(ObjectType)defaultObject;

- (instancetype)initWithCapacity:(NSUInteger)capacity;

- (instancetype)initWithDefaultObject:(ObjectType)defaultObject;

- (void)setDefaultObject:(ObjectType)defaultObject;

- (void)setArray:(nullable NSArray<ObjectType> *)otherArray;

- (void)addObject:(nullable ObjectType)anObject;

- (void)addInteger:(NSInteger)integerValue;

- (void)addInt:(int)intValue;

- (void)addLong:(long)longValue;

- (void)addFloat:(float)floatValue;

- (void)addDouble:(double)doubleValue;

- (void)addBool:(BOOL)boolValue;

- (void)addObjectsFromArray:(nullable NSArray<ObjectType> *)otherArray;

- (void)insertObject:(nullable ObjectType)anObject atIndex:(NSUInteger)index;

- (void)insertObjects:(nullable NSArray<ObjectType> *)objects atIndexes:(NSIndexSet *)indexes;

- (void)removeLastObject;

- (void)removeAllObjects;

- (void)removeObject:(nullable ObjectType)anObject;

- (void)removeObjectAtIndex:(NSUInteger)index;

- (void)removeObjectsAtIndexes:(nullable NSIndexSet *)indexes;

- (void)removeObjectsInArray:(nullable NSArray<ObjectType> *)otherArray;

- (void)replaceObjectAtIndex:(NSUInteger)index withObject:(nullable ObjectType)anObject;

- (void)replaceObjectsAtIndexes:(nullable NSIndexSet *)indexes withObjects:(nullable NSArray<ObjectType> *)objects;

@end


NS_ASSUME_NONNULL_END
