using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public bool isPaused = false;
    public GameObject pausePanel;
    public GameObject resumeButton;
    public GameObject restartButton;
    public GameObject quitButton;
    public Slider sensitivitySlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    
    
    void Start()
    {
        musicSlider.SetValueWithoutNotify( PlayerPrefs.GetFloat("MusVolume", 1f) );
        sfxSlider.SetValueWithoutNotify( PlayerPrefs.GetFloat("SFXVolume", 1f) );
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        // pauseButton.SetActive(false);
        resumeButton.SetActive(true);
        restartButton.SetActive(true);
        quitButton.SetActive(true);
        sensitivitySlider.gameObject.SetActive(true);
        musicSlider.gameObject.SetActive(true);
        sfxSlider.gameObject.SetActive(true);
        SFXManager.Instance.PlayMenuClickSound();

        Time.timeScale = 0;
    }
    
    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        // pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        restartButton.SetActive(false);
        quitButton.SetActive(false);
        sensitivitySlider.gameObject.SetActive(false);
        musicSlider.gameObject.SetActive(false);
        sfxSlider.gameObject.SetActive(false);
        SFXManager.Instance.PlayMenuClickSound();

        Time.timeScale = 1;
    }
    
    public void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Quit()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }
}
