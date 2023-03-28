using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    
    public AudioSource audioSource;
    
    public AudioClip[] coinSound;
    public AudioClip menuClickSound;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayCoinSound()
    {
        PlaySound(coinSound[Random.Range(0, coinSound.Length)]);
    }
    
    public void PlayMenuClickSound()
    {
        PlaySound(menuClickSound);
    }
}
