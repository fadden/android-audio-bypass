using UnityEngine;
using System.Collections;

public class BypassSoundPlayer : MonoBehaviour {
	private AndroidJavaObject audioBypassInstance;

	void Start() {
		using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.faddensoft.androidaudiobypass.AudioBypass")) {
			Debug.Log("pluginClass is " + pluginClass);
			audioBypassInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
		}

		audioBypassInstance.Call("testMethod");
	}
	
}
