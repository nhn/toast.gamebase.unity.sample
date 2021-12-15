#import <Foundation/Foundation.h>
#import <Gamebase/Gamebase.h>

@interface UnityMessage : NSObject {
    NSString* _gameObjectName;
    NSString* _responseMethodName;
}

@property (nonatomic, strong) NSString* gameObjectName;
@property (nonatomic, strong) NSString* responseMethodName;

@end

