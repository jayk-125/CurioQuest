/* Author: Jaykin Lee
Date: 15/2/2025
Desc:
- Allows GameObject to play a sound
- Allows sound to be looped
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerScript : MonoBehaviour
{
	//references
	// public static SoundManager instance;

	// public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
		// //if there are no sounds, it destroys itself
		// if (instance != null)
		// {
		// 	Destroy(gameObject);
		// }
		// else
		// {
		// 	instance = this;
		// 	DontDestroyOnLoad(gameObject);
		// }

		//creates the sounds for later use
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			// s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string name) //sound filters (if viable)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
	// 	s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
	// 	s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

    s.source.Play();
	}
}
