#import "TCGBUnityMessage.h"
#import "TCGBUnityMessageSender.h"
#import <GamebasePlugin/GamebasePlugin.h>
#import <Gamebase/Gamebase.h>
#import "TCGBUnityInterface.h"

#define GAMEBASE_UNITY_PLUGIN_API

extern "C" {
    GAMEBASE_UNITY_PLUGIN_API void initialize(char* className);
    GAMEBASE_UNITY_PLUGIN_API char* getSync(char* jsonString);
    GAMEBASE_UNITY_PLUGIN_API void getAsync(char* jsonString);
}

#pragma mark - extern C

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
        GamebaseEngine.engineType = @"UNITY";
    }
}

char* getSync(char* jsonString) {
    NSString *data;
    
    if(jsonString != nil) {
        data = [NSString stringWithUTF8String:jsonString];
    }
    
    if(data != nil) {
        [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageReceiver] getSync jsonString : %@", [[TCGBJsonUtil sharedGamebaseJsonUtil] prettyJsonStringFromJsonString:data]];
    }

    EngineMessage* engineMessage = [[EngineMessage alloc]initWithJsonString:data];
    NSInvocation* invocation = [[DelegateManager sharedDelegateManager] getSyncDelegate:engineMessage.scheme];
    
    void *invokeReturnChar;
    if (invocation) {
        TCGBPluginData* pluginData = [[TCGBPluginData alloc]initWithEngineMessage:engineMessage completion:^(EngineMessage *engineMessage, NativeMessage *message) {
            [[TCGBUnityMessageSender sharedUnityMessageSender] sendMessage:message gameObjectName:engineMessage.gameObjectName responseMethodName:engineMessage.responseMethodName];
        }];
        
        [invocation setArgument:&pluginData atIndex:2];
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
  
    if(jsonString != nil) {
        data = [NSString stringWithUTF8String:jsonString];
    }
    
    if(data != nil) {
        [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageReceiver] getAsync jsonString : %@", [[TCGBJsonUtil sharedGamebaseJsonUtil] prettyJsonStringFromJsonString:data]];
    }

    EngineMessage* engineMessage = [[EngineMessage alloc]initWithJsonString:data];
    NSInvocation* invocation = [[DelegateManager sharedDelegateManager] getAsyncDelegate:engineMessage.scheme];
    
    if (invocation) {
        TCGBPluginData* pluginData = [[TCGBPluginData alloc]initWithEngineMessage:engineMessage completion:^(EngineMessage *engineMessage, NativeMessage *message) {
            [[TCGBUnityMessageSender sharedUnityMessageSender] sendMessage:message gameObjectName:engineMessage.gameObjectName responseMethodName:engineMessage.responseMethodName];
        }];
        
        [invocation setArgument:&pluginData atIndex:2];
        [invocation invoke];
    }
}
