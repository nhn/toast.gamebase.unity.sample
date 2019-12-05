//
//  ToastPushMedia.h
//  ToastPush
//
//  Created by JooHyun Lee on 2018. 11. 30..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSUInteger, ToastPushMediaType) {
    ToastPushMediaTypeImage,
    ToastPushMediaTypeVideo,
    ToastPushMediaTypeAudio,
};


@interface ToastPushMedia : NSObject <NSCoding, NSCopying>

@property (nonatomic, readonly) ToastPushMediaType mediaType;

@property (nonatomic, readonly) NSString *source;

@end

NS_ASSUME_NONNULL_END
