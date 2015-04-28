using UnityEngine;
using System.Collections;

public class BypassSoundPlayer : MonoBehaviour {
	private AndroidJavaObject activityContext;
	private AndroidJavaObject audioBypassInstance;

	void Start() {
		using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
		}

		using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.faddensoft.androidaudiobypass.AudioBypass")) {
			Debug.Log("pluginClass is " + pluginClass);
			audioBypassInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
		}

		audioBypassInstance.Call("testMethod");

		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			audioBypassInstance.Call("testMethod");
		}));
	}
}
