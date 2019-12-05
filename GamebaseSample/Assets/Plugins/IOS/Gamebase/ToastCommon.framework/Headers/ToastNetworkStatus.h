//
//  ToastNetworkStatus.h
//  ToastCommon
//
//  Created by JooHyun Lee on 2018. 6. 29..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <SystemConfiguration/SystemConfiguration.h>

typedef NS_ENUM(NSUInteger, ToastNetworkType) {
    ToastNetworkTypeNone,
    ToastNetworkTypeWIFI,
    ToastNetworkTypeWWAN,
};

@interface ToastNetworkStatus : NSObject

@property (nonatomic, readonly, getter=isConnected) BOOL connected;
@property (nonatomic, readonly) ToastNetworkType type;

@end
