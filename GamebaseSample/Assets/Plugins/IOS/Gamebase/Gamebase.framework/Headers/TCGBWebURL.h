//
//  TCGBWebURL.h
//  TCGBWebKit
//
//  Created by NHN on 2016. 12. 20..
//  © NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

/** This TCGBWebURL class represents URL that is much more easy to handling.
 */
@interface TCGBWebURL : NSObject


/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/** scheme
 
 The url scheme string, and the key of schemeCallbackDictionary;
 @see `TCGBWebView`
 */
@property (nonatomic, strong, readonly) NSString *scheme;

/** host
 
 This is a host of the scheme.
 */
@property (nonatomic, strong, readonly) NSString *host;

/** query
 
 This is a query dictionary of the scheme.
 */
@property (nonatomic, strong, readonly) NSDictionary *query;

/** fragment
 
 This is a fragment of the scheme.
 */
@property (nonatomic, strong, readonly) NSString *fragment;

/** url
 
 This is a NSURL object of the scheme.
 */
@property (nonatomic, strong, readonly) NSURL *url;

/**---------------------------------------------------------------------------------------
 * @name Initialization
 *  ---------------------------------------------------------------------------------------
 */

/** Creates and returns an `TCGBWebURL` object.
 
 @param url NSURL type parameter
 */
+ (instancetype)webViewURLWithURL:(NSURL *)url;


/** Creates and returns an `TCGBWebURL` object.
 
 @param url NSString type parameter
 */
+ (instancetype)webViewURLWithURLString:(NSString *)url;

@end

