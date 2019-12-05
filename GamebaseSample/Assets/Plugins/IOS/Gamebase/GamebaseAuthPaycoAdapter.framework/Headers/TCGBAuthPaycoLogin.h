//
//  TCGBAuthPaycoLogin.h
//  TCGBAuthPaycoAdapter
//
//  Created by NHN on 2016. 11. 28..
//  © NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Gamebase/Gamebase.h>
#import <PIDThirdPartyAuth/PIDThirdPartyAuth.h>

#define GamebaseAuthPaycoAdapterVersion @"2.0.2"

@interface TCGBAuthPaycoLogin : NSObject <TCGBAuthAdapterDelegate, PIDThirdPartyAuthDelegate>

@property (nonatomic, strong) NSString *urlScheme;
@property (nonatomic, strong) TCGBProviderAuthCredential* credential;

@end
