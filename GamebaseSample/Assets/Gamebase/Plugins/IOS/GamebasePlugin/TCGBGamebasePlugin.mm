#import "TCGBGamebasePlugin.h"
#import "DelegateManager.h"
#import <Gamebase/Gamebase.h>
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessageSender.h"
#import "UnityMessage.h"
#import "NativeMessage.h"
#import "TCGBUnityInterface.h"

#define GAMEBASE_API_INITIALIZE                     @"gamebase://initialize"
#define GAMEBASE_API_SET_DEBUG_MODE                 @"gamebase://setDebugMode"
#define GAMEBASE_API_GET_SDK_VERSION                @"gamebase://getSDKVersion"
#define GAMEBASE_API_GET_USERID                     @"gamebase://getUserID"
#define GAMEBASE_API_GET_ACCESSTOKEN                @"gamebase://getAccessToken"
#define GAMEBASE_API_GET_LAST_LOGGED_IN_PROVIDER    @"gamebase://getLastLoggedInProvider"
#define GAMEBASE_API_GET_DEVICE_LANGUAGE_CODE       @"gamebase://getDeviceLanguageCode"
#define GAMEBASE_API_GET_CARRIER_CODE               @"gamebase://getCarrierCode"
#define GAMEBASE_API_GET_CARRIER_NAME               @"gamebase://getCarrierName"
#define GAMEBASE_API_GET_COUNTRY_CODE               @"gamebase://getCountryCode"
#define GAMEBASE_API_GET_COUNTRY_CODE_OF_USIM       @"gamebase://getCountryCodeOfUSIM"
#define GAMEBASE_API_GET_COUNTRY_CODE_OF_DEVICE     @"gamebase://getCountryCodeOfDevice"
#define GAMEBASE_API_IS_SANDBOX                     @"gamebase://isSandbox"
#define GAMEBASE_API_SET_DISPLAY_LANGUAGE_CODE      @"gamebase://setDisplayLanguageCode"
#define GAMEBASE_API_GET_DISPLAY_LANGUAGE_CODE      @"gamebase://getDisplayLanguageCode"
#define GAMEBASE_API_ADD_SERVER_PUSH_EVENT          @"gamebase://addServerPushEvent"
#define GAMEBASE_API_REMOVE_SERVER_PUSH_EVENT       @"gamebase://removeServerPushEvent"
#define GAMEBASE_API_ADD_OBSERVER                   @"gamebase://addObserver"
#define GAMEBASE_API_REMOVE_OBSERVER                @"gamebase://removeObserver"

@implementation TCGBGamebasePlugin


@synthesize serverPushMessage                       = _serverPushMessage;
@synthesize observerMessage                         = _observerMessage;
@synthesize observerGameObjectName                  = _observerGameObjectName;
@synthesize observerResponseMethodName              = _observerResponseMethodName;
@synthesize observerHandle                          = _observerHandle;
@synthesize serverPushEventGameObjectName           = _serverPushEventGameObjectName;
@synthesize serverPushEventResponseMethodName       = _serverPushEventResponseMethodName;
@synthesize serverPushEventHandle                   = _serverPushEventHandle;;

- (instancetype)init {
    if ((self = [super init]) == nil) {
        return nil;
    }
    
    __block TCGBGamebasePlugin *tempSelf = self;
    
    _serverPushMessage = ^(TCGBServerPushMessage* message){
        NSMutableDictionary* jsonDic = [[NSMutableDictionary alloc]init];
        jsonDic[@"type"] = message.type;
        jsonDic[@"data"] = message.data;
        
        NSString* jsonString = [jsonDic JSONString];
        
        NativeMessage* responseMessage = [[NativeMessage alloc] initWithMessage:GAMEBASE_API_ADD_SERVER_PUSH_EVENT handle:tempSelf.serverPushEventHandle TCGBError:nil jsonData:jsonString extraData:nil];
        
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:tempSelf.serverPushEventGameObjectName responseMethodName:tempSelf.serverPushEventResponseMethodName];
    };
    
    _observerMessage = ^(TCGBObserverMessage* message){
        NSMutableDictionary* jsonDic = [[NSMutableDictionary alloc] init];
        jsonDic[@"type"] = message.type;
        jsonDic[@"data"] = message.data;
        
        NSString* jsonString = [jsonDic JSONString];
        
        NativeMessage* responseMessage = [[NativeMessage alloc] initWithMessage:GAMEBASE_API_ADD_OBSERVER handle:tempSelf.observerHandle TCGBError:nil jsonData:jsonString extraData:nil];
        
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:tempSelf.observerGameObjectName responseMethodName:tempSelf.observerResponseMethodName];
    };
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:GAMEBASE_API_INITIALIZE target:self selector:@selector(initialize:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:GAMEBASE_API_SET_DEBUG_MODE target:self selector:@selector(setDebugMode:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_SDK_VERSION target:self selector:@selector(getSDKVersion:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_USERID target:self selector:@selector(getUserID:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_ACCESSTOKEN target:self selector:@selector(getAccessToken:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_LAST_LOGGED_IN_PROVIDER target:self selector:@selector(getLastLoggedInProvider:)];
    	
	[[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_DEVICE_LANGUAGE_CODE target:self selector:@selector(getDeviceLanguageCode:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_CARRIER_CODE target:self selector:@selector(getCarrierCode:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_CARRIER_NAME target:self selector:@selector(getCarrierName:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_COUNTRY_CODE target:self selector:@selector(getCountryCode:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_COUNTRY_CODE_OF_USIM target:self selector:@selector(getCountryCodeOfUSIM:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_COUNTRY_CODE_OF_DEVICE target:self selector:@selector(getCountryCodeOfDevice:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_IS_SANDBOX target:self selector:@selector(isSandbox:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_SET_DISPLAY_LANGUAGE_CODE target:self selector:@selector(setDisplayLanguageCode:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_GET_DISPLAY_LANGUAGE_CODE target:self selector:@selector(getDisplayLanguageCode:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_ADD_SERVER_PUSH_EVENT target:self selector:@selector(addServerPushEvent:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_REMOVE_SERVER_PUSH_EVENT target:self selector:@selector(removeServerPushEvent:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_ADD_OBSERVER target:self selector:@selector(addObserver:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:GAMEBASE_API_REMOVE_OBSERVER target:self selector:@selector(removeObserver:)];
    
    return self;
}

-(void)initialize:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    TCGBConfiguration* con = [TCGBConfiguration configurationWithAppID:convertedDic[@"appID"] appVersion:convertedDic[@"appVersion"] zoneType:convertedDic[@"zoneType"]];
    
    [con enablePopup:[convertedDic[@"enablePopup"] boolValue]];
    [con enableLaunchingStatusPopup:[convertedDic[@"enableLaunchingStatusPopup"] boolValue]];
    [con enableBanPopup:[convertedDic[@"enableBanPopup"] boolValue]];
    [con setStoreCode:convertedDic[@"storeCode"]];
    [con setDisplayLanguageCode:convertedDic[@"displayLanguageCode"]];
    
    [TCGBGamebase initializeWithConfiguration:con completion:^(id launchingData, TCGBError *error) {
        [TCGBUnityInterface sharedUnityInterface];
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[launchingData JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)setDebugMode:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    BOOL isDebugMode = [convertedDic[@"isDebugMode"] boolValue];
    [TCGBGamebase setDebugMode:isDebugMode];
}

-(NSString*)getSDKVersion:(UnityMessage*)message; {
    NSString* version = [TCGBGamebase SDKVersion];
    return version;
}

-(NSString*)getUserID:(UnityMessage*)message; {
    return [TCGBGamebase userID];
}

-(NSString*)getAccessToken:(UnityMessage*)message; {
    return [TCGBGamebase accessToken];
}

-(NSString*)getLastLoggedInProvider:(UnityMessage*)message; {
    return [TCGBGamebase lastLoggedInProvider];
}

-(NSString*)getDeviceLanguageCode:(UnityMessage*)message; {
    return [TCGBGamebase deviceLanguageCode];
}

-(NSString*)getCarrierCode:(UnityMessage*)message; {
    return [TCGBGamebase carrierCode];
}

-(NSString*)getCarrierName:(UnityMessage*)message; {
    return [TCGBGamebase carrierName];
}

-(NSString*)getCountryCode:(UnityMessage*)message; {
    return [TCGBGamebase countryCode];
}

-(NSString*)getCountryCodeOfUSIM:(UnityMessage*)message; {
    return [TCGBGamebase countryCodeOfUSIM];
}

-(NSString*)getCountryCodeOfDevice:(UnityMessage*)message; {
    return [TCGBGamebase countryCodeOfDevice];
}

-(NSString*)isSandbox:(UnityMessage*)message; {
    NSMutableDictionary *contentDictionary = [[NSMutableDictionary alloc]init];
    [contentDictionary setValue:[NSNumber numberWithBool:[TCGBGamebase isSandbox]] forKey:@"isSandbox"];
    
    NSString* returnValue = [contentDictionary JSONString];
    if(returnValue == nil)
        return @"";
    
    return returnValue ;
}

-(NSString*)setDisplayLanguageCode:(UnityMessage*)message; {
    [TCGBGamebase setDisplayLanguageCode:message.jsonData];
    return @"";
}

-(NSString*)getDisplayLanguageCode:(UnityMessage*)message; {
    return [TCGBGamebase displayLanguageCode];
}

-(NSString*)addServerPushEvent:(UnityMessage*)message;{
    _serverPushEventHandle              = message.handle;
    _serverPushEventGameObjectName      = message.gameObjectName;
    _serverPushEventResponseMethodName   = message.responseMethodName;
    
    [TCGBGamebase addServerPushEvent:_serverPushMessage];
    return @"";
}

-(NSString*)removeServerPushEvent:(UnityMessage*)message;{
    _serverPushEventHandle              = -1;
    _serverPushEventGameObjectName      = nil;
    _serverPushEventResponseMethodName   = nil;
    
    [TCGBGamebase removeServerPushEvent:_serverPushMessage];
    return @"";
}

-(NSString*)addObserver:(UnityMessage*)message;{
    _observerHandle                     = message.handle;
    _observerGameObjectName             = message.gameObjectName;
    _observerResponseMethodName          = message.responseMethodName;
    
    [TCGBGamebase addObserver:_observerMessage];
    return @"";
}

-(NSString*)removeObserver:(UnityMessage*)message;{
    _observerHandle                     = -1;
    _observerGameObjectName             = nil;
    _observerResponseMethodName          = nil;
    
    [TCGBGamebase removeObserver:_observerMessage];
    return @"";
}
@end

