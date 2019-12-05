//
//  TCGBPurchasable.h
//  Gamebase
//
//  Created by NHN on 2016. 12. 7..
//  © NHN Corp. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "TCGBError.h"

@class TCGBPurchasableItem;
@class TCGBPurchasableReceipt;
@class TCGBPurchasableRetryTransactionResult;

/** The protocol TCGBPurchasePromotionIAPDelegate is for developing IAP Adapter.<br/>
 This protocol contains methods to receive result of promotion purchase.
 */
@protocol TCGBPurchasePromotionIAPDelegate <NSObject>

- (void)didReceivePurchasableReceipt:(TCGBPurchasableReceipt *)purchasableReceipt error:(TCGBError *)error;

@end

/** The protocol TCGBPurchasable is for developing IAP Adapter.<br/>
 This protocol contains several methods such as to request item list, to puchasing item and retrying incompleted purchasing.
 */
@protocol TCGBPurchasable


/**---------------------------------------------------------------------------------------
 * @name Initilaize IAP Adapter
 *  ---------------------------------------------------------------------------------------
 */

/** This method ininialize the IAP Adapter class.
 
 @param appID       appID is ToastCloud IAP's appID, not ToastCloud projectID.
 @param store       store should be appstore. Others will be ignored.
 @param userID      userID is obtained after logged in. This must be unique cause of tracing purchasing.
 @param isDebugMode isDebugMode is for setting debug mode of ToastCloud IAP module.
 */
@optional
- (void)initializePurchaseWithAppID:(NSString *)appID store:(NSString *)store userID:(NSString *)userID enableDebugMode:(BOOL)isDebugMode;


@required
/**---------------------------------------------------------------------------------------
 * @name Request Item List
 *  ---------------------------------------------------------------------------------------
 */

/** This is the primary method for obtaining ItemList which is registered at ToastCloud IAP Console and Apple Itunes Connect.
 
 Request a item list which is purchasable. This list has items which are registered in both Market(AppStore) and ToastCloud IAP Console.
 
 @param completion      completion may return the NSArray of TCGBPurchasableItem.<br/>
 If there is an error, TCGBError will be returned.
 @warning   You should call this method after ```logged in```, otherwise you will get **TCGB_ERROR_NOT_LOGGED_IN** error in the completion.
 */
- (void)requestItemListPurchasableWithCompletion:(void(^)(NSArray<TCGBPurchasableItem *> *purchasableItemArray, TCGBError *error))completion;


/** This is the method for obtaining ItemList which is registered at ToastCloud IAP Console.
 
 Request a item list which is purchasable. This list has items which are only registered in ToastCloud IAP Console, not Market(AppStore)
 
 @param completion      completion may return the NSArray of TCGBPurchasableItem.<br/>
 If there is an error, TCGBError will be returned.
 @warning   You should call this method after ```logged in```, otherwise you will get **TCGB_ERROR_NOT_LOGGED_IN** error in the completion.
 */
- (void)requestItemListAtIAPConsoleWithCompletion:(void(^)(NSArray<TCGBPurchasableItem *> *purchasableItemArray, TCGBError *error))completion;


/**---------------------------------------------------------------------------------------
 * @name Request Purchasing Item
 *  ---------------------------------------------------------------------------------------
 */

/** This is the method to request purchasing item which identifier is itemSeq. There is a viewController parameter and you may put your top most viewController. If you don't, this method will find out top most view controller and put it in the parameter.
 
 Request Purchasing Item that has itemId.
 
 @param itemSeq         itemID which you want to purchase.
 @param viewController  represent to current viewcontroller.
 @param completion      completion may return the TCGBPurchasableReceipt instance.<br/>
 If there is an error, TCGBError will be returned.
 @warning   You should call this method after ```logged in```, otherwise you will get **TCGB_ERROR_NOT_LOGGED_IN** error in the completion.
 */
- (void)requestPurchaseWithItemSeq:(long)itemSeq viewController:(UIViewController *)viewController completion:(void(^)(TCGBPurchasableReceipt *purchasableReceipt, TCGBError *error))completion;

- (void)requestPurchaseWithMarketItemId:(NSString *)marketItemId viewController:(UIViewController *)viewController completion:(void(^)(TCGBPurchasableReceipt *purchasableReceipt, TCGBError *error))completion;


/**---------------------------------------------------------------------------------------
 * @name Request non-consumed Item List
 *  ---------------------------------------------------------------------------------------
 */

/** This method provides an item list that have non-consumed.
 
 Request a Item List which is not consumed. You should deliver this itemReceipt to your game server to consume it or request consuming API to ToastCloud IAP Server. You may call this method after logged in and deal with these non-consumed items.
 
 @param completion      completion may return the NSArray of TCGBPurchasableReceipt instances.<br/>
 This instance has the paymentSequence, itemSequence, PurchaseToken information.<br/>
 If there is an error, TCGBError will be returned.
 @warning   You should call this method after ```logged in```, otherwise you will get **TCGB_ERROR_NOT_LOGGED_IN** error in the completion.
 */
- (void)requestItemListOfNotConsumedWithCompletion:(void(^)(NSArray<TCGBPurchasableReceipt *> *purchasableReceiptArray, TCGBError *error))completion;


/**---------------------------------------------------------------------------------------
 * @name Request retry purchasing processes
 *  ---------------------------------------------------------------------------------------
 */

/** This method is for retrying failed purchasing proccesses.
 
 Request a retrying transaction which is not completed to IAP Server
 
 @param completion      completion may return the TCGBPurchasableRetryTransactionResult which has two member variables which are named 'successList' and 'failList'.<br/>
 These two variables are array of TCGBPurchasableReceipt.<br/>
 Each key has a list of TCGBPurchasableReceipt that has uncompleted purchase information.<br/>
 If there is an error, TCGBError will be returned.
 */
- (void)requestRetryTransactionWithCompletion:(void(^)(TCGBPurchasableRetryTransactionResult *transactionResult, TCGBError *error))completion;

- (void)requestActivatedPurchasesWithCompletion:(void(^)(NSArray<TCGBPurchasableReceipt *> *purchasableReceiptArray, TCGBError *error))completion;

- (void)requestRestoreWithCompletion:(void(^)(NSArray<TCGBPurchasableReceipt *> *purchasableReceiptArray, TCGBError *error))completion;

@property (nonatomic, weak) id<TCGBPurchasePromotionIAPDelegate> promotionDelegate;

@end


#pragma mark - TCGBPurchasableItem
/** The TCGBPurchasableItem class is VO class of item entity.
 */
@interface TCGBPurchasableItem : NSObject

/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/** itemSeq
 
 Item Sequence which is the number presented in Toast Cloud IAP Console.
*/
@property (assign)            long itemSeq;

/** item price
 
 This value is from the market.
 @warning If there is no price data, it will be initialized to -1
 */
@property (assign)            float price;

/** item name
 
 Item name is from Toast Cloud IAP Console.
 */
@property (nonatomic, strong) NSString *itemName;

/** marketId
 
 which is actually **AS**.
 */
@property (nonatomic, strong) NSString *marketId;

/** marketItemId
 
 ItemID which is registered at market(itunesconnect).
 */
@property (nonatomic, strong) NSString *marketItemId;

/** currency
 */
@property (nonatomic, strong) NSString *currency;

/** usingStatus
 
 This string value represent if this item is available.
 */
@property (nonatomic, strong) NSString *usingStatus;


/**---------------------------------------------------------------------------------------
 * @name Allocation
 *  ---------------------------------------------------------------------------------------
 */

/** Initialize the class with JSON Value.
 
 @param result      result is a json formatted NSDictionary object. This is from ToastCloud IAP Server.
 @return Instance being initialized.
 */
+ (instancetype)purchasableItemWithResult:(NSDictionary*)result;
@end



#pragma mark - TCGBPurchasableReceipt
/** The TCGBPurchasableReceipt class represent a receipt that is from the IAP Server.
 */
@interface TCGBPurchasableReceipt : NSObject

/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/** itemSeq
 
 Item Sequence which is the number presented in Toast Cloud IAP Console.
 */
@property (assign)            long itemSeq;

/** marketItemId
 
 ItemID which is registered at market(itunesconnect).
 */
@property (nonatomic, strong) NSString *marketItemId;

/** item price
 
 This value is from the market.
 @warning If there is no price data, it will be initialized to -1
 */
@property (assign)            float price;

/** currency
 */
@property (nonatomic, strong) NSString *currency;

/** paymentSeq
 
 Payment Sequence is used to trace purchase transaction.
 */
@property (nonatomic, strong) NSString *paymentSeq;

/** purchaseToken
 
 Purchase Token is an unique string to validate purchasement.
 */
@property (nonatomic, strong) NSString *purchaseToken;

/**---------------------------------------------------------------------------------------
 * @name Allocation
 *  ---------------------------------------------------------------------------------------
 */

/** Initialize the class with JSON Value.
 
 @param result      result is a json formatted NSDictionary object. This is from ToastCloud IAP Server.
 @return Instance being initialized.
 */
+ (instancetype)purchasableReceiptWithResult:(NSDictionary*)result;

@end


#pragma mark - TCGBPurchasableRetryTransactionResult
/** The TCGBPurchasableRetryTransactionResult class represent a result after retrying failed purchasing processes.
 */
@interface TCGBPurchasableRetryTransactionResult : NSObject

/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/** successList
 
 This array contains results of successed receipt. Each receipt is implemented by `TCGBPurchasableReceipt`.
 */
@property (nonatomic, strong) NSArray<TCGBPurchasableReceipt *> *successList;

/** failList
 
 This array contains results of failed receipt. Each receipt is implemented by `TCGBPurchasableReceipt`.
 */
@property (nonatomic, strong) NSArray<TCGBPurchasableReceipt *> *failList;

/**---------------------------------------------------------------------------------------
 * @name Allocation
 *  ---------------------------------------------------------------------------------------
 */

/** Initialize the class with JSON Value.
 
 @param result      result is a json formatted NSDictionary object. This is from ToastCloud IAP Server.
 @return Instance being initialized.
 */
+ (instancetype)purchasableTransactionResultWithResult:(NSDictionary*)result;

@end
