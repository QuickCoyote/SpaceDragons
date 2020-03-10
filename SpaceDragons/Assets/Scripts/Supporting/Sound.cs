using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string audioName = "";
	public AudioClip audioClip = null;
	public AudioMixerGroup audioMixerGroup = null;
	[Range(0.0f, 1.0f)] public float volume = 1.0f;
	[Range(0.01f, 3.0f)] public float pitch = 1.0f;
	public bool loop = false;
    public Sprite image = null;
	

	[HideInInspector] public AudioSource audioSource = null;
}
