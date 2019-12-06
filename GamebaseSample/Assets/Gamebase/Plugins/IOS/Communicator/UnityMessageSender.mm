#import "UnityMessageSender.h"
#import "GamebaseJsonUtil.h"
#import "TCGBUnityDictionaryJSON.h"

@implementation UnityMessageSender

@synthesize gameObjectName = _gameObjectName;
@synthesize responseMethodName = _responseMethodName;
@synthesize message = _message;

+(UnityMessageSender*)sharedUnityMessageSender {
    static dispatch_once_t onceToken;
    static UnityMessageSender* instance = nil;
    dispatch_once(&onceToken, ^{
        instance = [[UnityMessageSender alloc] init];
    });
    return instance;
}

-(void)sendMessage:(NativeMessage*)message gameObjectName:(NSString*)gameObjectName responseMethodName:(NSString*)responseMethodName {
    [self setMessage:message];
    [self setGameObjectName:gameObjectName];
    [self setResponseMethodName:responseMethodName];
    [self sendMessage];
}

-(void)sendMessage {
    if (self.gameObjectName == nil){
    }
    else {
        [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageSender] sendMessage jsonString : %@", [[GamebaseJsonUtil sharedGamebaseJsonUtil] prettyJsonStringFromObject:self.message]];
        UnitySendMessage([self.gameObjectName UTF8String], [self.responseMethodName UTF8String], [[self.message toJsonString] UTF8String]);
    }
}
@end
