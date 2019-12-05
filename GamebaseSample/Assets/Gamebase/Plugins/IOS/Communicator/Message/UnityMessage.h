#import <Foundation/Foundation.h>
#import <Gamebase/Gamebase.h>

@interface UnityMessage : NSObject {
    NSString* _scheme;
    NSInteger _handle;
    NSString* _jsonData;
    NSString* _extraData;
    NSString* _gameObjectName;
    NSString* _responseMethodName;
}

@property (nonatomic, strong) NSString* scheme;
@property (nonatomic, assign) NSInteger handle;
@property (nonatomic, strong) NSString* jsonData;
@property (nonatomic, strong) NSString* extraData;
@property (nonatomic, strong) NSString* gameObjectName;
@property (nonatomic, strong) NSString* responseMethodName;

@end

