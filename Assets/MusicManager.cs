using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public bool mainMenu = true;
    public AudioSource audioSource;
    
    public List<AudioClip> musicTracks;
    public List<AudioClip> menuTracks;
    
    public int currentTrack = 0;
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            RandomizeOrder();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    
    public void PlayMenuMusic()
    {
        PlayMusic(menuTracks[Random.Range(0, menuTracks.Count)]);
    }
    
    public void PlayGameMusic()
    {
        PlayMusic(musicTracks[currentTrack]);
    }

    public void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }
    
    public void ChangeMode(bool mainMenu)
    {
        audioSource.Stop();
        RandomizeOrder();
        this.mainMenu = mainMenu;
        if (mainMenu)
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameMusic();
        }
    }

    public void PlayNextTrack()
    {
        currentTrack++;
        if (mainMenu)
        {
            if (currentTrack >= menuTracks.Count)
            {
                currentTrack = 0;
                RandomizeOrder();
            }
            PlayMenuMusic();
            return;
        }
        
        if (currentTrack >= musicTracks.Count)
        {
            currentTrack = 0;
            RandomizeOrder();
        }
        PlayMusic(musicTracks[currentTrack]);
    }

    public void RandomizeOrder()
    {
        musicTracks.Sort((_, _) => Random.Range(-1, 1));
        menuTracks.Sort((_, _) => Random.Range(-1, 1));
    }
}
