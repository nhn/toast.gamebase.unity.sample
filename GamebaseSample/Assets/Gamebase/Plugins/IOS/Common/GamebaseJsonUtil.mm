#import <Foundation/Foundation.h>
#import "NativeMessage.h"
#import "GamebaseJsonUtil.h"
#import "TCGBUnityDictionaryJSON.h"
#import <objc/runtime.h>

@implementation GamebaseJsonUtil

+(GamebaseJsonUtil*)sharedGamebaseJsonUtil {
    static dispatch_once_t onceToken;
    static GamebaseJsonUtil* instance = nil;
    dispatch_once(&onceToken, ^{
        instance = [[GamebaseJsonUtil alloc] init];
    });
    return instance;
}

-(NSMutableDictionary*)mutableDictionaryFromObject:(NSObject*)src {
    NSMutableDictionary *dict = [NSMutableDictionary dictionary];
    
    unsigned count;
    objc_property_t *properties = class_copyPropertyList([src class], &count);
    
    for (int i = 0; i < count; i++) {
        NSString *key = [NSString stringWithUTF8String:property_getName(properties[i])];
        dict[key] = [src valueForKey:key];
    }
    
    free(properties);
    
    return dict;
}

-(NSString*)prettyJsonStringFromObject:(NSObject*)src {
    NSMutableDictionary* jsonDictionary = [[GamebaseJsonUtil sharedGamebaseJsonUtil] mutableDictionaryFromObject:src];
    NSString* jsonString = [self prettyJsonStringFromMutableDictionary:jsonDictionary];
    return jsonString;
}

-(NSString*)prettyJsonStringFromJsonString:(NSString*)src {
    NSMutableDictionary* jsonDictionary = [[src JSONDictionary] mutableCopy];
    NSString* jsonString = [self prettyJsonStringFromMutableDictionary:jsonDictionary];
    return jsonString;
}

-(NSString*)prettyJsonStringFromMutableDictionary:(NSMutableDictionary*)src {
    NSError* jsonError;
    NSData* jsonData = [NSJSONSerialization dataWithJSONObject:src options:NSJSONWritingPrettyPrinted error:&jsonError];
    
    if(jsonData) {
        return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }
    
    if(jsonError) {
        [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][GamebaseJsonUtil] prettyJsonStringFromMutableDictionary Error : %@", jsonError];
    }
    
    return @"";
}
@end

