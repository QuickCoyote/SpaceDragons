using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] Sound[] m_sounds = null;
    [SerializeField] AudioMixerGroup m_music = null;
    [SerializeField] AudioMixerGroup m_sfx = null;

    [SerializeField] Slider musicSlider = null;
    [SerializeField] Slider sfxSlider = null;

    [SerializeField] TextMeshProUGUI Music_Readout = null;
    [SerializeField] TextMeshProUGUI SFX_Readout = null;
    [SerializeField] GameObject UIDisplay = null;

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

        if (sfxSlider)
        {
            sfxSlider.minValue = .0001f;
            sfxSlider.maxValue = 1f;
        }
        if (musicSlider)
        {
            musicSlider.minValue = .0001f;
            musicSlider.maxValue = 1f;
        }

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
        PlayRandomMusic("Battle ");
    }

    public void NextSong()
    {
        StopAll();
        curSongLoc++;
        if (curSongLoc > m_sounds.Length - 1)
        {
            curSongLoc = 0;
        }
        PlayThrough();
    }
    public void PrevSong()
    {
        StopAll();
        curSongLoc--;
        if (curSongLoc < 0)
        {
            curSongLoc = m_sounds.Length - 1;
        }
        PlayThrough();
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

    public void PlayThrough()
    {
        StopAll();
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Sound sound = m_sounds[curSongLoc];
            if (sound != null)
            {
                if (sound.name.Contains("Music"))
                {
                    sound.audioSource.Play();
                }
            }
            //curMusicImage.sprite = sound.image;
            //musicSlider.maxValue = m_sounds[curSongLoc].audioClip.length;
            //string minSec = string.Format("{0}:{1:00}", (int)musicSlider.maxValue / 60, (int)musicSlider.maxValue % 60);
            //musicMaxTimeText.text = minSec;
            //musicNameText.text = m_sounds[curSongLoc].audioClip.name;
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

    private void Update()
    {

        if (curSongLoc > m_sounds.Length - 1)
        {
            curSongLoc = 0;
        }
        if (curSongLoc < 0)
        {
            curSongLoc = m_sounds.Length - 1;
        }
        if (!isPaused)
        {
            if (!m_sounds[curSongLoc].audioSource.isPlaying)
            {
                curSongLoc++;
                if (curSongLoc > m_sounds.Length - 1)
                {
                    curSongLoc = 0;
                }
                PlayThrough();
            }
            //musicSlider.value = m_sounds[curSongLoc].audioSource.time;
            //string minSec = string.Format("{0}:{1:00}", (int)musicSlider.value / 60, (int)musicSlider.value % 60);
            //musicTimeText.text = minSec;
        }
    }

    void PlayRandomMusic(string contains)
    {
        StopAll();
        if (contains != "")
        {
            Dictionary<string, Sound>.KeyCollection ss = music.Keys;

            foreach (string key in ss)
            {
                if (key.Contains(contains))
                {
                    Play(key);
                    return;
                }
            }
        }


        Dictionary<string, Sound>.KeyCollection stuffs = music.Keys;

        List<string> keys = new List<string>();

        foreach (string key in stuffs)
        {
            keys.Add(key);
        }

        int randNum;
        randNum = UnityEngine.Random.Range(0, keys.Count);

        Play(keys[randNum]);
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

    public void ToggleUIDisplay()
    {
        UIDisplay.SetActive(!UIDisplay.activeSelf);
    }

    public void SetSFXVolume(float volume)
    {
        m_sfx.audioMixer.SetFloat("SFXVolume", Mathf.Log(volume) * 20);
        SFX_Readout.text = (volume).ToString("00%");
    }
    public void SetMusicVolume(float volume)
    {
        m_music.audioMixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
        Music_Readout.text = (volume).ToString("00%");
    }
}
