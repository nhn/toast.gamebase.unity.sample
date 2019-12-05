#import "TCGBAnalyticsPlugin.h"
#import "DelegateManager.h"
#import <Gamebase/Gamebase.h>
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessageSender.h"
#import "UnityMessage.h"
#import "NativeMessage.h"

#define ANALYTICS_API_SET_GAME_USER_DATA                @"gamebase://setGameUserData"
#define ANALYTICS_API_TRACE_LEVEL_UP                    @"gamebase://traceLevelUp"


@implementation TCGBAnalyticsPlugin

- (instancetype)init {
    if ((self = [super init]) == nil) {
        return nil;
    }

    [[DelegateManager sharedDelegateManager] addAsyncDelegate:ANALYTICS_API_SET_GAME_USER_DATA target:self selector:@selector(setGameUserData:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:ANALYTICS_API_TRACE_LEVEL_UP target:self selector:@selector(traceLevelUp:)];
    
    return self;
}

-(void)setGameUserData:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    TCGBAnalyticsGameUserData* gameUserData = [TCGBAnalyticsGameUserData gameUserDataWithUserLevel:[convertedDic[@"userLevel"] intValue]];
    gameUserData.channelId = convertedDic[@"channelId"];
    gameUserData.characterId = convertedDic[@"characterId"];
    gameUserData.classId = convertedDic[@"characterClassId"];
    
    [TCGBAnalytics setGameUserData:gameUserData];
}

-(void)traceLevelUp:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    TCGBAnalyticsLevelUpData* levelUpData = [TCGBAnalyticsLevelUpData levelUpDataWithUserLevel:[convertedDic[@"userLevel"] intValue] levelUpTime:[convertedDic[@"levelUpTime"] longLongValue]];
    [TCGBAnalytics traceLevelUpWithLevelUpData:levelUpData];
}
@end