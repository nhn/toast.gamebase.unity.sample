#import <Foundation/Foundation.h>

@class NativeMessage;

@interface UnityMessageSender: NSObject {
    
}

+(UnityMessageSender*)sharedUnityMessageSender;

-(void)sendMessage:(NativeMessage*)message gameObjectName:(NSString*)gameObjectName responseMethodName :(NSString*)responseMethodName;

@end
