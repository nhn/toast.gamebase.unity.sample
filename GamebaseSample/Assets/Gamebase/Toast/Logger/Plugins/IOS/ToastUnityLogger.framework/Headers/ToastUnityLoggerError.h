//
//  ToastUnityLoggerError.h
//  ToastUnityLogger
//
//  Created by JooHyun Lee on 2018. 11. 13..
//  Copyright © 2018년 NHNEnt. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger, ToastUnityLoggerErrorCode) {
    ToastUnityLoggerErrorLoggerNotInitialized = 20000,  // Logger 초기화 되지 않음
    ToastUnityLoggerErrorUserFieldKeyInvalid = 20002,   // 잘못된 UserKey 혹은 UserValue 입력
    ToastUnityLoggerErrorCrashReportNotFound = 20003,   // PLCrashReport 링크되지 않음
};
