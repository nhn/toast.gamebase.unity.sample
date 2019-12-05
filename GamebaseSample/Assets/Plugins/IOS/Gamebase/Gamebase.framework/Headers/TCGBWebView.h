//
//  TCGBWebView.h
//  TCGBWebKit
//
//  Created by NHN on 2016. 12. 20..
//  © NHN. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>

@class TCGBError;
@class TCGBWebURL;
@class TCGBWebViewConfiguration;
@class TCGBWebViewController;
@protocol TCGBWebViewDelegate;

typedef void(^TCGBWebViewCloseCompletion)(TCGBError *error);
typedef void(^TCGBWebViewSchemeEvent)(NSString *fullUrl, TCGBError *error);

extern NSString * const kTCGBWebKitDomain;
extern NSString * const kTCGBWebKitBundleName;

/** The TCGBWebView class represents the entry point for **launching WebView**.
 */
@interface TCGBWebView : NSObject

/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/**
 
 This property is a global configuration for launching webview.<br/>
 When you handle the webview without any configuration, TCGBWebView set its configuration with this value.
 */
@property (nonatomic, strong) TCGBWebViewConfiguration *defaultWebConfiguration;


/**---------------------------------------------------------------------------------------
 * @name Initialization
 *  ---------------------------------------------------------------------------------------
 */

/**
 
 Creates and returns an `TCGBWebView` object.
 */
+ (instancetype)sharedTCGBWebView;

/**---------------------------------------------------------------------------------------
 * @name Launching WebView
 *  ---------------------------------------------------------------------------------------
 */

/**
 Show WebView that is not for local url.
 
 @param urlString The string value for target url
 @param viewController The presenting view controller
 @warning If viewController is nil, TCGBWebView set it to top most view controller automatically.
 @param configuration This configuration is applied to the behavior of webview.
 @warning If configuration is nil, TCGBWebView set it to default value. It is described in `TCGBWebViewConfiguration`.
 @param closeCompletion This completion would be called when webview is closed
 @param schemeList This schemeList would be filtered every web view request and call schemeEvent
 @param schemeEvent This schemeEvent would be called when web view request matches one of the schemeLlist
 
 @since Added 1.5.0.
 */
+ (void)showWebViewWithURL:(NSString *)urlString
            viewController:(UIViewController*)viewController
             configuration:(TCGBWebViewConfiguration *)configuration
           closeCompletion:(TCGBWebViewCloseCompletion)closeCompletion
                schemeList:(NSArray<NSString *> *)schemeList
               schemeEvent:(TCGBWebViewSchemeEvent)schemeEvent;


/**
 Show WebView for local html (or other web resources)
 
 @param filePath The string value for target local path.
 @param bundle where the html file is located.
 @warning If bundle is nil, TCGBWebView set it to main bundle automatically.
 @param viewController The presenting view controller
 @warning If viewController is nil, TCGBWebView set it to top most view controller automatically.
 @param configuration This configuration is applied to the behavior of webview.
 @warning If configuration is nil, TCGBWebView set it to default value. It is described in `TCGBWebViewConfiguration`.
 @param closeCompletion This completion would be called when webview is closed
 @param schemeList This schemeList would be filtered every web view request and call schemeEvent
 @param schemeEvent This schemeEvent would be called when web view request matches one of the schemeLlist
 
 @since Added 1.5.0.
 */
+ (void)showWebViewWithLocalURL:(NSString *)filePath
                         bundle:(NSBundle *)bundle
                 viewController:(UIViewController*)viewController
                  configuration:(TCGBWebViewConfiguration *)configuration
                closeCompletion:(TCGBWebViewCloseCompletion)closeCompletion
                     schemeList:(NSArray<NSString *> *)schemeList
                    schemeEvent:(TCGBWebViewSchemeEvent)schemeEvent;

+ (void)showWebViewWithDefaultHTML:(NSString *)defaultHTML viewController:(UIViewController *)viewController configuration:(TCGBWebViewConfiguration *)configuration closeCompletion:(TCGBWebViewCloseCompletion)closeCompletion schemeList:(NSArray<NSString *> *)schemeList schemeEvent:(TCGBWebViewSchemeEvent)schemeEvent;


/**
 Open the Browser with urlString
 
 @param urlString The URL to be loaded.
 @warning If urlString is not valid, to open browser would be failed. Please check the url before calling.
 
 @since Added 1.5.0.
 */
+ (void)openWebBrowserWithURL:(NSString *)urlString;


/**
 Close the presenting Webview
 
 @since Added 1.5.0.
 */
+ (void)closeWebView;

@end





/** The TCGBWebViewDelegate is a UIViewController delegate.
 */
@protocol TCGBWebViewDelegate <NSObject>

@required

@optional
- (void)viewDidAppear:(BOOL)animated;
- (void)viewDidDisappear:(BOOL)animated;
- (void)close;
- (void)goBack;

//- (void)webViewDidStartLoad:(UIWebView *)webView;
- (void)webView:(WKWebView *)webView didCommitNavigation:(WKNavigation *)navigation;

//- (void)webViewDidFinishLoad:(UIWebView *)webView;
- (void)webView:(WKWebView *)webView didFinishNavigation:(WKNavigation *)navigation;

//- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType;
- (void)webView:(WKWebView *)webView decidePolicyForNavigationAction:(WKNavigationAction *)navigationAction decisionHandler:(void (^)(WKNavigationActionPolicy))decisionHandler;

//- (void)webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error;
- (void)webView:(WKWebView *)webView didFailNavigation:(WKNavigation *)navigation withError:(NSError *)error;

//- (NSString *)stringByEvaluatingJavaScriptFromString:(NSString *)script;
- (void)evaluateJavaScript:(NSString *)script completionHandler:(void(^)(id data, NSError *error))completion;

@property (nonatomic, weak) UIView *rootView;

@end
