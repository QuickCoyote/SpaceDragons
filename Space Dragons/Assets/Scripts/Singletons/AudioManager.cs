using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] Slider SFX_Slider = null;
    [SerializeField] Slider Music_Slider = null;
    [SerializeField] TextMeshProUGUI Music_Readout = null;
    [SerializeField] TextMeshProUGUI SFX_Readout = null;
    [SerializeField] AudioMixer AudioMixer = null;
    [SerializeField] GameObject UIDisplay = null;

    [SerializeField] AudioSource defaultTrack = null;

    AudioSource currentTrack;

    void Start()
    {
        if (SFX_Slider)
        {
            SFX_Slider.minValue = .0001f;
            SFX_Slider.maxValue = 1f;
        }
        if (Music_Slider)
        {
            Music_Slider.minValue = .0001f;
            Music_Slider.maxValue = 1f;
        }
        currentTrack = defaultTrack;
        currentTrack.Play();
        UIDisplay.SetActive(false);

    }

    public void ToggleUIDisplay()
    {
        UIDisplay.SetActive(!UIDisplay.activeSelf);
    }

    public void SetSFXVolume(float volume)
    {
        AudioMixer.SetFloat("SFXVolume", Mathf.Log(volume) * 20);
        SFX_Readout.text = (volume * 100).ToString("00%");
    }
    public void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
        Music_Readout.text = (volume * 100).ToString("00%");
    }

    public void ReplaceCurrentTrack(AudioSource audiosource)
    {
        currentTrack.Stop();
        currentTrack = audiosource;
        currentTrack.Play();
    }
    public void ResumeDefaultTrack()
    {
        currentTrack.Stop();
        currentTrack = defaultTrack;
        currentTrack.Play();
    }


    
}
