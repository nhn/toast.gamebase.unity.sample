//
//  TCGBUnityDictionaryJSON.m
//  TCGBUnityPlugin
//
//  Created by Yun Juhyun on 2017. 01. 17..
//  Copyright © 2017년 NHN Entertainment Corp. All rights reserved.
//

@implementation NSDictionary (JSON)

- (NSString *)JSONString {
    NSError* error;
    NSData* jsonData = [NSJSONSerialization dataWithJSONObject:self options:0 error:&error];
    if (error) {
        return nil;
    }
    NSString* jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return jsonString;
}

- (NSString *)PrettyJSONString {
    NSError* error;
    NSData* jsonData = [NSJSONSerialization dataWithJSONObject:self options:NSJSONWritingPrettyPrinted error:&error];
    if (error) {
        return nil;
    }
    NSString* jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return jsonString;
}
@end

@implementation NSArray (JSON)

- (NSString *)JSONString {
    __block NSMutableArray *jsonArray = [NSMutableArray array];
    [self enumerateObjectsUsingBlock:^(id  _Nonnull obj, NSUInteger idx, BOOL * _Nonnull stop) {
        [jsonArray addObject:[obj description]];
    }];
    
    return [NSString stringWithFormat:@"[%@]", [jsonArray componentsJoinedByString:@","]];
}


- (NSString *)JSONStringFromArray {
    NSError* error;
    NSData* jsonData = [NSJSONSerialization dataWithJSONObject:self options:0 error:&error];
    if (error) {
        return nil;
    }
    NSString* jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return jsonString;
}
/*
- (NSString *)PrettyJSONString {
    NSError* error;
    NSData* jsonData = [NSJSONSerialization dataWithJSONObject:self options:NSJSONWritingPrettyPrinted error:&error];
    if (error) {
        return nil;
    }
    NSString* jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return jsonString;
}
 */
@end

@implementation NSString (JSON)

- (id)JSONDictionary {
    NSError* error;
    if (self == nil || [self length] <= 0) {
        return nil;
    }
    
    id jsonObject = [NSJSONSerialization JSONObjectWithData:[self dataUsingEncoding:NSUTF8StringEncoding] options:0 error:&error];
    if (error) {
        return nil;
    }
    return jsonObject;
}

@end
