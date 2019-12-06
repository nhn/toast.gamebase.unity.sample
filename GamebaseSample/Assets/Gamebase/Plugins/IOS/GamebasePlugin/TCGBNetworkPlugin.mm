#import "TCGBNetworkPlugin.h"
#import "DelegateManager.h"
#import <Gamebase/Gamebase.h>
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessageSender.h"
#import "UnityMessage.h"
#import "NativeMessage.h"

#define NETWORK_API_GET_TYPE                            @"gamebase://getType"
#define NETWORK_API_GET_TYPE_NAME                       @"gamebase://getTypeName"
#define NETWORK_API_IS_CONNECTED                        @"gamebase://isConnected"

@implementation TCGBNetworkPlugin

- (instancetype)init {
    if ((self = [super init]) == nil) {
        return nil;
    }
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:NETWORK_API_GET_TYPE target:self selector:@selector(getType:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:NETWORK_API_GET_TYPE_NAME target:self selector:@selector(getTypeName:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:NETWORK_API_IS_CONNECTED target:self selector:@selector(isConnected:)];
    
    return self;
}

-(NSString*)getType:(UnityMessage*)message {
    NSString* result = [@([TCGBNetwork type]) stringValue];
    return result;
}

-(NSString*)getTypeName:(UnityMessage*)message {
    NSString* result = [TCGBNetwork typeName];
    return result;
}

-(NSString*)isConnected:(UnityMessage*)message {
    NSMutableDictionary *contentDictionary = [[NSMutableDictionary alloc]init];
    [contentDictionary setValue:[NSNumber numberWithBool:[TCGBNetwork isConnected]] forKey:@"isConnected"];
    
    NSString* result = [contentDictionary JSONString];
    if(result == nil)
        return @"";
    
    return result;
}
@end

