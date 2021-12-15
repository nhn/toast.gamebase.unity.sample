#import "UnityMessage.h"
#import "UnityMessageSender.h"
#import <GamebasePlugin/GamebasePlugin.h>
#import <Gamebase/Gamebase.h>
#import "TCGBUnityInterface.h"

#pragma mark - extern C
extern "C" {
    void initializeUnityInterface() {
        [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageReceiver] initializeUnityInterface"];
        [TCGBUnityInterface sharedUnityInterface];
		
		[[TCGBViewControllerManager sharedGamebaseViewControllerManager] setViewController:UnityGetGLViewController()];		
    }
    
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
            [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][UnityMessageReceiver] getSync jsonString : %@", [[TCGBJsonUtil sharedGamebaseJsonUtil] prettyJsonStringFromJsonString:data]];
        }
        
        NSDictionary* convertedDic = [data JSONDictionary];
        NSInvocation* invocation = [[DelegateManager sharedDelegateManager] getSyncDelegate:convertedDic[@"scheme"]];
        
        void *invokeReturnChar;
        if (invocation) {
            TCGBPluginData* pluginData =
            [[TCGBPluginData alloc]initWithJsonData:data completion:^(NSString *jsonData, NativeMessage *message) {
                
                NSDictionary* convertedDic = [data JSONDictionary];
                
                UnityMessage* sendMessage = [UnityMessage alloc];
                sendMessage.gameObjectName = convertedDic[@"gameObjectName"];
                sendMessage.responseMethodName = convertedDic[@"responseMethodName"];
                
               [[UnityMessageSender sharedUnityMessageSender] sendMessage:message gameObjectName:sendMessage.gameObjectName responseMethodName:sendMessage.responseMethodName];
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
        
      
        NSDictionary* convertedDic = [data JSONDictionary];
        NSInvocation* invocation = [[DelegateManager sharedDelegateManager] getAsyncDelegate:convertedDic[@"scheme"]];
        if (invocation) {
            TCGBPluginData* pluginData =
            [[TCGBPluginData alloc]initWithJsonData:data completion:^(NSString *jsonData, NativeMessage *message) {
                
                NSDictionary* convertedDic = [data JSONDictionary];
                
                UnityMessage* sendMessage = [UnityMessage alloc];
                sendMessage.gameObjectName = convertedDic[@"gameObjectName"];
                sendMessage.responseMethodName = convertedDic[@"responseMethodName"];
                
                [[UnityMessageSender sharedUnityMessageSender] sendMessage:message gameObjectName:sendMessage.gameObjectName responseMethodName:sendMessage.responseMethodName];
            }];
            
            [invocation setArgument:&pluginData atIndex:2];
            [invocation invoke];
        }
    }
}

