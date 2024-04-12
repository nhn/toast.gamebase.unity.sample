#ifndef TCGBUnityViewController_h
#define TCGBUnityViewController_h

#import <GamebasePlugin/GamebasePlugin.h>

@interface TCGBUnityViewController : NSObject <TCGBViewControllerDelegate>

- (UIViewController*)getViewController;

@end

#endif
