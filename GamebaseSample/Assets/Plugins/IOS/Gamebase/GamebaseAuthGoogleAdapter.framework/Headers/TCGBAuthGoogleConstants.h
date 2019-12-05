//
//  TCGBAuthGoogleConstants.h
//  GamebaseAuthGoogleAdapter
//
//  Created by NHN on 2018. 5. 24..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

extern NSString* const kTCGBAuthGoogleOAuthErrorDomain;
typedef NS_ENUM(NSInteger, TCGBAuthGoogleErrorCode) {
    GoogleErrorUserCancel = 1,                          // user cancel login process
    GoogleErrorLoginPageError = 10,                     // cannot load google login web page
    GoogleErrorLoginURLSchemeIsNotExist = 20,           // no URL scheme string
    GoogleErrorLoginURLSchemeFormatError = 21,          // URL scheme format is invalid
    GoogleErrorLoginURLSchemeInvalidQueriesError = 22,    // snsCd query parameter is invalid
    GoogleErrorLoginGoogleAPIInvalidStatus = 100,       // Receive Invalid Status Code from Response object
    GoogleErrorUnknown = 999,                            // Unknown Error
};

