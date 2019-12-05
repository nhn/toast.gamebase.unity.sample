//
//  ToastNativeMessage.h
//  ToastUnityCore
//
//  Created by JooHyun Lee on 2018. 3. 9..
//  Copyright © 2018년 NHNEnt. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastUnityMessage.h"

@interface ToastNativeMessage : NSObject

@property (nonatomic, readonly, copy) NSString *uri;
@property (nonatomic, readonly, copy) NSString *transactionIdentifier;
@property (nonatomic, readonly) NSDictionary *body;
@property (nonatomic, readonly, copy) NSString *objectName;
@property (nonatomic, readonly, copy) NSString *methodName;
@property (nonatomic, readonly, copy) NSError *error;

- (instancetype)initWithUnityMessage:(ToastUnityMessage *)message;

- (instancetype)initWithUnityMessage:(ToastUnityMessage *)message
                                body:(NSDictionary *)body;

- (instancetype)initWithUnityMessage:(ToastUnityMessage *)message
                               error:(NSError *)error;

- (instancetype)initWithURI:(NSString *)uri
                 objectName:(NSString *)objectName
                 methodName:(NSString *)methodName;

- (instancetype)initWithURI:(NSString *)uri
                 objectName:(NSString *)objectName
                 methodName:(NSString *)methodName
                       body:(NSDictionary *)body;

- (instancetype)initWithURI:(NSString *)uri
                 objectName:(NSString *)objectName
                 methodName:(NSString *)methodName
                      error:(NSError *)error;

- (instancetype)initWithURI:(NSString *)uri
                 objectName:(NSString *)objectName
                 methodName:(NSString *)methodName
                       body:(NSDictionary *)body
                      error:(NSError *)error;

- (void)setTransactionIdentifier:(NSString *)transactionIdentifier;

- (void)addBodyWithValue:(id)value forKey:(NSString *)key;

- (char *)UTF8String;

@end
