//
//  TCGBServerPushMessage.h
//  Gamebase
//
//  Created by NHN on 2018. 2. 5..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

@class TCGBWebSocketResponseProtocol;

@interface TCGBServerPushMessage : NSObject {
    @private
    NSDictionary*   _serverPush;
    NSDictionary*   _popup;
    
}
@property (nonatomic, strong)   NSString*       type;           // type
@property (nonatomic, strong)   NSString*       data;           // data.result

+ (TCGBServerPushMessage *)serverPushMessageWithType:(NSString *)type data:(NSString *)data popup:(NSDictionary *)popup serverPush:(NSDictionary *)serverPush;
+ (TCGBServerPushMessage *)serverPushMessageWithResponseData:(TCGBWebSocketResponseProtocol *)responseData;



@end
