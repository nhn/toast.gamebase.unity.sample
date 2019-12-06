//
//  ToastGamebaseIAPConfiguration.h
//  ToastGamebaseIAP
//
//  Created by Hyup on 17/04/2019.
//  Copyright © 2019 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastGamebaseConstant.h"

NS_ASSUME_NONNULL_BEGIN

@interface ToastGamebaseIAPConfiguration : NSObject <NSCoding, NSCopying>

+ (instancetype)configurationWithAppKey:(NSString *)appKey
                            serviceZone:(ToastGamebaseServiceZone)serviceZone;

- (void)setExtraObject:(NSDictionary *)object forStore:(NSString *)store;

@end

NS_ASSUME_NONNULL_END
