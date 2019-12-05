//
//  ToastPushButton.h
//  ToastPush
//
//  Created by JooHyun Lee on 2018. 11. 30..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UserNotifications/UserNotifications.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSUInteger, ToastPushButtonType) {
    ToastPushButtonTypeDismiss = 0,
    ToastPushButtonTypeOpenApp = 1,
    ToastPushButtonTypeOpenURL = 2,
    ToastPushButtonTypeReply = 3,
};

@interface ToastPushButton : NSObject <NSCoding, NSCopying>

@property (nonatomic, readonly) NSString *identifier;

@property (nonatomic, readonly) ToastPushButtonType buttonType;

@property (nonatomic, readonly) NSString *name;

@property (nonatomic, readonly, nullable) NSString *link;

@property (nonatomic, readonly, nullable) NSString *hint;

@property (nonatomic, readonly, nullable) NSString *submit;

@end

NS_ASSUME_NONNULL_END
