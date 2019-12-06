#import "TCGBGamebasePlugin.h"
#import "TCGBAuthPlugin.h"
#import "TCGBLaunchingPlugin.h"
#import "TCGBPurchasePlugin.h"
#import "TCGBPushPlugin.h"
#import "TCGBWebviewPlugin.h"
#import "TCGBUtilPlugin.h"
#import "TCGBNetworkPlugin.h"
#import "DelegateManager.h"
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessage.h"
#import "GamebaseJsonUtil.h"
#import <Gamebase/Gamebase.h>


#pragma mark - extern C
extern "C" {
    void initialize(char* className)
    {
        Class newClass = NSClassFromString([NSString stringWithUTF8String:className]);
        if(newClass != nil)
        {
            id newInstance = [[newClass alloc] init];
            if(newInstance == nil)
            {
                [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageReceiver] initialize fail : %s", className];
                return;
            }
            [[DelegateManager sharedDelegateManager] addClass:newInstance];
        }
    }
    
    char* getSync(char* jsonString) {
        NSString *data;
        if(jsonString != nil) {
            data = [NSString stringWithUTF8String:jsonString];
        }
        
        if(data != nil) {
            [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageReceiver] getSync jsonString : %@", [[GamebaseJsonUtil sharedGamebaseJsonUtil] prettyJsonStringFromJsonString:data]];
        }
        
        NSDictionary* convertedDic = [data JSONDictionary];
        
        UnityMessage* message = [UnityMessage alloc];
        message.scheme = convertedDic[@"scheme"];
        message.handle = [convertedDic[@"handle"] intValue];
        message.jsonData = convertedDic[@"jsonData"];
        message.extraData = convertedDic[@"extraData"];
        message.gameObjectName = convertedDic[@"gameObjectName"];
        message.responseMethodName = convertedDic[@"responseMethodName"];
        
        NSInvocation* invocation = [[DelegateManager sharedDelegateManager] getSyncDelegate:message.scheme];
        
        void *invokeReturnChar;
        if (invocation) {
            [invocation setArgument:&message atIndex:2];
            [invocation invoke];
            [invocation getReturnValue:&invokeReturnChar];
        }
        
        NSString *returnCharBridged = (__bridge NSString *)invokeReturnChar;
        if (returnCharBridged == nil) {
            returnCharBridged = @"";
        }
        
        [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageReceiver] getSync returnValue : %@", returnCharBridged];
        
        char *returnValue = (char*)[returnCharBridged UTF8String];
        return returnValue;
    }
    
    void getAsync(char* jsonString) {
        NSString *data;
        if(jsonString != nil)
            data = [NSString stringWithUTF8String:jsonString];
        
        NSDictionary* convertedDic = [data JSONDictionary];
        
        UnityMessage* message = [UnityMessage alloc];
        message.scheme = convertedDic[@"scheme"];
        message.handle = [convertedDic[@"handle"] intValue];
        message.jsonData = convertedDic[@"jsonData"];
        message.extraData = convertedDic[@"extraData"];
        message.gameObjectName = convertedDic[@"gameObjectName"];
        message.responseMethodName = convertedDic[@"responseMethodName"];
        
        if(jsonString != nil) {
            data = [NSString stringWithUTF8String:jsonString];
        }
        
        if(data != nil) {
            [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageReceiver] getAsync jsonString : %@", [[GamebaseJsonUtil sharedGamebaseJsonUtil] prettyJsonStringFromJsonString:data]];
        }
        
        NSInvocation* invocation = [[DelegateManager sharedDelegateManager] getAsyncDelegate:message.scheme];
        if (invocation) {
            [invocation setArgument:&message atIndex:2];
            [invocation invoke];
        }
    }
}

