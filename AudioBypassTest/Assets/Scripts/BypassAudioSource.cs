using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// AudioSource equivalent for the Android bypass mechanism.
/// </summary>
public class BypassAudioSource : MonoBehaviour {
	public BypassAudioManager m_bypassAudioManager;
	public string m_audioFile;
	public int m_priority = 1;			// stream priority (0 = lowest)
	public float m_volume = 1.0f;		// volume [0.0, 1.0]
	public float m_rate = 1.0f;			// playback rate (1.0 = normal; [0.5, 2.0])
	public bool m_playOnAwake = false;
	public bool m_loop = false;

	private int m_soundId;

	public void Start() {
		if (String.IsNullOrEmpty(m_audioFile)) {
			Debug.LogError("Audio file not specified in " + gameObject.name);
			return;
		}
		m_soundId = m_bypassAudioManager.RegisterSoundFile(m_audioFile);

		if (m_playOnAwake) {
			Play();
		}
	}

	public void Play() {
		if (m_rate < 0.5f) {
			m_rate = 0.5f;
		}
		if (m_rate > 2.0f) {
			m_rate = 2.0f;
		}

		m_bypassAudioManager.PlaySound(m_soundId, m_volume, m_volume, m_priority,
		                               m_loop ? -1 : 0, m_rate);
	}
}
