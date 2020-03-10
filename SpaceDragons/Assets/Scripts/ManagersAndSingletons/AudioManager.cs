using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] Sound[] m_sounds = null;
    [SerializeField] AudioMixerGroup m_music = null;
    [SerializeField] AudioMixerGroup m_sfx = null;

    Dictionary<string, Sound> music = new Dictionary<string, Sound>();
    Dictionary<string, Sound> sfx = new Dictionary<string, Sound>();

    int curSongLoc = 0;
    bool isPaused;

    public override void Awake()
    {
        base.Awake();
        foreach (Sound sound in m_sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.outputAudioMixerGroup = sound.audioMixerGroup;
            sound.audioSource.volume = 1;
            sound.audioSource.pitch = 1;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.playOnAwake = true;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_music.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        m_sfx.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));

        foreach (Sound sound in m_sounds)
        {
            if (sound.audioName.Contains("Music"))
            {
                music.Add(sound.audioName, sound);
            }
            else
            {
                sfx.Add(sound.audioName, sound);
            }
        }

        StopAll();
        PlayRandomMusic("Battle");
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(m_sounds, s => s.audioName == name);
        if (sound != null)
        {
            if (sound.volume != 0)
            {
                sound.audioSource.Play();
            }
        }
    }
    public void Pause()
    {
        Sound sound = m_sounds[curSongLoc];
        if (sound != null)
        {
            sound.audioSource.Pause();
        }

        isPaused = true;
    }

    public void Resume()
    {
        Sound sound = m_sounds[curSongLoc];
        if (sound != null)
        {
            sound.audioSource.UnPause();
        }
        isPaused = false;
    }

    public void PlayRandomMusic(string contains)
    {
        List<string> randommusic = new List<string>();
        foreach (KeyValuePair<string, Sound> m in music)
        {
            if (m.Key.Contains(contains))
            {
                randommusic.Add(m.Value.audioName);   
            }
        }

        int num = UnityEngine.Random.Range(0, randommusic.Count);

        Play(randommusic[num]);
    }

    public void StopAll()
    {
        foreach (Sound sound in m_sounds)
        {
            sound.audioSource.Stop();
        }
    }

    public void SetSFXVolume(float volume)
    {
        m_sfx.audioMixer.SetFloat("SFXVolume", Mathf.Log(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", Mathf.Log(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume01", volume);
    }
    public void SetMusicVolume(float volume)
    {
        m_music.audioMixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume01", volume);

    }
}
