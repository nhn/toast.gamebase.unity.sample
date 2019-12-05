//
//  ToastPurchaseResult.h
//  ToastIAP
//
//  Created by JooHyun Lee on 2018. 9. 12..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastProduct.h"
#import "ToastIAPConfiguration.h"

NS_ASSUME_NONNULL_BEGIN

// --no-merge-categories \

/**
 # ToastPurchaseResult
 
 The result of purchase.
 
 It used in
 
 * [ToastIAP requestConsumablePurchasesWithCompletionHandler:]
 * [ToastIAP restoreWithCompletionHandler:]
 * [ToastIAP requestActivePurchasesWithCompletionHandler:]
 * [ToastIAP consumeWithPurchaseResult:completionHandler:]
 * [ToastInAppPurchaseDelegate didReceivePurchaseResult:]
 
 */
@interface ToastPurchaseResult : NSObject <NSCoding, NSCopying>

/// ---------------------------------
/// @name Properties
/// ---------------------------------

/** The userID of purchase */
@property (nonatomic, copy, readonly) NSString *userID;

/** The store code of purchase */
@property (nonatomic, copy, readonly) NSString *storeCode;

/** The product identifier of purchase */
@property (nonatomic, copy, readonly) NSString *productIdentifier;

/** The product sequence of purchase */
@property (nonatomic, readonly) long productSeq;

/** The product type of purchase */
@property (nonatomic, readonly) ToastProductType productType;

/** The price of purchase */
@property (nonatomic, copy, readonly) NSDecimalNumber *price;

/** The currency of purchse */
@property (nonatomic, copy, readonly) NSString *currency;

/** The payment sequence of purchase(Issued from the IAP server) */
@property (nonatomic, copy, readonly) NSString *paymentSeq;

/** The access token of purchase */
@property (nonatomic, copy, readonly) NSString *accessToken;

/** The appstore transaction identifier of purchase */
@property (nonatomic, copy, readonly) NSString *transactionIdentifier;

/** The original transaction identifier of purchase(In case of auto-renewable-subscription) */
@property (nonatomic, copy, readonly, nullable) NSString *originalTransactionIdentifier;

/** The purchase time of purchase */
@property (nonatomic, readonly) NSTimeInterval purchaseTime;

/** The expiry time of purchse */
@property (nonatomic, readonly) NSTimeInterval expiryTime;

/** Is the payment added from AppStore */
@property (nonatomic, readonly, getter=isStorePayment) BOOL storePayment;

/** The developer's payload of purchase */
@property (nonatomic, readonly, copy, nullable) NSString *payload;

@end

NS_ASSUME_NONNULL_END
