//
//  ToastGamebaseIAP+SDK.h
//  ToastGamebaseIAP
//
//  Created by Hyup on 16/04/2019.
//  Copyright © 2019 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "ToastGamebaseIAPConfiguration.h"
#import "ToastGamebaseProduct.h"
#import "ToastGamebaseProductsResponse.h"


@class ToastGamebasePurchase;

typedef void (^ToastGamebasePurchaseHandler)(ToastGamebaseStoreCode _Nonnull store,
                                             BOOL isSuccess,
                                             ToastGamebasePurchase *_Nullable purchase,
                                             NSString *_Nonnull productID,
                                             NSError* _Nullable error);

@protocol ToastGamebaseInAppPurchaseDelegate <NSObject>

- (void)didReceivePurchaseResultForStore:(ToastGamebaseStoreCode _Nonnull)store
                               isSuccess:(BOOL)isSuccess
                                purchase:(ToastGamebasePurchase *_Nullable)purchase
                               productID:(NSString * _Nonnull)productID
                                   error:(NSError *_Nullable)error;
@end

NS_ASSUME_NONNULL_BEGIN

@interface ToastGamebaseIAP : NSObject

#pragma mark - initialize
+ (void)setDelegate:(id<ToastGamebaseInAppPurchaseDelegate>)delegate;

+ (void)initWithConfiguration:(ToastGamebaseIAPConfiguration *)configuration;

+ (void)initWithConfiguration:(ToastGamebaseIAPConfiguration *)configuration
                     delegate:(id<ToastGamebaseInAppPurchaseDelegate>)delegate;

#pragma mark - products
+ (void)requestProductsForStore:(ToastGamebaseStoreCode)store
          withCompletionHandler:(nullable void (^)(ToastGamebaseProductsResponse * _Nullable response, NSError * _Nullable error))completionHandler;

#pragma mark - consumable
+ (void)requestConsumablePurchasesForStore:(ToastGamebaseStoreCode)store
                     withCompletionHandler:(nullable void (^)(NSArray<ToastGamebasePurchase *> * _Nullable purchases, NSError * _Nullable error))completionHandler;

#pragma mark - purchase
+ (void)purchaseForStore:(ToastGamebaseStoreCode)store
                 product:(ToastGamebaseProduct *)product
   withCompletionHandler:(ToastGamebasePurchaseHandler)completionHandler;

+ (void)purchaseForStore:(ToastGamebaseStoreCode)store
               productID:(NSString *)productID 
   withCompletionHandler:(ToastGamebasePurchaseHandler)completionHandler;

#pragma mark - purchase - payload (payload is ToastIAP only)
+ (void)purchaseForStore:(ToastGamebaseStoreCode)store
               productID:(NSString *)productID
                 payload:(nullable NSString *)payload
   withCompletionHandler:(ToastGamebasePurchaseHandler)completionHandler;

+ (void)purchaseForStore:(ToastGamebaseStoreCode)store
                 product:(ToastGamebaseProduct *)product
                 payload:(nullable NSString *)payload
   withCompletionHandler:(ToastGamebasePurchaseHandler)completionHandler;

#pragma mark - purchase - payload, extra (payload(gamebasePayload) is ToastIAP only)
+ (void)purchaseForStore:(ToastGamebaseStoreCode)store
               productID:(NSString *)productID
                 payload:(nullable NSString *)payload
         gamebasePaylaod:(nullable NSString *)gamebasePayload
   withCompletionHandler:(ToastGamebasePurchaseHandler)completionHandler;

+ (void)purchaseForStore:(ToastGamebaseStoreCode)store
                 product:(ToastGamebaseProduct *)product
                 payload:(nullable NSString *)payload
         gamebasePaylaod:(nullable NSString *)gamebasePayload
   withCompletionHandler:(ToastGamebasePurchaseHandler)completionHandler;

#pragma mark - restore (ToastIAP Only)
+ (void)restoreForStore:(ToastGamebaseStoreCode)store
  withCompletionHandler:(nullable void (^)(NSArray<ToastGamebasePurchase *> * _Nullable purchases, NSError * _Nullable error))completionHandler;

#pragma mark - activatedPurchase (ToastIAP Only)
+ (void)requestActivatedPurchasesForStore:(ToastGamebaseStoreCode)store
                    withCompletionHandler:(nullable void (^)(NSArray<ToastGamebasePurchase *> * _Nullable purchases, NSError * _Nullable error))completionHandler;

#pragma mark - processes incomplete purchases  (ToastIAP Only - TCIAP -> ToastIAP)
+ (void)processesIncompletePurchasesForStore:(ToastGamebaseStoreCode)store
                       withCompletionHandler:(nullable void (^)(NSArray <ToastGamebasePurchase *> * _Nullable results, NSError * _Nullable error))completionHandler;

#pragma mark - Support Utils
+ (void)consumeForStore:(ToastGamebaseStoreCode)store
               purchase:(ToastGamebasePurchase *)purchase
  withCompletionHandler:(nullable void (^)(NSError * _Nullable error))completionHandler;

+ (void)setDebugMode:(BOOL)debugMode;

+ (NSString *)version;
@end

NS_ASSUME_NONNULL_END
