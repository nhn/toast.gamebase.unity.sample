#import <Foundation/Foundation.h>
#import "NativeMessage.h"

@interface UnityMessageSender : NSObject {
    NSString* _gameObjectName;
    NSString* _responseMethodName;
    NativeMessage* _message;
}

@property (nonatomic, strong) NSString* gameObjectName;
@property (nonatomic, strong) NSString* responseMethodName;
@property (nonatomic, strong) NativeMessage* message;

+(UnityMessageSender*)sharedUnityMessageSender;

-(void)sendMessage:(NativeMessage*)message gameObjectName:(NSString*)gameObjectName responseMethodName :(NSString*)responseMethodName;

@end
