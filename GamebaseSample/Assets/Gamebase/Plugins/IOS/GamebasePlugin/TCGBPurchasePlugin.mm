#import "TCGBPurchasePlugin.h"
#import "DelegateManager.h"
#import <Gamebase/Gamebase.h>
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessageSender.h"
#import "UnityMessage.h"
#import "NativeMessage.h"

#define PURCHASE_API_REQUEST_PURCHASE_SEQ                   @"gamebase://requestPurchaseSeq"
#define PURCHASE_API_REQUEST_PURCHASE                       @"gamebase://requestPurchase"
#define PURCHASE_API_REQUEST_ITEM_LIST_OF_NOT_CONSUMED      @"gamebase://requestItemListOfNotConsumed"
#define PURCHASE_API_REQUEST_RETYR_TRANSACTION              @"gamebase://requestRetryTransaction"
#define PURCHASE_API_REQUEST_ITEM_LIST_PURCHASABLE          @"gamebase://requestItemListPurchasable"
#define PURCHASE_API_REQUEST_ITEM_LIST_AT_AP_CONSOLE        @"gamebase://requestItemListAtIAPConsole"
#define PURCHASE_API_SET_PROMOTION_IAP_HANDLER              @"gamebase://setPromotionIAPHandler"
#define PURCHASE_API_SET_STORE_CODE                         @"gamebase://setStoreCode"
#define PURCHASE_API_GET_STORE_CODE                         @"gamebase://getStoreCode"
#define PURCHASE_API_REQUEST_ACTIVATED_PURCHASES            @"gamebase://requestActivatedPurchases"

@implementation TCGBPurchasePlugin

- (instancetype)init {
    if ((self = [super init]) == nil) {
        return nil;
    }
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PURCHASE_API_REQUEST_PURCHASE_SEQ target:self selector:@selector(requestPurchaseSeq:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PURCHASE_API_REQUEST_ITEM_LIST_OF_NOT_CONSUMED target:self selector:@selector(requestItemListOfNotConsumed:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PURCHASE_API_REQUEST_RETYR_TRANSACTION target:self selector:@selector(requestRetryTransaction:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PURCHASE_API_REQUEST_ITEM_LIST_PURCHASABLE target:self selector:@selector(requestItemListPurchasable:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PURCHASE_API_REQUEST_ITEM_LIST_AT_AP_CONSOLE target:self selector:@selector(requestItemListAtIAPConsole:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PURCHASE_API_SET_PROMOTION_IAP_HANDLER target:self selector:@selector(setPromotionIAPHandler:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:PURCHASE_API_SET_STORE_CODE target:self selector:@selector(setStoreCode:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:PURCHASE_API_GET_STORE_CODE target:self selector:@selector(getStoreCode:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PURCHASE_API_REQUEST_ACTIVATED_PURCHASES target:self selector:@selector(requestActivatedPurchases:)];
    return self;
}

-(void)requestPurchaseSeq:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    [TCGBPurchase requestPurchaseWithItemSeq:[convertedDic[@"itemSeq"] longValue] viewController:UnityGetGLViewController() completion:^(TCGBPurchasableReceipt *purchasableReceipt, TCGBError *error) {
        
        NSMutableDictionary* jsonDic = [NSMutableDictionary dictionary];
        if(purchasableReceipt != nil) {
            jsonDic[@"itemSeq"] = [NSNumber numberWithLong:purchasableReceipt.itemSeq];
            jsonDic[@"price"] = [NSNumber numberWithLong:purchasableReceipt.price];
            jsonDic[@"currency"] = purchasableReceipt.currency;
            jsonDic[@"paymentSeq"] = purchasableReceipt.paymentSeq;
            jsonDic[@"purchaseToken"] = purchasableReceipt.purchaseToken;
        }
        
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[jsonDic JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)requestItemListOfNotConsumed:(UnityMessage*)message {
    [TCGBPurchase requestItemListOfNotConsumedWithCompletion:^(NSArray<TCGBPurchasableReceipt *> *purchasableReceiptArray, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[purchasableReceiptArray JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)requestRetryTransaction:(UnityMessage*)message {
    [TCGBPurchase requestRetryTransactionWithCompletion:^(TCGBPurchasableRetryTransactionResult *transactionResult, TCGBError *error) {
        NSString* jsonString = @"";
        if(transactionResult != nil)
        {
            jsonString = [transactionResult description];
        }
        
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:jsonString extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)requestItemListPurchasable:(UnityMessage*)message {
    [TCGBPurchase requestItemListPurchasableWithCompletion:^(NSArray<TCGBPurchasableItem *> *purchasableItemArray, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[purchasableItemArray JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)requestItemListAtIAPConsole:(UnityMessage*)message {
    [TCGBPurchase requestItemListAtIAPConsoleWithCompletion:^(NSArray<TCGBPurchasableItem *> *purchasableItemArray, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[purchasableItemArray JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)setPromotionIAPHandler:(UnityMessage*)message {
    [TCGBPurchase setPromotionIAPHandler:^(TCGBPurchasableReceipt *purchasableReceipt, TCGBError *error) {
        NSMutableDictionary* jsonDic = [NSMutableDictionary dictionary];
        if(purchasableReceipt != nil) {
            jsonDic[@"itemSeq"] = [NSNumber numberWithLong:purchasableReceipt.itemSeq];
            jsonDic[@"price"] = [NSNumber numberWithLong:purchasableReceipt.price];
            jsonDic[@"currency"] = purchasableReceipt.currency;
            jsonDic[@"paymentSeq"] = purchasableReceipt.paymentSeq;
            jsonDic[@"purchaseToken"] = purchasableReceipt.purchaseToken;
        }
        
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[jsonDic JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(NSString*)setStoreCode:(UnityMessage*)message {
    [TCGBPurchase setStoreCode:message.jsonData];
    return @"";
}

-(NSString*)getStoreCode:(UnityMessage*)message {
    NSString *storeCode = [TCGBPurchase storeCode];
    
    return storeCode;
}

-(void)requestActivatedPurchases:(UnityMessage*)message {
    [TCGBPurchase requestActivatedPurchasesWithCompletion:^(NSArray<TCGBPurchasableReceipt *> *purchasableReceiptArray, TCGBError *error) {
        
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[purchasableReceiptArray JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}
@end

