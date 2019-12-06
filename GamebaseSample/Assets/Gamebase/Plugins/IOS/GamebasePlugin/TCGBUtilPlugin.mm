#import "TCGBUtilPlugin.h"
#import "DelegateManager.h"
#import <Gamebase/Gamebase.h>
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessageSender.h"
#import "UnityMessage.h"
#import "NativeMessage.h"

#define UTIL_API_SHOW_TOAST_WITH_TYPE   @"gamebase://showToastWithType"
#define UTIL_API_SHOW_ALERT             @"gamebase://showAlert"
#define UTIL_API_SHOW_ALERT_EVENT       @"gamebase://showAlertEvent"

@implementation TCGBUtilPlugin

- (instancetype)init {
    if ((self = [super init]) == nil) {
        return nil;
    }

    [[DelegateManager sharedDelegateManager] addAsyncDelegate:UTIL_API_SHOW_TOAST_WITH_TYPE target:self selector:@selector(showToastWithType:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:UTIL_API_SHOW_ALERT target:self selector:@selector(showAlert:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:UTIL_API_SHOW_ALERT_EVENT target:self selector:@selector(showAlertEvent:)];
    
    return self;
}

-(void)showToastWithType:(UnityMessage*)message {
    NSDictionary* convertedDic    = [message.jsonData JSONDictionary];
    
    NSInteger durationType        = [convertedDic[@"duration"] integerValue];
    NSString* context             = convertedDic[@"message"];
    
    if(durationType == 0)
    {
        [TCGBUtil showToastWithMessage:context length:GamebaseToastLengthShort];
    }
    else
    {
        [TCGBUtil showToastWithMessage:context length:GamebaseToastLengthLong];
    }
}

-(void)showAlert:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    NSString* title         = convertedDic[@"title"];
    NSString* context       = convertedDic[@"message"];
    
    [TCGBUtil showAlertWithTitle:title message:context];
}

-(void)showAlertEvent:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    NSString* title         = convertedDic[@"title"];
    NSString* context       = convertedDic[@"message"];
    
    [TCGBUtil showAlertWithTitle:title message:context completion:^{
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:nil jsonData:nil extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}
@end
