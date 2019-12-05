#import <Foundation/Foundation.h>
#import <Gamebase/Gamebase.h>

@interface GamebaseJsonUtil : NSObject {
}

+(GamebaseJsonUtil*)sharedGamebaseJsonUtil;

-(NSMutableDictionary*)mutableDictionaryFromObject:(NSObject*)src;
-(NSString*)prettyJsonStringFromObject:(NSObject*)src;
-(NSString*)prettyJsonStringFromJsonString:(NSString*)src;
@end
