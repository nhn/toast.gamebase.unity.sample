//
//  TCGBUnityInterface.h
//  Unity-iPhone
//
//  Created by daengi on 2017. 1. 12..
//
//

#import <Foundation/Foundation.h>
#import "AppDelegateListener.h"

@interface TCGBUnityInterface : NSObject <AppDelegateListener>

+ (TCGBUnityInterface *)sharedUnityInterface;

@end
