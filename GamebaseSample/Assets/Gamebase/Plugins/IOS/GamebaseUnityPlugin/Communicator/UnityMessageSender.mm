#import "UnityMessageSender.h"
#import <Gamebase/Gamebase.h>
#import <GamebasePlugin/NativeMessage.h>
#import <GamebasePlugin/TCGBJsonUtil.h>

@implementation UnityMessageSender

+(UnityMessageSender*)sharedUnityMessageSender {
    static dispatch_once_t onceToken;
    static UnityMessageSender* instance = nil;
    dispatch_once(&onceToken, ^{
        instance = [[UnityMessageSender alloc] init];
    });
    return instance;
}

-(void)sendMessage:(NativeMessage*)message gameObjectName:(NSString*)gameObjectName responseMethodName:(NSString*)responseMethodName {
    NativeMessage* responseMessage = message;
    NSString* jsonString = [responseMessage toJsonString];
    
    if (gameObjectName == nil || responseMethodName == nil || jsonString == nil){
    }
    else {
        [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageSender] sendMessage jsonString : %@", [[TCGBJsonUtil sharedGamebaseJsonUtil] prettyJsonStringFromObject:responseMessage]];
        UnitySendMessage([gameObjectName UTF8String], [responseMethodName UTF8String], [jsonString UTF8String]);
    }
}
@end
