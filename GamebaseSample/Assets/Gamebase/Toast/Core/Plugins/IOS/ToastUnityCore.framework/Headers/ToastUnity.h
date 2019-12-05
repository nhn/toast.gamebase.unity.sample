//
//  ToastUnity.h
//  ToastUnity
//
//  Created by JooHyun Lee on 2018. 2. 28..
//  Copyright © 2018년 NHNEnt. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastUnityAction.h"
#import "ToastUnityMessage.h"
#import "ToastNativeMessage.h"

@protocol ToastUnityModule

+ (void)registerActions;

@end

@interface ToastUnity : NSObject

+ (void)registerAction:(ToastUnityAction *)action;

+ (char *)didReceiveUnityMessage:(ToastUnityMessage *)unityMessage;

+ (void)sendNativeMessageToUnity:(ToastNativeMessage *)nativeMessage;

+ (NSString *)version;

@end
