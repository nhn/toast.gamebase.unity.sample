//
//  TCGBServerPushMessage.h
//  Gamebase
//
//  Created by NHN on 2018. 2. 5..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

@class TCGBWebSocketResponseProtocol;

@interface TCGBObserverMessage : NSObject
@property (nonatomic, strong)   NSString*       type;
@property (nonatomic, strong)   NSDictionary*   data;

+ (TCGBObserverMessage *)observerMessageWithType:(NSString *)type data:(NSDictionary *)data;

@end
