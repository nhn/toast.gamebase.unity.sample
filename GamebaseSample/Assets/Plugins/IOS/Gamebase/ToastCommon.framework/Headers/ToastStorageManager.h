//
//  ToastStorageManager.h
//  ToastLogger
//
//  Created by Hyup on 2017. 9. 11..
//  Copyright © 2017년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 # Storage manager
 
 A class that helps you store an array as a file or read from a file into an array.
 */
@interface ToastStorageManager : NSObject

/// ---------------------------------
/// @name Work about filename
/// ---------------------------------

/**
 Whether a matching file name exists.

 @param fileName The name of the file to search
 @return If 'YES', A file exists that matches the file name. If 'NO', don't exists.
 */
- (BOOL)existsFileName:(NSString *)fileName;

/**
 Remove, if there is a matching file name.
 
 @param fileName The name of the file to remove
 @return If 'YES', Successfully removing the file. If 'NO', Failed to remove the file.
 */
- (BOOL)removeFileName:(NSString *)fileName;

/// ---------------------------------
/// @name Save & Load file
/// ---------------------------------

/**
 Use when you need to save the contents of the array as a file.

 @param array The array to save
 @param saveKey The identifying key to use when loading
 @param condition The condition to use for thread safety.
 @param fileName The name of the file to save
 @param removeArray Whether or not to delete the array after saving.
 */
- (void)saveArrayToFile:(NSMutableArray *) array
                saveKey:(NSString *)saveKey
              condition:(NSCondition *)condition
               fileName:(NSString *)fileName
            removeArray:(BOOL)removeArray;


/**
 Used to load the contents of a file into an array.

 @param array The array to load
 @param arrayLimitSize The limit size of array
 @param fileName The name of the file to load
 @param saveKey The identifying key that was used to save.
 @param condition The condition to use for thread safety.
 @param removeFile Whether or not to delete the file after loading.
 */
- (void)fileToMemory:(NSMutableArray *)array
      arrayLimitSize:(NSNumber *)arrayLimitSize
            fileName:(NSString *)fileName
             saveKey:(NSString *)saveKey
           condition:(NSCondition *)condition
          removeFile:(BOOL)removeFile;



@end
