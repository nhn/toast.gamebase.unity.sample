//
//  ToastUnityError.h
//  ToastUnityCore
//
//  Created by JooHyun Lee on 2018. 8. 13..
//  Copyright © 2018년 NHNEnt. All rights reserved.
//

#import <Foundation/Foundation.h>

static NSString *const ToastUnityErrorDomain = @"com.toast.unity.core";

typedef NS_ENUM(NSInteger, ToastUnityErrorCode) {
    ToastUnityErrorParameterInvalid = 10000,    // 요청 파라미터 오류
    ToastUnityErrorCallbackInvalid = 10007,     // 콜백 오류
    ToastUnityErrorActionNotFound = 60000,      // URI 에 해당하는 Action 을 찾지 못함
    ToastUnityErrorUnknown = 99999,             // 알수 없는 오류
};
