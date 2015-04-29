using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the interaction with the Java-land audio player.
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
	private AndroidJavaObject activityContext;		// android.app.Activity; subclass of android.content.Context
	private AndroidJavaObject audioBypassInstance;	// com.faddensoft.androidaudiobypass.AudioBypass

	void Start() {
		using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
		}

		using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.faddensoft.androidaudiobypass.AudioBypass")) {
			Debug.Log("pluginClass is " + pluginClass);
			audioBypassInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
		}

		audioBypassInstance.Call("initialize", activityContext);

		if (true) {
			audioBypassInstance.Call("testMethod");
			
			activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
				audioBypassInstance.Call("testMethod");
			}));
			
			Debug.Log("Streaming assets path is '" + Application.streamingAssetsPath + "'");
		}
	}

	public void Play(string soundFile) {
		// It shouldn't matter which thread we call this from, so no need to
		// route through runOnUiThread().
		audioBypassInstance.Call("play", soundFile);
	}
}
