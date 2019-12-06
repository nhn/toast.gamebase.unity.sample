#import "TCGBContactPlugin.h"
#import "DelegateManager.h"
#import <Gamebase/Gamebase.h>
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessageSender.h"
#import "UnityMessage.h"
#import "NativeMessage.h"

#define CONTACT_API_OPEN_CONTACT    @"gamebase://openContact"

@implementation TCGBContactPlugin

- (instancetype)init {
    if ((self = [super init]) == nil) {
        return nil;
    }    
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:CONTACT_API_OPEN_CONTACT target:self selector:@selector(openContact:)];
    
    return self;
}

-(void)openContact:(UnityMessage*)message {
    [TCGBContact openContactWithViewController:UnityGetGLViewController() completion:^(TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:nil extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}
@end

