using UnityEngine;
using System.Collections;

public class BypassAudioSource : MonoBehaviour {
	public BypassAudioManager m_bypassAudioManager;
	public string m_audioFile;

	public void Play() {
		m_bypassAudioManager.Play(m_audioFile);
	}
}
