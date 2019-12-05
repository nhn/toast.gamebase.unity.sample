//
//  ToastFieldKeyValidater.h
//  ToastCommon
//
//  Created by JooHyun Lee on 2018. 3. 23..
//  Copyright © 2018년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 # Field Validator
 
 This class checks the validity of the values used in ToastSDK.
 
 */
@interface ToastFieldValidator : NSObject

/// ---------------------------------
/// @name Properties
/// ---------------------------------

/** Pre-reserved keys used by the SDK */
@property (strong, nonatomic) NSMutableArray *ignoreKeys;

/// ---------------------------------
/// @name Get Methods
/// ---------------------------------

/**
 Gets only the fields that passed validation with given fields consisting of key and value.

 @param fields The fields to be validated
 @return The valid fields
 */
- (NSDictionary *)validateFields:(NSDictionary *)fields;

/**
 Gets a verify key with given the key to be validated.
 If the key is nil or null or empty string, this return nil.
 If the key is in use by the SDK, this return with "reserved_" appended to the front of the key.

 @param key The key to be validated
 @return The valid key(If the key is empty string, this be nil)
 */
- (NSString *)validateFieldKey:(NSString *)key;

/**
 Gets a verify value with given the value to be validated.
 If the value is nil or null, this return nil.
 If the value's class is not NSString or NSNumber, this return specific string.
 
 @param value The value to be validated
 @return The valid value.
 */
- (NSString *)validateFieldValue:(id)value;


/**
 Whether or not the key is in ignoreKeys.

 @param key The key to check
 @return If 'YES', the key is not in ignoreKeys. If 'NO', the key is in ignoreKeys.
 */
- (BOOL)isValidateFieldKey:(NSString *)key;

@end
