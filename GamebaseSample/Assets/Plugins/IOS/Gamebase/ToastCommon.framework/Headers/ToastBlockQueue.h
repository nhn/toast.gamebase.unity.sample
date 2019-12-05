//
//  ToastBlockQueue.h
//  ToastLogger
//
//  Created by Hyup on 2017. 9. 11..
//  Copyright © 2017년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 # Block Queue
 
 A Queue that additionally supports operations that wait for the queue to become non-empty when retrieving an element, and wait for space to become available in the queue when storing an element.
 */
@interface ToastBlockQueue : NSObject

/// ---------------------------------
/// @name Initialize
/// ---------------------------------

/**
 Initialize a block queue with a given queue capacity
 
 @param queueCapacity capacity of block queue(The maximum capacity of the queue can not exceed this value)
 @return Instance of ToastBlockQueue
 
 */
- (id)initWithQueueCapacity:(int)queueCapacity;

/// ---------------------------------
/// @name Queue Methods
/// ---------------------------------

/**
 Whether or not the queue is empty.

 @return If `YES`, the queue is empty. If `NO`, not empty.
 */
- (BOOL)queueEmpty;


/**
 Gets the size of queue. If the size of the queue is 0, it keeps waiting.

 @return The size of queue
 */
- (NSUInteger)queueSize;

/**
 Gets the size of queue.
 
 @return The size of queue
 */
- (NSUInteger)queueCount;

/**
 Gets the first object of queue.

 @return The first object of queue.
 */
- (id)queueFront;

/**
 Gets the last object of queue.

 @return The last object of queue.
 */
- (id)queueBack;

/**
 Inserts the specified element into the end of this queue if it is possible to do so immediately without violating capacity restrictions. If the capacity restrictions is violated, the last object is deleted until it is not.

 @param obj The object to enqueue
 */
- (void)queueEnqueue:(id)obj;


/**
 Inserts the specified element into the head of this queue if it is possible to do so immediately without violating capacity restrictions. If the capacity restrictions is violated, the last object is deleted until it is not.

 @param obj The object to enqueue
 */
- (void)queueFrontEnqueue:(id)obj;


/**
 Retrieves and removes the head of this queue, waiting if necessary until an element becomes available.


 @return The head object of this queue
 */
- (id)queueDequeue;


- (NSArray *)queueObjects;

//Queue 저장이 필요할 경우 사용
/**
 Use when you need to save the contents of the queue.

 @param fileName The name of the file to save
 @param saveKey The identifying key to use when loading
 @param projectKey The project key to be used when creating the file name
 */
- (void)queueSaveStorageWithFileName:(NSString *)fileName
                             saveKey:(NSString *)saveKey
                          projectKey:(NSString *)projectKey;


/**
 Used to load the contents of a file into a queue.

 @param fileName The name of the file to load
 @param saveKey The identifying key that was used to save.
 @param projectKey The project key to be used when creating the file name
 */
- (void)loadStorageToQueueWithFileName:(NSString *)fileName
                               saveKey:(NSString *)saveKey
                            projectKey:(NSString *)projectKey;


/**
 Used to remove the file that saved the queue.

 @param fileName The name of the file to remove
 @param projectKey The project key to be used when creating the file name
 @return If `YES`, succeed in removing. If `NO`, faliure in removing
 */
- (BOOL)removeQueueSaveFileWithFileName:(NSString *)fileName projectKey:(NSString *)projectKey;

@end
