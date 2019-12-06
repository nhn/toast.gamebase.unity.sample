//
//  TCGBUnityDictionaryJSON.h
//  TCGBUnityPlugin
//
//  Created by Yun Juhyun on 2017. 01. 17..
//  Copyright © 2017년 NHN Entertainment Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface NSDictionary (JSON)
- (NSString *)JSONString;
- (NSString *)PrettyJSONString;
@end

@interface NSArray (JSON)
- (NSString *)JSONString;
- (NSString *)JSONStringFromArray;
@end

@interface NSString (JSON)
- (NSDictionary *)JSONDictionary;
@end
