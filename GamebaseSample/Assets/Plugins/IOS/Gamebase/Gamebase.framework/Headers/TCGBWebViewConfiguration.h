//
//  TCGBWebViewConfiguration.h
//  TCGBWebKit
//
//  Created by NHN on 2016. 12. 20..
//  © NHN. All rights reserved.
//

#import <UIKit/UIKit.h>
@protocol TCGBWebViewDelegate;

typedef NSUInteger TCGBWebViewOrientationMask;

/** These constants indicate the type of launch style such as popup, fullscreen.
 * @see style
 */
typedef NS_ENUM(NSUInteger, TCGBWebViewStyle) {
    /** PopUp Style.
     * The background opacity can be set by `backgroundOpacity` property.<br/>
     * The background color can be set by `maskViewColor` property.<br/>
     * The close button can be set by `closeImagePathForPopup` property.<br/>
     * @see backgroundOpacity
     * @see maskViewColor
     */
    TCGBWebViewLaunchPopUp                       = 0,
    
    /** Full Screen Style.
     * There is not status bar.<br/>
     * The navigation bar height can be set by `navigationBarHeight` property.<br/>
     * The navigation bar title can be set by `webViewTitle` property.<br/>
     * The navigation bar color can be set by `navBarColor` property.<br/>
     * @see navigationBarHeight
     * @see webViewTitle
     * @see navBarColor
     * @see isNavigationBarVisible
     * @see isStatusBarVisible
     */
    TCGBWebViewLaunchFullScreen                  = 1
};


/** Configure webview's orientation. It can be set by multiple value. 
 For example, "TCGBWebViewOrientationPortrait|TCGBWebViewOrientationPortraitUpsideDown" is available.
 TCGBWebView process this value using bit mask operation.
 * @see orientationMask
 */
typedef NS_ENUM(NSUInteger, TCGBWebViewOrientation) {
    /** Unspecified
     * Default value, it is set by application's orientation.
     * 0x0000, 0
     */
    TCGBWebViewOrientationUnspecified                = 0,
    
    /** Portrait
     * Default value, it is set by application's orientation.
     * 0x0001, 1
     */
    TCGBWebViewOrientationPortrait                 = 1,
    
    /** Portrait UpsideDown
     * Default value, it is set by application's orientation.
     * 0x0010, 2
     */
    TCGBWebViewOrientationPortraitUpsideDown       = 2,
    
    /** Landscape Right
     * Default value, it is set by application's orientation.
     * 0x0100, 4
     */
    TCGBWebViewOrientationLandscapeRight           = 4,
    
    /** Landscape Left
     * Default value, it is set by application's orientation.
     * 0x1000, 8
     */
    TCGBWebViewOrientationLandscapeLeft            = 8,
};

/** The TCGBWebViewConfiguration configures the behavior of webview launching.
 */
@interface TCGBWebViewConfiguration : NSObject

/**---------------------------------------------------------------------------------------
 * @name Style
 *  ---------------------------------------------------------------------------------------
 */

/**
 Launching Style.
 
 @warning Default is a TCGBWebViewLaunchFullScreen.
 */
@property (atomic) TCGBWebViewStyle style;


/**---------------------------------------------------------------------------------------
 * @name Orientation
 *  ---------------------------------------------------------------------------------------
 */

/**
 Orientation Mask.
 
 @see TCGBWebViewOrientation.
 @warning Default is a value that is converted to application's orientation value toward TCGBWebViewOrientation bit mask.
 */
@property (atomic, setter=setOrientationMask:) TCGBWebViewOrientationMask orientationMask;


/**---------------------------------------------------------------------------------------
 * @name Navigation
 *  ---------------------------------------------------------------------------------------
 */

/**
 Navigation Bar Height.
 
 Only used in full screen launch style.
 @warning Default value is 50.
 */
@property (assign) CGFloat navigationBarHeight;

/**
 Navigation Bar Color.
 
 Only used in full screen launch style.
 @warning Default value is blue.
 */
@property (strong, nonatomic) UIColor *navigationBarColor;

/**
 Navigation Bar Title.
 
 Only used in full screen launch style.
 @warning Default value is html document's title. If it was set, this title would be displayed.
 */
@property (strong, nonatomic) NSString *navigationBarTitle;

/**
 Boolean value.
 
 @warning Default value is NO. It is only used in FullScreen Style.
 */
@property (assign) BOOL isStatusBarVisible;

/**
 Boolean value.
 
 @warning Default value is YES. It is only used in FullScreen Style.
 */
@property (assign) BOOL isNavigationBarVisible;

/**
 Boolean value.
 
 @warning Default value is YES.
 */
@property (assign) BOOL isBackButtonVisible;

/**
 Close Image Path.
 
 Only used in full screen launch style.
 @warning Default value is "TCGBwebkit-cancel-black.png" in Gamebase.bundle
 */
@property (strong, nonatomic) NSString *closeImagePathForFullScreenNavigation;

/**
 Back Button Image Path.
 
 Only used in full screen launch style.
 @warning Default value is "TCGBwebkit-goback-black.png" in Gamebase.bundle
 */
@property (strong, nonatomic) NSString *goBackImagePathForFullScreenNavigation;


/**---------------------------------------------------------------------------------------
 * @name Popup
 *  ---------------------------------------------------------------------------------------
 */

/**
 Alpha value of the background.
 
 Only used in popup launch style.
 @warning This value should be between 0 and 1.
 */
@property (atomic, setter=setBackgroundOpacity:) CGFloat backgroundOpacity;

/**
 Background Mask View Color.
 
 Only used in popup launch style.
 @warning Default value is black.
 */
@property (strong, nonatomic) UIColor *backgroundColor;

/**
 Close Image Path.
 
 Only used in popup launch style.
 @warning Default value is "TCGBwebkit-cancel-circle-white.png" in Gamebase.bundle
 */
@property (strong, nonatomic) NSString *closeImagePathForPopup;

/**
 Offset of close button in Pop Up Style.
 
 Base point is the right upper corner point. Default value is (0, 0)
 */
@property (atomic) CGPoint closeButtonOffsetInPopupStyle;


/**---------------------------------------------------------------------------------------
 * @name Delegate Protocol
 *  ---------------------------------------------------------------------------------------
 */

/**
 UIViewController Delegate.
 
 It is used to delegate UIViewController's methods such as viewDidLoad: viewWillLoad: viewDidDisappear: and etc.
 */
@property (strong, nonatomic) id<TCGBWebViewDelegate> delegate;




- (UIInterfaceOrientationMask)convertOrientationMask;
- (TCGBWebViewOrientationMask)convertTCGBWebViewOrientationMaskFrom:(UIInterfaceOrientationMask)orientationMask;

@end
