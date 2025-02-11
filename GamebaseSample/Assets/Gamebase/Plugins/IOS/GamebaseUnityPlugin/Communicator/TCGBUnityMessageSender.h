#import <Foundation/Foundation.h>

@class NativeMessage;

@interface TCGBUnityMessageSender: NSObject {
    
}

+(TCGBUnityMessageSender*)sharedUnityMessageSender;

-(void)sendMessage:(NativeMessage*)message gameObjectName:(NSString*)gameObjectName responseMethodName :(NSString*)responseMethodName;

@end
