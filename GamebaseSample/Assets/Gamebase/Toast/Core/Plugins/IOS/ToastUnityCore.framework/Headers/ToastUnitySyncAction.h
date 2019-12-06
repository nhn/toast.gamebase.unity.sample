//
//  ToastUnitySyncAction.h
//  ToastUnityCore
//
//  Created by JooHyun Lee on 2018. 3. 12..
//  Copyright © 2018년 NHNEnt. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastUnityAction.h"

@interface ToastUnitySyncAction : ToastUnityAction

- (ToastNativeMessage *)action:(ToastUnityMessage *)message;

@end
