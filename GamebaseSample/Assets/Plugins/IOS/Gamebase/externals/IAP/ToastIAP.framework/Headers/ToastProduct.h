//
//  ToastProduct.h
//  ToastIAP
//
//  Created by JooHyun Lee on 2018. 9. 12..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

/**
 The type of product.

 - ToastProductTypeUnknown: Unknown type
 - ToastProductTypeConsumable: Consumable type
 - ToastProductTypeAutoRenewableSubscription: Auto renewable subscription type
 - ToastProductTypeConsumableSubscription : Consumable subscription type
 */
typedef NS_ENUM(NSInteger, ToastProductType) {
    ToastProductTypeUnknown = 0,
    ToastProductTypeConsumable = 1,
    ToastProductTypeAutoRenewableSubscription = 2,
    ToastProductTypeConsumableSubscription = 3,
};

/**
 # ToastProduct
 
 The information of product.
 */
@interface ToastProduct : NSObject <NSCoding, NSCopying>

/// ---------------------------------
/// @name Properties
/// ---------------------------------

/** The identifier of product */
@property (nonatomic, copy, readonly) NSString *productIdentifier;

/** The sequence of product. */
@property (nonatomic, readonly) long productSeq;

/** The name of product */
@property (nonatomic, copy, readonly, nullable) NSString *productName;

/** The Type of product(consumable or auto-renewable-subscription) */
@property (nonatomic, readonly) ToastProductType productType;

/** The price of product */
@property (nonatomic, copy, readonly, nullable) NSDecimalNumber *price;

/** The currency of product */
@property (nonatomic, copy, readonly, nullable) NSString *currency;

/** The localized price of product */
@property (nonatomic, copy, readonly, nullable) NSString *localizedPrice;

/** If 'YES', the product is currently active. If 'NO', it is deactive. */
@property (nonatomic, readonly, getter=isActive) BOOL active;

/** The store code of purchase */
@property (nonatomic, copy, readonly) NSString *storeCode;

@end

NS_ASSUME_NONNULL_END
