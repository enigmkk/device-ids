#import "ATT.h"
#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import <AdSupport/ASIdentifierManager.h>

static ATTCallback s_callback = nullptr;
static bool s_requested = false;

void ATT_SetCallback(ATTCallback cb)
{
    s_callback = cb;
}

bool ATT_IsAuthorized(void)
{
    if (@available(iOS 14, *))
    {
        return ATTrackingManager.trackingAuthorizationStatus ==
               ATTrackingManagerAuthorizationStatusAuthorized;
    }
    return [ASIdentifierManager sharedManager].advertisingTrackingEnabled;
}

static void NotifyUnity(ATTrackingManagerAuthorizationStatus status)
{
    if (!s_callback) return;

    // 映射为 int，避免暴露 iOS enum
    int v = 0;
    switch (status)
    {
        case ATTrackingManagerAuthorizationStatusAuthorized:    v = 1; break;
        case ATTrackingManagerAuthorizationStatusDenied:        v = 2; break;
        case ATTrackingManagerAuthorizationStatusRestricted:   v = 3; break;
        case ATTrackingManagerAuthorizationStatusNotDetermined: v = 0; break;
    }

    // ⚠️ Unity 必须在主线程
    dispatch_async(dispatch_get_main_queue(), ^{
        s_callback(v);
    });
}

void ATT_Request(void)
{
    if (s_requested) return;
    s_requested = true;

    if (@available(iOS 14, *))
    {
        dispatch_async(dispatch_get_main_queue(), ^{
            [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:
             ^(ATTrackingManagerAuthorizationStatus status)
            {
                NotifyUnity(status);
            }];
        });
    }
}
