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
using UnityEngine;
using System;
using System.Collections;

namespace AndroidAudioBypass {

    /// <summary>
    /// Manages the interaction with the Android SoundPool object.
    /// </summary>
    ///
    /// There should only be one of these, but that's not strictly required.
    /// The underlying Java-land code will get upset if it's initialized twice
    /// with different contexts, but since we're passing in the current Activity
    /// that shouldn't happen.
    ///
    /// This could be made a Unity-object singleton.  I don't want to make it a
    /// C# singleton because the Activity might change with scenes (not sure
    /// about that).
    public class BypassAudioManager : MonoBehaviour {
        private AndroidJavaObject activityContext;      // android.app.Activity; subclass of android.content.Context
        private AndroidJavaObject audioBypassInstance;  // com.faddensoft.androidaudiobypass.AudioBypass

        /// <summary>
        /// Creates an instance of the AudioBypass object, using the Unity JNI bridge.
        /// </summary>
        void Awake() {
            if (Application.platform != RuntimePlatform.Android) {
                Debug.LogError("BypassAudioManager only works on Android");
                throw new SystemException("This is not Android");
            }

            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            }

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.faddensoft.androidaudiobypass.AudioBypass")) {
                //Debug.Log("pluginClass is " + pluginClass);
                audioBypassInstance = pluginClass.CallStatic<AndroidJavaObject>("createInstance", activityContext);
            }

#if false   // debug stuff
        audioBypassInstance.Call("testMethod");

        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            audioBypassInstance.Call("testMethod");
        }));

        Debug.Log("Streaming assets path is '" + Application.streamingAssetsPath + "'");
#endif
        }

        /// <summary>
        /// Handles object shutdown.  We explicitly shut the SoundPool down to release
        /// resources without having to go through various garbage collection passes.
        /// </summary>
        void OnDestroy() {
            if (audioBypassInstance != null) {
                Debug.Log("Shutting down SoundPool");
                audioBypassInstance.Call("destroy");
                audioBypassInstance = null;
            }
        }

        /// <summary>
        /// Registers a sound file with the SoundPool.
        /// </summary>
        /// <returns>The sound ID to use on subsequent play() calls.</returns>
        /// <param name="soundFile">File name in StreamingAssets directory.</param>
        public int RegisterSoundFile(string soundFile) {
            if (audioBypassInstance == null) { return -1; }

            // TODO: if sound file is already registered, don't register it again.
            // Note that this potentially makes UnregisterSound() more complicated
            // as we would need to refcount.

            int soundId = audioBypassInstance.Call<int>("register", soundFile);
            Debug.Log("Registered " + soundFile + " as ID " + soundId);

            // TODO: the sound isn't necessarily ready to play at this point -- the
            // load mechanism is asynchronous.  We would need to tie into the
            // SoundPool OnLoadCompleteListener to figure out when sounds were
            // actually ready to go.

            return soundId;
        }

        public void UnregisterSound(int soundId) {
            // TODO -- should call through to SoundPool.unload(soundId)
        }

        /// <summary>
        /// Plays the specified sound.  Parameters are passed through to SoundPool.play().
        /// </summary>
        /// <param name="soundId">Sound identifier, returned earlier by the registration method.</param>
        /// <param name="leftVolume">Left volume, [0.0, 1.0].</param>
        /// <param name="rightVolume">Right volume, [0.0, 1.0].</param>
        /// <param name="priority">Priority (0 = lowest).</param>
        /// <param name="loop">Loop mode (0 = no loop, -1 = loop forever).</param>
        /// <param name="rate">Rate (1.0 = normal playback, [0.5, 2.0]).</param>
        public void PlaySound(int soundId, float leftVolume, float rightVolume, int priority, int loop, float rate) {
            if (audioBypassInstance == null) { return; }

            // It shouldn't matter which thread we call this from, so no need to
            // route through runOnUiThread().
            int streamId = audioBypassInstance.Call<int>("play", soundId, leftVolume, rightVolume,
                                                         priority, loop, rate);
            if (streamId == -1) {
                Debug.LogWarning("PlaySound id=" + soundId + " failed");

                // TODO: this is useful for sounds that are looping (so you
                // can stop them) or just very long, like music.  We should
                // return it and provide the relevant functions.
            }
        }
    }

} // namespace AndroidAudioBypass
