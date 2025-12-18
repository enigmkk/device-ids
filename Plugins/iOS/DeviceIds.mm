#import "DeviceId.h"
#import <AdSupport/ASIdentifierManager.h>
#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <Security/Security.h>

static NSString* GetIDFA()
{
    if (@available(iOS 14, *))
    {
        ASIdentifierManager* mgr = [ASIdentifierManager sharedManager];
        if (mgr.advertisingTrackingEnabled)
        {
            NSUUID* idfa = mgr.advertisingIdentifier;
            if (idfa &&
                ![idfa.UUIDString isEqualToString:@"00000000-0000-0000-0000-000000000000"])
            {
                return idfa.UUIDString;
            }
        }
        return nil;
    }
    else
    {
        NSUUID* idfa = [ASIdentifierManager sharedManager].advertisingIdentifier;
        return idfa ? idfa.UUIDString : nil;
    }
}

static NSString* GetIDFV()
{
    return [UIDevice currentDevice].identifierForVendor.UUIDString;
}

static NSString* kKey = @"com.astra.deviceid";

static NSString* Load() {
    NSDictionary* q = @{
        (__bridge id)kSecClass: (__bridge id)kSecClassGenericPassword,
        (__bridge id)kSecAttrAccount: kKey,
        (__bridge id)kSecReturnData: @YES
    };
    CFTypeRef res = NULL;
    if (SecItemCopyMatching((__bridge CFDictionaryRef)q, &res) != errSecSuccess)
        return nil;

    NSData* d = (__bridge_transfer NSData*)res;
    return [[NSString alloc] initWithData:d encoding:NSUTF8StringEncoding];
}

static void Save(NSString* v) {
    NSDictionary* q = @{
        (__bridge id)kSecClass: (__bridge id)kSecClassGenericPassword,
        (__bridge id)kSecAttrAccount: kKey,
        (__bridge id)kSecValueData: [v dataUsingEncoding:NSUTF8StringEncoding],
        (__bridge id)kSecAttrAccessible:
            (__bridge id)kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly
    };
    SecItemDelete((__bridge CFDictionaryRef)q);
    SecItemAdd((__bridge CFDictionaryRef)q, NULL);
}

extern "C" const char* DeviceIDFA(void)
{
    NSString* v = GetIDFA();
    if (!v) return nullptr;
    return strdup(v.UTF8String); // malloc
}

extern "C" const char* DeviceIDFV(void)
{
    NSString* v = GetIDFV();
    if (!v) return nullptr;
    return strdup(v.UTF8String); // malloc
}

extern "C" const char* DeviceId() {
    NSString* v = Load();
    if (!v) {
        v = [[NSUUID UUID] UUIDString];
        Save(v);
    }
    return strdup(v.UTF8String);
}

extern "C" void Device_Free(const char* ptr)
{
    if (ptr) free((void*)ptr);
}
