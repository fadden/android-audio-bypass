package com.faddensoft.androidaudiobypass;

import android.content.Context;
import android.content.res.AssetManager;
import android.util.Log;

import java.io.IOException;

public class AudioBypass {
    private static String TAG = "AudioBypass";

    private static final AudioBypass mInstance = new AudioBypass();

    private Context mContext;
    private AssetManager mAssetMan;


    // Get the singleton.
    //
    // Unity wasn't able to find this when the return type was
    // AudioBypass; use Object instead.
    public static Object getInstance() {
        return mInstance;
    }

    // Singleton constructor.
    //
    // We don't know much about the environment at this point,
    // and we don't have the Context yet, so there's not much
    // we can or should do.
    private void AudioBypass() {
        Log.d(TAG, "ctor");
    }

    public void initialize(Context context) {
        if (context == null) {
            Log.e(TAG, "context must not be null");
            throw new NullPointerException("context arg to initialize");
        }
        if (mContext != null) {
            if (context != mContext) {
                Log.e(TAG, "AudioBypass object initialized twice with different contexts");
                throw new RuntimeException("AudioBypass already initialized");
            } else {
                Log.d(TAG, "AudioBypass already initialized with this context");
                return;
            }
        }

        mContext = context;
        mAssetMan = context.getAssets();

        Log.i(TAG, "AudioBypass initialized: " + context);
    }

    private void checkInit() {
        if (mContext == null) {
            Log.e(TAG, "AudioBypass object not initialized");
            throw new RuntimeException("AudioBypass not initialized");
        }
    }

    public void testMethod() {
        checkInit();

        Log.i(TAG, "This is a test");
        Log.i(TAG, "This is a test");
        Log.i(TAG, "This is a test");
        Log.i(TAG, "This is a test");
        Log.i(TAG, "This is a test");

        // For debugging, dump the list of assets found.
        if (true) {
            try {
                String[] assets = mAssetMan.list("");
                Log.i(TAG, "Found " + assets.length + " assets:");
                for (int i = 0; i < assets.length; i++) {
                    Log.i(TAG, i + ": '" + assets[i] + "'");
                }
            } catch (IOException ioe) {
                Log.e(TAG, "failed " + ioe);
            }
        }
    }

    public void play(String name) {
        Log.i(TAG, "Playing " + name);
    }
}

