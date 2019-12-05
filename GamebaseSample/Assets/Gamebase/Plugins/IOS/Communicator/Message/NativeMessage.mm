#import <Foundation/Foundation.h>
#import "NativeMessage.h"
#import "GamebaseJsonUtil.h"
#import "TCGBUnityDictionaryJSON.h"

@implementation NativeMessage

@synthesize scheme = _scheme;
@synthesize handle = _handle;
@synthesize gamebaseError = _gamebaseError;
@synthesize jsonData = _jsonData;
@synthesize extraData = _extraData;


-(id)initWithMessage:(NSString*)scheme handle:(NSInteger)handle TCGBError:(TCGBError*)gamebaseError jsonData:(NSString*)jsonData extraData:(NSString*)extraData{
    if(self = [super init]) {
        
        self.scheme = scheme;
        self.handle = handle;
        if(gamebaseError != nil) {
            self.gamebaseError = [gamebaseError jsonString];
        }
        else {
            self.gamebaseError = @"";
        }
        if(jsonData != nil) {
            self.jsonData = jsonData;
        }
        else {
            self.jsonData = @"";
        }
        if(extraData != nil) {
            self.extraData = extraData;
        }
        else {
            self.extraData = @"";
        }
    }
    return self;
}

-(NSString*)toJsonString {
    NSMutableDictionary* jsonDic = [[GamebaseJsonUtil sharedGamebaseJsonUtil] mutableDictionaryFromObject:self];
    
    NSString* jsonString = [jsonDic JSONString];
    
    return jsonString;
}

@end

