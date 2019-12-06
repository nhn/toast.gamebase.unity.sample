//
//  ToastPushAction.h
//  ToastPush
//
//  Created by JooHyun Lee on 25/06/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UserNotifications/UserNotifications.h>
#import "ToastPushMessage.h"

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, ToastPushActionType) {
    ToastPushActionDismiss = 0,
    ToastPushActionOpenApp = 1,
    ToastPushActionOpenURL = 2,
    ToastPushActionReply = 3,
};

@interface ToastPushAction : NSObject <NSCoding, NSCopying>

@property (nonatomic, readonly) NSString *actionIdentifier;

@property (nonatomic, readonly) NSString *categoryIdentifier;

@property (nonatomic, readonly) ToastPushActionType actionType;

@property (nonatomic, readonly, nullable) ToastPushButton *button;

@property (nonatomic, readonly, nullable) ToastPushMessage *message;

@property (nonatomic, readonly, nullable) NSString *userText;

@end

NS_ASSUME_NONNULL_END
