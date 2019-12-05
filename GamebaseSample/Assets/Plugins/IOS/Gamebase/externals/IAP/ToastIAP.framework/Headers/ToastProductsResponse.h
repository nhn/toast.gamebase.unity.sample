//
//  ToastProductsResponse.h
//  ToastIAP
//
//  Created by JooHyun Lee on 2018. 9. 12..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastProduct.h"

NS_ASSUME_NONNULL_BEGIN

/**
 # ToastProductsResponse
 
 The response you receive as a result of the product listing inquiry([ToastIAP requestProductsWithCompletionHandler:]).
 
 */
@interface ToastProductsResponse : NSObject <NSCoding, NSCopying>

/// ---------------------------------
/// @name Properties
/// ---------------------------------

/** A list of products whose status is active in the IAP Console and can be viewed from AppConnect (iTunseConnect). */
@property (nonatomic, copy, readonly) NSArray<ToastProduct *> *products;

/** A list of products whose status is active in the IAP Console and can not be viewed by AppConnect(iTunseConnect). */
@property (nonatomic, copy, readonly) NSArray<ToastProduct *> *invalidProducts;

@end

NS_ASSUME_NONNULL_END
