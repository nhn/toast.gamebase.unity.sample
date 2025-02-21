#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@protocol TCGBViewControllerDelegate <NSObject>

- (UIViewController*)getViewController;

@end


@interface TCGBViewControllerManager : NSObject {
}

@property (nonatomic, weak) id<TCGBViewControllerDelegate> delegate;
	
+ (TCGBViewControllerManager*)sharedGamebaseViewControllerManager;

- (UIViewController*)getViewController;

@end
