//
//  TCGBPurchaseIAPCommon.h
//  TCGBPurchaseIAPAdapter
//
//  Created by NHN on 2016. 6. 7..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <Gamebase/Gamebase.h>
#import <ToastIAP/ToastIAP.h>
#import <ToastGamebaseIAP/ToastGamebaseIAP.h>

#define GamebasePurchaseIAPAdapterVersion @"2.6.0"

@interface TCGBPurchaseIAPCommon : NSObject <TCGBPurchasable, ToastGamebaseInAppPurchaseDelegate>

@property (nonatomic, weak) id<TCGBPurchasePromotionIAPDelegate> promotionDelegate;

#pragma mark - protocol TCGBPurchasable
- (void)initializePurchaseWithAppID:(NSString *)appID store:(NSString *)store userID:(NSString *)userId enableDebugMode:(BOOL)isDebugMode;

- (void)requestPurchaseWithItemSeq:(long)itemSeq viewController:(UIViewController *)viewController completion:(void(^)(TCGBPurchasableReceipt *purchasableReceipt, TCGBError *error))completion;

- (void)requestPurchaseWithMarketItemId:(NSString *)productId viewController:(UIViewController *)viewController completion:(void(^)(TCGBPurchasableReceipt *purchasableReceipt, TCGBError *error))completion;

- (void)requestItemListOfNotConsumedWithCompletion:(void(^)(NSArray<TCGBPurchasableReceipt *> *purchasableReceiptArray, TCGBError *error))completion;

- (void)requestRetryTransactionWithCompletion:(void(^)(TCGBPurchasableRetryTransactionResult *transactionResult, TCGBError *error))completion;

- (void)requestItemListPurchasableWithCompletion:(void(^)(NSArray<TCGBPurchasableItem *> *purchasableItemArray, TCGBError *error))completion;

- (void)requestItemListAtIAPConsoleWithCompletion:(void(^)(NSArray<TCGBPurchasableItem *> *purchasableItemArray, TCGBError *error))completion;

- (void)requestActivatedPurchasesWithCompletion:(void(^)(NSArray<TCGBPurchasableReceipt *> *purchasableReceiptArray, TCGBError *error))completion;

- (void)requestRestoreWithCompletion:(void(^)(NSArray<TCGBPurchasableReceipt *> *purchasableReceiptArray, TCGBError *error))completion;

+ (NSString *)versionString;

@end
