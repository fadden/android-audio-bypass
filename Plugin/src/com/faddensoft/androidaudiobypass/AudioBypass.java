/*
 * Copyright 2015 faddenSoft. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package com.faddensoft.androidaudiobypass;

import android.content.Context;
import android.content.res.AssetFileDescriptor;
import android.content.res.AssetManager;
import android.media.AudioManager;
import android.media.SoundPool;
import android.util.Log;

import java.io.IOException;

// This provides simple access to SoundPool.  It's intended to be used
// as a Unity 3D plugin for Android.
public class AudioBypass {
    private static String TAG = "AudioBypass";

    // SoundPool parameters.  These could be args to the AudioBypass
    // constructor, but except perhaps for MAX_STREAMS there's little
    // value in doing so.
    private static final int MAX_STREAMS = 4;
    private static final int STREAM_TYPE = AudioManager.STREAM_MUSIC;
    private static final int SRC_QUALITY = 0;

    private final Object mLock = new Object();
    private Context mContext;
    private AssetManager mAssetMan;
    private SoundPool mSoundPool;


    // Create a new instance.
    //
    // Unity wasn't able to find this when the return type was
    // AudioBypass; use Object instead.
    public static Object createInstance(Context context) {
        return new AudioBypass(context);
    }

    // Constructor.  Pass in the Activity context.
    //
    // We have a final field, so the VM's immutability guarantees
    // should kick in and ensure that construction is visible to
    // all threads.
    private AudioBypass(Context context) {
        if (context == null) {
            // not strictly necessary, as we'll blow up below,
            // but it's nice to have an explicit log message
            Log.e(TAG, "context must not be null");
            throw new NullPointerException("context arg");
        }

        mContext = context;
        mAssetMan = context.getAssets();

        mSoundPool = new SoundPool(MAX_STREAMS, STREAM_TYPE, SRC_QUALITY);

        Log.d(TAG, "AudioBypass created: " + context);
    }

    // Destroy the SoundPool and all associated resources.
    //
    // The AudioBypass object may not be used after this point.
    public void destroy() {
        synchronized (mLock) {
            mSoundPool.release();
            mSoundPool = null;

            mAssetMan = null;
            mContext = null;
        }
    }

    // Registers the sound file with the SoundPool.
    public int register(String soundFile) {
        final int LOAD_PRIORITY = 1;    // recommended value; does nothing

        Log.d(TAG, "Registering " + soundFile);
        synchronized (mLock) {
            AssetFileDescriptor afd;
            try {
                afd = mAssetMan.openFd(soundFile);
            } catch (IOException ioe) {
                Log.w(TAG, "Failed to load " + soundFile + ": " + ioe);
                return -1;
            }

            int id = mSoundPool.load(afd, LOAD_PRIORITY);
            Log.d(TAG, " --> " + id);
            return id;
        }
    }

    // Plays the sound through SoundPool.
    public int play(int soundId, float leftVolume, float rightVolume,
            int priority, int loop, float rate) {
        Log.d(TAG, "Playing " + soundId + ": lv=" + leftVolume +
                " rv=" + rightVolume + " pr=" + priority + " lp=" + loop +
                " rt=" + rate);
        synchronized (mLock) {
            int streamId = mSoundPool.play(soundId, leftVolume, rightVolume, priority,
                    loop, rate);
            Log.d(TAG, " --> streamId=" + streamId);
            return streamId;
        }
    }
}

