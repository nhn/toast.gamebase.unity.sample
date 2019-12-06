//
//  ToastThreadSafeDictionary.h
//  ToastCommon
//
//  Created by JooHyun Lee on 01/04/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastDictionary.h"

NS_ASSUME_NONNULL_BEGIN

@interface ToastThreadSafeDictionary<__covariant KeyType, __covariant ObjectType> : ToastDictionary <KeyType, ObjectType>

@end


@interface ToastThreadSafeMutableDictionary<__covariant KeyType, __covariant ObjectType> : ToastMutableDictionary <KeyType, ObjectType>

@end

NS_ASSUME_NONNULL_END
