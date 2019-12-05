//
//  ToastTimeoutTask.h
//  ToastCommon
//
//  Created by JooHyun Lee on 12/04/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface ToastTimeoutTask : NSObject

@property (readonly, getter=isExecuting) BOOL executing;
@property (readonly, getter=isCancelled) BOOL cancelled;
@property (readonly, getter=isFinished) BOOL finished;

@property (nonatomic, readonly) NSTimeInterval timeoutInterval;

- (instancetype)initWithTimeoutInterval:(NSTimeInterval)timeoutInterval
                         timeoutHandler:(nullable void (^) (void))timeoutHandler;

- (void)resume;

- (void)suspend;

- (void)cancel;

- (void)finish;

@end

NS_ASSUME_NONNULL_END
