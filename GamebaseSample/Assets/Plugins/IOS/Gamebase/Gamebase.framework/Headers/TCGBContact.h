//
//  TCGBContact.h
//  Gamebase
//
//  Created by NHNEnt on 21/08/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#ifndef TCGBContact_h
#define TCGBContact_h

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "TCGBError.h"

@interface TCGBContact : NSObject

+ (void)openContactWithViewController:(UIViewController *)viewController
                           completion:(void(^)(TCGBError *error))completion;

@end


#endif /* TCGBContact_h */
