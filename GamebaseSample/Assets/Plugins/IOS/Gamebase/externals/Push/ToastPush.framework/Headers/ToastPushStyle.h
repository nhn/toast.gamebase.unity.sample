//
//  ToastPushStyle.h
//  ToastPush
//
//  Created by JooHyun Lee on 08/10/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface ToastPushStyle : NSObject <NSCoding, NSCopying>

@property (nonatomic, readonly, getter=isUseHtml) BOOL useHtml;

@end

NS_ASSUME_NONNULL_END
