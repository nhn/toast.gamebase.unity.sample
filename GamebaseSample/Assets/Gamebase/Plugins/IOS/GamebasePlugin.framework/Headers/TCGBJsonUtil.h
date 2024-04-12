#import <Foundation/Foundation.h>

@interface TCGBJsonUtil : NSObject {
}
+(TCGBJsonUtil*)sharedGamebaseJsonUtil;

-(NSMutableDictionary*)mutableDictionaryFromObject:(NSObject*)src;
-(NSString*)prettyJsonStringFromObject:(NSObject*)src;
-(NSString*)prettyJsonStringFromJsonString:(NSString*)src;
@end

@interface NSDictionary (JSON)
- (NSString *)toJSONString;
- (NSString *)toJSONPrettyString;
@end

@interface NSArray (JSON)
- (NSString *)toJSONString;
- (NSString *)toJSONStringOfObject;
@end

@interface NSString (JSON)
- (NSDictionary *)toJSONDictionary;
- (NSArray *)toJSONArray;
@end
