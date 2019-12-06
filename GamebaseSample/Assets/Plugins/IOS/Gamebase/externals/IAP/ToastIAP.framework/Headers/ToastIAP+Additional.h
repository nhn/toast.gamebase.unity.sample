//
//  ToastIAP+Additional.h
//  ToastIAP
//
//  Created by JooHyun Lee on 2018. 9. 27..
//  Copyright © 2018년 NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastIAP+SDK.h"

NS_ASSUME_NONNULL_BEGIN

/**
 # ToastIAP + Additional
 
 SDK for compatibility with previous IAP SDK.
 */
@interface ToastIAP (Additional)

/// ---------------------------------
/// @name Purchase & Consume
/// ---------------------------------

// 상품 구매
/**
 Purchase a product by only identifier. The purchase result is returned to the delegate.
 
 @param productIdentifier The product identifier to purchase
 */
+ (void)purchaseWithProductIdentifier:(NSString *)productIdentifier;

/**
 Purchase a product by only identifier. The purchase result is returned to the delegate.
 
 @param productIdentifier The product identifier to purchase
 @param payload The developer's purchase payload
 */
+ (void)purchaseWithProductIdentifier:(NSString *)productIdentifier payload:(nullable NSString *)payload;

// 소모성 상품 소비
/**
 An additional API that allows Consume to be performed in the SDK.

 @param result The result of purchase
 @param completionHandler The handler to execute after consume
 */
+ (void)consumeWithPurchaseResult:(ToastPurchaseResult *)result
                completionHandler:(nullable void (^)(NSError * _Nullable error))completionHandler;


/// ---------------------------------
/// @name Reprocessing
/// ---------------------------------

// 미완료 결제건 재처리 - (구) IAP SDK 호환
/**
 Ability to process items stored in LoaclDB generated by IAP SDK
 
 @param completionHandler The handler to execute after the process is complete
 
 @note Used when changing SDK from from IAP SDK to ToastIAP SDK
 @note If the item purchased from the IAP SDK is not completed, the ToastIAP SDK processes the obsolete items in the LocalDB.
 @note Need to add libsqlite3.tdb to Linked Framework
 */
+ (void)processesIncompletePurchasesWithCompletionHandler:(nullable void (^)(NSArray <ToastPurchaseResult *> * _Nullable results, NSError * _Nullable error))completionHandler;

@end

NS_ASSUME_NONNULL_END