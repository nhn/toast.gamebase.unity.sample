//
//  ToastScheduleWorker.h
//  ToastCommon
//
//  Created by JooHyun Lee on 2018. 5. 10..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface ToastScheduleWorker : NSObject

@property (readonly, getter=isExecuting) BOOL executing;
@property (readonly) NSUInteger executeCount;

@property (nonatomic, readonly) NSTimeInterval timeInterval;
@property (nonatomic, readonly) NSTimeInterval remainingTimeIntervalUntilNextExecute;
@property (nonatomic, readonly) NSTimeInterval lastExecuteTimeInterval;

- (instancetype)initWithTimeInterval:(NSTimeInterval)timeInterval
                        executeBlock:(void (^) (void))executeBlock;

- (void)resume;

- (void)suspend;

- (void)reset;

@end

NS_ASSUME_NONNULL_END
