/* Author: Jaykin Lee
Date: 15/2/2025
Desc:
- Stores data of audio files into an array
- In Inspector, allows to control volume and whether or not to loop
*/

using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;


	public bool loop = false;


	[HideInInspector]
	public AudioSource source;

}
