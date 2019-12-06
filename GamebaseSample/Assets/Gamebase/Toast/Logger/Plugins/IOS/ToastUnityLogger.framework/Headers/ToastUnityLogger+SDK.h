//
//  ToastUnityLogger.h
//  ToastUnityLogger
//
//  Created by JooHyun Lee on 2018. 2. 27..
//  Copyright © 2018년 NHNEnt. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <ToastUnityCore/ToastUnityCore.h>
#import <ToastLogger/ToastLogger.h>

@interface ToastUnityLogger : NSObject <ToastUnityModule>

+ (NSString *)version;

@end
