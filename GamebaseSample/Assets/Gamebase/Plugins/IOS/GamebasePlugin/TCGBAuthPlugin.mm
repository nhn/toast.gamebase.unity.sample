#import "TCGBAuthPlugin.h"
#import "DelegateManager.h"
#import <Gamebase/Gamebase.h>
#import "TCGBUnityDictionaryJSON.h"
#import "UnityMessageSender.h"
#import "UnityMessage.h"
#import "NativeMessage.h"

#define AUTH_API_LOGIN                                  @"gamebase://login"
#define AUTH_API_LOGIN_ADDITIONAL_INFO                  @"gamebase://loginWithAdditionalInfo"
#define AUTH_API_LOGIN_CREDENTIAL_INFO                  @"gamebase://loginWithCredentialInfo"
#define AUTH_API_LOGIN_FOR_LAST_LOGGED_IN_PROVIDER      @"gamebase://loginForLastLoggedInProvider"
#define AUTH_API_LOGOUT                                 @"gamebase://logout"
#define AUTH_API_ADD_MAPPING                            @"gamebase://addMapping"
#define AUTH_API_ADD_MAPPING_CREDENTIAL_INFO            @"gamebase://addMappingWithCredentialInfo"
#define AUTH_API_ADD_MAPPING_ADDITIONAL_INFO            @"gamebase://addMappingWithAdditionalInfo"
#define AUTH_API_ADD_MAPPING_FORCIBLY                   @"gamebase://addMappingForcibly"
#define AUTH_API_ADD_MAPPING_FORCIBLY_CREDENTIAL_INFO   @"gamebase://addMappingForciblyWithCredentialInfo"
#define AUTH_API_ADD_MAPPING_FORCIBLY_ADDITIONAL_INFO   @"gamebase://addMappingForciblyWithAdditionalInfo"
#define AUTH_API_REMOVE_MAPPING                         @"gamebase://removeMapping"
#define AUTH_API_WITH_DRAW_ACCOUT                       @"gamebase://withdraw"
#define AUTH_API_ISSUE_TRANSFER_ACCOUNT                 @"gamebase://issueTransferAccount"
#define AUTH_API_QUERY_TRANSFER_ACCOUNT                 @"gamebase://queryTransferAccount"
#define AUTH_API_RENEW_TRANSFER_ACCOUNT                 @"gamebase://renewTransferAccount"
#define AUTH_API_TRANSFER_ACCOUNT_WITH_IDP_LOGIN        @"gamebase://transferAccountWithIdPLogin"
#define AUTH_API_GET_AUTH_MAPPING_LIST                  @"gamebase://getAuthMappingList"
#define AUTH_API_GET_AUTH_PROVIDER_USERID               @"gamebase://getAuthProviderUserID"
#define AUTH_API_GET_AUTH_PROVIDER_ACCESSTOKEN          @"gamebase://getAuthProviderAccessToken"
#define AUTH_API_GET_AUTH_PROVIDER_PROFILE              @"gamebase://getAuthProviderProfile"
#define AUTH_API_GET_BAN_INFO                           @"gamebase://getBanInfo"

// transfer account
#define RENEWAL_MODE_TYPE                               @"renewalModeType"
#define RENEWAL_MODE_TYPE_MANUAL                        @"MANUAL"
#define RENEWAL_MODE_TYPE_AUTO                          @"AUTO"
#define RENEWAL_TARGET_TYPE                             @"renewalTargetType"
#define RENEWAL_TARGET_TYPE_PASSWORD                    0
#define RENEWAL_TARGET_TYPE_ID_PASSWORD                 1
#define ACCOUNT_ID                                      @"accountId"
#define ACCOUNT_PASSWORD                                @"accountPassword"    

@implementation TCGBAuthPlugin

- (instancetype)init {
    if ((self = [super init]) == nil) {
        return nil;
    }
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_LOGIN target:self selector:@selector(login:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_LOGIN_ADDITIONAL_INFO target:self selector:@selector(loginWithAdditionalInfo:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_LOGIN_CREDENTIAL_INFO target:self selector:@selector(loginWithCredentialInfo:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_LOGIN_FOR_LAST_LOGGED_IN_PROVIDER target:self selector:@selector(loginForLastLoggedInProvider:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_LOGOUT target:self selector:@selector(logout:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_ADD_MAPPING target:self selector:@selector(addMapping:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_ADD_MAPPING_CREDENTIAL_INFO target:self selector:@selector(addMappingWithCredentialInfo:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_ADD_MAPPING_ADDITIONAL_INFO target:self selector:@selector(addMappingWithAdditionalInfo:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_ADD_MAPPING_FORCIBLY target:self selector:@selector(addMappingForcibly:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_ADD_MAPPING_FORCIBLY_CREDENTIAL_INFO target:self selector:@selector(addMappingForciblyWithCredentialInfo:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_ADD_MAPPING_FORCIBLY_ADDITIONAL_INFO target:self selector:@selector(addMappingForciblyWithAdditionalInfo:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_REMOVE_MAPPING target:self selector:@selector(removeMapping:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_WITH_DRAW_ACCOUT target:self selector:@selector(withdraw:)];
        
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_ISSUE_TRANSFER_ACCOUNT target:self selector:@selector(issueTransferAccount:)];
	
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_QUERY_TRANSFER_ACCOUNT target:self selector:@selector(queryTransferAccount:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_RENEW_TRANSFER_ACCOUNT target:self selector:@selector(renewTransferAccount:)];
    
    [[DelegateManager sharedDelegateManager] addAsyncDelegate:AUTH_API_TRANSFER_ACCOUNT_WITH_IDP_LOGIN target:self selector:@selector(transferAccountWithIdPLogin:)];

    [[DelegateManager sharedDelegateManager] addSyncDelegate:AUTH_API_GET_AUTH_MAPPING_LIST target:self selector:@selector(getAuthMappingList:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:AUTH_API_GET_AUTH_PROVIDER_USERID target:self selector:@selector(getAuthProviderUserID:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:AUTH_API_GET_AUTH_PROVIDER_ACCESSTOKEN target:self selector:@selector(getAuthProviderAccessToken:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:AUTH_API_GET_AUTH_PROVIDER_PROFILE target:self selector:@selector(getAuthProviderProfile:)];
    
    [[DelegateManager sharedDelegateManager] addSyncDelegate:AUTH_API_GET_BAN_INFO target:self selector:@selector(getBanInfo:)];
    
    return self;
}

-(void)login:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    [TCGBGamebase loginWithType:convertedDic[@"providerName"] viewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken description] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)loginWithAdditionalInfo:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    [TCGBGamebase loginWithType:convertedDic[@"providerName"] additionalInfo:convertedDic[@"additionalInfo"] viewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)loginWithCredentialInfo:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    [TCGBGamebase loginWithCredential:convertedDic viewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)loginForLastLoggedInProvider:(UnityMessage*)message {
    [TCGBGamebase loginForLastLoggedInProviderWithViewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)logout:(UnityMessage*)message {
    [TCGBGamebase logoutWithViewController:UnityGetGLViewController() completion:^(TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:nil extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)addMapping:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    [TCGBGamebase addMappingWithType:convertedDic[@"providerName"] viewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)addMappingWithCredentialInfo:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    [TCGBGamebase addMappingWithCredential:convertedDic viewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)addMappingWithAdditionalInfo:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    [TCGBGamebase addMappingWithType:convertedDic[@"providerName"] additionalInfo:convertedDic[@"additionalInfo"] viewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)addMappingForcibly:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    [TCGBGamebase addMappingForciblyWithType:convertedDic[@"providerName"] forcingMappingKey:convertedDic[@"forcingMappingKey"] viewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)addMappingForciblyWithCredentialInfo:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    [TCGBGamebase addMappingWithCredential:convertedDic[@"credentialInfo"] forcingMappingKey:convertedDic[@"forcingMappingKey"] viewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)addMappingForciblyWithAdditionalInfo:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    [TCGBGamebase addMappingForciblyWithType:convertedDic[@"providerName"] forcingMappingKey:convertedDic[@"forcingMappingKey"] additionalInfo:convertedDic[@"additionalInfo"] viewController:UnityGetGLViewController() completion:^(id authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken JSONString] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)removeMapping:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    [TCGBGamebase removeMappingWithType:convertedDic[@"providerName"] viewController:UnityGetGLViewController() completion:^(TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:nil extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)withdraw:(UnityMessage*)message {
    [TCGBGamebase withdrawWithViewController:UnityGetGLViewController() completion:^(TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:nil extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
        
    }];
}

-(void)issueTransferAccount:(UnityMessage*)message {
    [TCGBGamebase issueTransferAccountWithCompletion:^(TCGBTransferAccountInfo *transferAccount, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[transferAccount description] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)queryTransferAccount:(UnityMessage*)message {
    [TCGBGamebase queryTransferAccountWithCompletion:^(TCGBTransferAccountInfo *transferAccount, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[transferAccount description] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)renewTransferAccount:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    NSString* renewalModeType = convertedDic[RENEWAL_MODE_TYPE];
    NSInteger renewalTargetType = [convertedDic[RENEWAL_TARGET_TYPE] integerValue];
    
    TCGBTransferAccountRenewConfiguration* configuration = nil;
    
    if([renewalModeType caseInsensitiveCompare:RENEWAL_MODE_TYPE_MANUAL] == NSOrderedSame) {
        if(renewalTargetType == RENEWAL_TARGET_TYPE_PASSWORD) {
            configuration = [TCGBTransferAccountRenewConfiguration manualRenewConfigurationWithAccountPassword:convertedDic[ACCOUNT_PASSWORD]];
        }
        else if(renewalTargetType == RENEWAL_TARGET_TYPE_ID_PASSWORD){
            configuration = [TCGBTransferAccountRenewConfiguration manualRenewConfigurationWithAccountId:convertedDic[ACCOUNT_ID] accountPassword:convertedDic[ACCOUNT_PASSWORD]];
        }
    }
    else {
        if(renewalTargetType == RENEWAL_TARGET_TYPE_PASSWORD) {
            configuration = [TCGBTransferAccountRenewConfiguration autoRenewConfigurationWithRenewalTarget:TCGBTransferAccountRenewalTargetTypePassword];
        }
        else if(renewalTargetType == RENEWAL_TARGET_TYPE_ID_PASSWORD){
            configuration = [TCGBTransferAccountRenewConfiguration autoRenewConfigurationWithRenewalTarget:TCGBTransferAccountRenewalTargetTypeIdPassword];
        }
    }
    
    [TCGBGamebase renewTransferAccountWithConfiguration:configuration completion:^(TCGBTransferAccountInfo *transferAccount, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[transferAccount description] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(void)transferAccountWithIdPLogin:(UnityMessage*)message {
    NSDictionary* convertedDic = [message.jsonData JSONDictionary];
    
    [TCGBGamebase transferAccountWithIdPLoginWithAccountId:convertedDic[ACCOUNT_ID] accountPassword:convertedDic[ACCOUNT_PASSWORD] completion:^(TCGBAuthToken *authToken, TCGBError *error) {
        NativeMessage* responseMessage = [[NativeMessage alloc]initWithMessage:message.scheme handle:message.handle TCGBError:error jsonData:[authToken description] extraData:nil];
        [[UnityMessageSender sharedUnityMessageSender] sendMessage:responseMessage gameObjectName:message.gameObjectName responseMethodName:message.responseMethodName];
    }];
}

-(NSString*)getAuthMappingList:(UnityMessage*)message {
    NSString* result = [[TCGBGamebase authMappingList] JSONStringFromArray];
    return result;
}

-(NSString*)getAuthProviderUserID:(UnityMessage*)message {
    NSString* result = [TCGBGamebase authProviderUserIDWithIDPCode:message.jsonData];
    return result;
}

-(NSString*)getAuthProviderAccessToken:(UnityMessage*)message {
    NSString* result = [TCGBGamebase authProviderAccessTokenWithIDPCode:message.jsonData];
    return result;
}

-(NSString*)getAuthProviderProfile:(UnityMessage*)message {
    TCGBAuthProviderProfile* profil = [TCGBGamebase authProviderProfileWithIDPCode:message.jsonData];
    if(profil == nil)
    {
        return nil;
    }
    NSString* result = [profil description];
    return result;
}

-(NSString*)getBanInfo:(UnityMessage*)message {
    TCGBBanInfo* info = [TCGBGamebase banInfo];
    if(info == nil)
    {
        return nil;
    }
    NSString* result = [info description];
    return result;
}
@end
