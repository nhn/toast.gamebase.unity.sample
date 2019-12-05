#import <ToastUnityCore/ToastUnityCore.h>

NSString *stringWithCString(const char *string) {
    NSString *convertString;
    
    if (string == NULL) {
        convertString = nil;
        
    } else if (strncmp(string, "", strlen(string)) == 0) {
        convertString = @"";
        
    } else {
        convertString = [NSString stringWithCString:string encoding:NSUTF8StringEncoding];
    }
    
    return convertString;
}

char *_toast_setApiData(const char *data) {
    NSString *message = stringWithCString(data);
    ToastUnityMessage *unityMessage = [[ToastUnityMessage alloc] initWithMessage:message];
    
    return [ToastUnity didReceiveUnityMessage:unityMessage];
}
