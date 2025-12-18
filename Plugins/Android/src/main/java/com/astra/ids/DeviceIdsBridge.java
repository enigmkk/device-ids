package com.astra.ids;

import android.content.Context;
import android.provider.Settings;

import android.util.Log;
import java.util.concurrent.Executors;

import com.astra.ids.IDeviceIdsCallback;
import com.google.android.gms.ads.identifier.AdvertisingIdClient;


public class DeviceIdsBridge 
{
    private static IDeviceIdsCallback m_callback;
    private static Context m_context;
    public static void start(IDeviceIdsCallback callback)
    {
        m_callback = callback;
        m_context = getContext();
        getGAIDAsync();
    }
    
    private static Context getContext() 
    {
        try 
        {
            Class<?> unityPlayer = Class.forName("com.unity3d.player.UnityPlayer");
            return (Context) unityPlayer.getField("currentActivity").get(null);
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    public static String getDeviceID() {
        Context ctx = m_context;
        return Settings.Secure.getString(
            ctx.getContentResolver(),
            Settings.Secure.ANDROID_ID
        );
    }
    public static void getGAIDAsync() {
        Executors.newSingleThreadExecutor().execute(() -> {
            String id = "";
            try {
                AdvertisingIdClient.Info info = AdvertisingIdClient.getAdvertisingIdInfo(m_context);
                if (info != null && !info.isLimitAdTrackingEnabled()) {
                    id = info.getId();
                }
            } catch (Exception e) {
                Log.e("DeviceId", "Failed to get GAID", e);
            }
            String finalId = id;
            m_callback.onGAIDReady(finalId);
        });
    }   
}

