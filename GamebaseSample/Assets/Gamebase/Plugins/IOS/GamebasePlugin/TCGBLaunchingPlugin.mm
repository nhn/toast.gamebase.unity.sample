#import "TCGBLaunchingPlugin.h"
#import "DelegateManager.h"
#import <Gamebase/Gamebase.h>
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessageSender.h"
#import "UnityMessage.h"
#import "NativeMessage.h"

#define NLAUNCHING_API_GET_LAUNCHING_INFORMATIONS       @"gamebase://getLaunchingInformations"
#define NLAUNCHING_API_GET_LAUNCHING_STATUS             @"gamebase://getLaunchingStatus"

@implementation TCGBLaunchingPlugin

- (instancetype)init {
    if ((self = [super init]) == nil) {
        return nil;
    }
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:NLAUNCHING_API_GET_LAUNCHING_INFORMATIONS target:self selector:@selector(getLaunchingInformations:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:NLAUNCHING_API_GET_LAUNCHING_STATUS target:self selector:@selector(getLaunchingStatus:)];
    
    return self;
}

-(NSString*)getLaunchingInformations:(UnityMessage*)message {
    return [[TCGBLaunching launchingInformations] JSONString];
}

-(NSString*)getLaunchingStatus:(UnityMessage*)message {
    NSString* result = [@([TCGBLaunching launchingStatus]) stringValue];
    return result;
}
@end

