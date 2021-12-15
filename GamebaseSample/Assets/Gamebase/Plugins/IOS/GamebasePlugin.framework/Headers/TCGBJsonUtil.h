#import <Foundation/Foundation.h>

@interface TCGBJsonUtil : NSObject {
}
+(TCGBJsonUtil*)sharedGamebaseJsonUtil;

-(NSMutableDictionary*)mutableDictionaryFromObject:(NSObject*)src;
-(NSString*)prettyJsonStringFromObject:(NSObject*)src;
-(NSString*)prettyJsonStringFromJsonString:(NSString*)src;
@end

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
- (NSArray *)JSONArray;
@end
