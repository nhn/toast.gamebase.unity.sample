//
//  ToastUnityMessage.h
//  ToastUnityCommon
//
//  Created by JooHyun Lee on 2018. 2. 28..
//  Copyright © 2018년 NHNEnt. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface ToastUnityMessage : NSObject

@property (nonatomic, readonly) NSDictionary *originalMessage;
@property (nonatomic, readonly, copy) NSString *uri;
@property (nonatomic, readonly, copy) NSString *transactionIdentifier;
@property (nonatomic, readonly, copy) NSString *callbackObject;
@property (nonatomic, readonly, copy) NSString *callbackMethod;
@property (nonatomic, readonly) NSDictionary *payload;

- (instancetype)initWithMessage:(NSString *)message;

@end
