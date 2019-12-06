//
//  ToastThreadSafeArray.h
//  ToastCommon
//
//  Created by JooHyun Lee on 01/04/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastArray.h"

NS_ASSUME_NONNULL_BEGIN

@interface ToastThreadSafeArray<__covariant ObjectType> : ToastArray <ObjectType>

@end


@interface ToastThreadSafeMutableArray<__covariant ObjectType> : ToastMutableArray <ObjectType>

@end

NS_ASSUME_NONNULL_END
