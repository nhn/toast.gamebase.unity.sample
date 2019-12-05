//
//  ToastPushServiceExtension.h
//  ToastPush
//
//  Created by JooHyun Lee on 2018. 11. 30..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UserNotifications/UserNotifications.h>

NS_ASSUME_NONNULL_BEGIN


/**
 # ToastPushServiceExtension
 
 Support from iOS 10.0+.
 
 You must create and set a NotificationServiceExtension in your application to receive rich message and to collect received event.
 
 
 ## NotificationServiceExtension configuration
 
     #import <UserNotifications/UserNotifications.h>
     #import <ToastPush/ToastPush.h>
 
     @interface NotificationService : ToastPushServiceExtension
 
     @end
 
 */
API_AVAILABLE(ios(10.0))
@interface ToastPushServiceExtension : UNNotificationServiceExtension

@end

NS_ASSUME_NONNULL_END
