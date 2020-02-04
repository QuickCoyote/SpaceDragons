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

    Dictionary<String, Sound> music = new Dictionary<String, Sound>();
    Dictionary<String, Sound> sfx = new Dictionary<String, Sound>();

    int curSongLoc = 0;
    bool isPaused = false;
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
            if (sound.name.Contains("Music "))
            {
                music.Add(sound.name, sound);
            }
            else
            {
                sfx.Add(sound.name, sound);
            }
        }

        StopAll();
        PlayRandomMusic("Battle");
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(m_sounds, s => s.name == name);
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
        List<string> battleMusic = new List<string>();
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name.Contains(contains))
            {
                battleMusic.Add(m_sounds[i].name);   
            }
        }

        int num = UnityEngine.Random.Range(0, battleMusic.Count);

        Play(battleMusic[num]);
    }

    public void StopAll()
    {
        foreach (Sound sound in m_sounds)
        {
            sound.audioSource.Stop();
        }
    }

    public void ToggleMusic()
    {
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == "Music")
            {
                if (m_sounds[i].volume == 0)
                {
                    m_sounds[i].volume = 1;
                    m_sounds[i].audioSource.UnPause();
                }
                else
                {
                    m_sounds[i].volume = 0;
                    m_sounds[i].audioSource.Pause();
                }
            }
        }

    }

    public void ToggleSFX()
    {
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == "SoundFX")
            {
                if (m_sounds[i].volume == 0)
                {
                    m_sounds[i].volume = 1;
                }
                else
                {
                    m_sounds[i].volume = 0;
                }
            }
        }
    }

    public void SetSFXVolume(float volume)
    {
        m_sfx.audioMixer.SetFloat("SFXVolume", Mathf.Log(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        m_music.audioMixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
    }
}
