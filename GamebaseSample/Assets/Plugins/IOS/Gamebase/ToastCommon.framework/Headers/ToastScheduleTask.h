//
//  ToastScheduleTask.h
//  ToastPush
//
//  Created by JooHyun Lee on 2018. 12. 21..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface ToastScheduleTask : NSObject

@property (readonly, getter=isExecuting) BOOL executing;
@property (readonly, getter=isCancelled) BOOL cancelled;
@property (readonly, getter=isFinished) BOOL finished;

@property (nonatomic, readonly) NSTimeInterval timeInterval;
@property (nonatomic, readonly) NSTimeInterval remainingTimeInterval;

- (instancetype)initWithTimeInterval:(NSTimeInterval)timeInterval
                        executeBlock:(void (^) (void))executeBlock;

- (void)resume;

- (void)suspend;

- (void)cancel;

@end

NS_ASSUME_NONNULL_END
