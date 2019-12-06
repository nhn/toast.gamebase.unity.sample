//
//  ToastGamebaseProductsResponse.h
//  ToastGamebaseIAP
//
//  Created by Hyup on 18/04/2019.
//  Copyright © 2019 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "ToastGamebaseProduct.h"

NS_ASSUME_NONNULL_BEGIN

@interface ToastGamebaseProductsResponse : NSObject <NSCoding, NSCopying>
{
    NSArray<ToastGamebaseProduct *> *_products;
    NSArray<ToastGamebaseProduct *> *_invalidProducts;
}

@property (nonatomic, readonly, copy) NSArray<ToastGamebaseProduct *> *products;
@property (nonatomic, readonly, copy) NSArray<ToastGamebaseProduct *> *invalidProducts;

@end

NS_ASSUME_NONNULL_END
