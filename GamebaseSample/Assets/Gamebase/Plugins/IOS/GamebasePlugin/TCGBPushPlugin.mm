#import "TCGBPushPlugin.h"
#import "DelegateManager.h"
#import <Gamebase/Gamebase.h>
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessageSender.h"
#import "UnityMessage.h"
#import "NativeMessage.h"

#define PUSH_API_REGISTER_PUSH      @"gamebase://registerPush"
#define PUSH_API_QUERY_PUSH         @"gamebase://queryPush"
#define PUSH_API_SET_SANDBOX_MODE   @"gamebase://setSandboxMode"

@implementation TCGBPushPlugin

- (instancetype)init {
    if ((self = [super init]) == nil) {
        return nil;
    }
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PUSH_API_REGISTER_PUSH target:self selector:@selector(registerPush:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PUSH_API_QUERY_PUSH target:self selector:@selector(queryPush:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:PUSH_API_SET_SANDBOX_MODE target:self selector:@selector(setSandboxMode:)];
    
    return self;
}

-(void)registerPush:(UnityMessage*)message {
    TCGBPushConfiguration * pushConfiguration = [TCGBPushConfiguration pushConfigurationWithJSONString:message.jsonData];
    
    [TCGBPush registerPushWithPushConfiguration:pushConfiguration completion:^(TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:nil extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)queryPush:(UnityMessage*)message {
    [TCGBPush queryPushWithCompletion:^(TCGBPushConfiguration *configuration, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[configuration JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)setSandboxMode:(UnityMessage*)message {
    
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    BOOL isSandbox = [convertedDic[@"isSandbox"] boolValue];
    [TCGBPush setSandboxMode:isSandbox];
}

@end

