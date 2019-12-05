//
//  ToastAsyncTask.h
//  ToastCommon
//
//  Created by JooHyun Lee on 03/06/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface ToastAsyncTask : NSObject

@property (readonly, getter=isExecuting) BOOL executing;
@property (readonly, getter=isCancelled) BOOL cancelled;
@property (readonly, getter=isFinished) BOOL finished;

- (instancetype)initWithCompletionHandler:(nullable void (^) (void))completionHandler;

- (void)resume;

- (void)suspend;

- (void)cancel;

- (void)finish;

@end

NS_ASSUME_NONNULL_END
