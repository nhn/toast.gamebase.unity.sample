//
//  ToastGamebasePurchase.h
//  ToastGamebaseIAP
//
//  Created by Hyup on 17/04/2019.
//  Copyright © 2019 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "ToastGamebaseConstant.h"

NS_ASSUME_NONNULL_BEGIN

@interface ToastGamebasePurchase : NSObject <NSCoding, NSCopying>
{
    NSString *_storeCode;
    NSString *_productID;
    NSString *_productSequence;
    ToastGamebaseProductType _productType;
    NSDecimalNumber *_price;
    NSString *_priceCurrencyCode;
    NSString *_paymentSequence;
    NSString *_accessToken;
    NSString *_userID;
    BOOL _storePayment;    
    NSTimeInterval _purchaseTime;
    NSTimeInterval _expiryTime;
    NSString *_paymentID;
    NSString *_originalPaymentID;
    NSString *_payload;
    NSString *_gamebasePayload;
}


@property (nonatomic, readonly, copy) NSString *storeCode;
@property (nonatomic, readonly, copy) NSString *productID;
@property (nonatomic, readonly, copy) NSString *productSequence;
@property (nonatomic, readonly) ToastGamebaseProductType productType;
@property (nonatomic, readonly, copy) NSDecimalNumber *price;
@property (nonatomic, readonly, copy) NSString *priceCurrencyCode;
@property (nonatomic, readonly, copy) NSString *paymentSequence;
@property (nonatomic, readonly, copy) NSString *accessToken;
@property (nonatomic, readonly, copy) NSString *userID;
@property (nonatomic, readonly, getter=isStorePayment) BOOL storePayment;
@property (nonatomic, readonly) NSTimeInterval purchaseTime;
@property (nonatomic, readonly) NSTimeInterval expiryTime;
@property (nonatomic, readonly, copy, nullable) NSString *paymentID;
@property (nonatomic, readonly, copy, nullable) NSString *originalPaymentID;
@property (nonatomic, readonly, copy, nullable) NSString *payload;
@property (nonatomic, readonly, copy, nullable) NSString *gamebasePayload;

@end

NS_ASSUME_NONNULL_END
