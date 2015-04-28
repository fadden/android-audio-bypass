package com.faddensoft.androidaudiobypass;

import android.util.Log;

public class AudioBypass {
    private static String TAG = "AudioBypass";

    private static AudioBypass instance = new AudioBypass();

    // Unity wasn't able to find this when the return type was
    // AudioBypass; use Object instead.
    public static Object getInstance() {
        return instance;
    }

    public void AudioBypass() {
        // This is called the first time the class is referenced.
        // We don't know much about the environment, so it's best to
        // defer initialization.
    }

    public void testMethod() {
        Log.i(TAG, "This is a test");
        Log.i(TAG, "This is a test");
        Log.i(TAG, "This is a test");
        Log.i(TAG, "This is a test");
        Log.i(TAG, "This is a test");
    }
}

