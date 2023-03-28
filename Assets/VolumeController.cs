using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    
    void Start()
    {
        UpdateVolume();
    }

    void UpdateVolume()
    {
        MusicManager.Instance.audioSource.volume = PlayerPrefs.GetFloat("MusVolume", 1f);
        SFXManager.Instance.audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
    
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusVolume", volume);
        UpdateVolume();
    }
    
    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        UpdateVolume();
    }
}
