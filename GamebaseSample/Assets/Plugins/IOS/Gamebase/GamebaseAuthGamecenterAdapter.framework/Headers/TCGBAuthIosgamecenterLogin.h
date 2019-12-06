//
//  TCGBAuthGamecenterLogin.h
//  TCGBAuthGamecenterAdapter
//
//  Created by NHN on 2016. 12. 13..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Gamebase/Gamebase.h>
#import <GameKit/GameKit.h>

#define GamebaseAuthGamecenterAdapterVersion @"2.0.1"

@interface TCGBAuthIosgamecenterLogin : NSObject <TCGBAuthAdapterDelegate>

@property (nonatomic, strong) NSString*         clientId;           // bundleId

@property (nonatomic, weak)   GKLocalPlayer*    localPlayer;
@property (nonatomic, strong) NSDictionary*     gameCenterAuthInfo;
@property (nonatomic, strong) NSString*         playerID;
@property (nonatomic, assign) BOOL              alreadyAuthenticated;
@property (nonatomic, assign) BOOL              callbackForAuthHandler;
@property (atomic, assign) BOOL                 isLoginViewPresented;

@property (nonatomic, strong) NSString*         snsToken;           // clientId + playerId


@end
