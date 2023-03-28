using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public MenuState menuState = MenuState.MainMenu;
    public GameObject mainMenu;
    public RectTransform mainMenuUI;
    public GameObject settingsMenu;
    public RectTransform settingsMenuUI;
    public GameObject shopMenu;
    public RectTransform shopMenuUI;
    public GameObject highScoreMenu;
    public RectTransform highScoreMenuUI;

    public float transitionTime = 1f;
    public Slider sensitivitySlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        Application.targetFrameRate = 60;
        MusicManager.Instance.ChangeMode(true);
        mainMenu.SetActive(true);
        mainMenuUI.gameObject.SetActive(true);
        settingsMenu.SetActive(false);
        settingsMenuUI.gameObject.SetActive(false);
        shopMenu.SetActive(false);
        shopMenuUI.gameObject.SetActive(false);
        highScoreMenu.SetActive(false);
        highScoreMenuUI.gameObject.SetActive(false);
        
        musicSlider.SetValueWithoutNotify( PlayerPrefs.GetFloat("MusVolume", 1f) );
        sfxSlider.SetValueWithoutNotify( PlayerPrefs.GetFloat("SFXVolume", 1f) );
        sensitivitySlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("sensitivity", 1f));

    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        switch (menuState)
        {
            case MenuState.MainMenu:
                Application.Quit();
                break;
            case MenuState.Settings or MenuState.HighScore or MenuState.Shop:
                SetMenuState(MenuState.MainMenu);
                break;
        }
    }
    
    public enum MenuState
    {
        MainMenu,
        Settings,
        Shop,
        HighScore
    }
    
    public void SetMenuState(MenuState state)
    {
        menuState = state;
        switch (state)
        {
            case MenuState.MainMenu:
                mainMenu.transform.position = new Vector3(50, 0, 0);
                mainMenu.SetActive(true);
                mainMenu.transform.DOMoveX(0, transitionTime).onComplete = () => mainMenuUI.gameObject.SetActive(true);
                
                settingsMenuUI.gameObject.SetActive(false);
                settingsMenu.transform.DOMoveX(-50, transitionTime).onComplete = () => settingsMenu.SetActive(false);
                
                shopMenuUI.gameObject.SetActive(false);
                shopMenu.transform.DOMoveX(-50, transitionTime).onComplete = () => shopMenu.SetActive(false);
                
                highScoreMenuUI.gameObject.SetActive(false);
                highScoreMenu.transform.DOMoveX(-50, transitionTime).onComplete = () => highScoreMenu.SetActive(false);
                break;
            case MenuState.Settings:
                mainMenuUI.gameObject.SetActive(false);
                mainMenu.transform.DOMoveX(50, transitionTime).onComplete = () => mainMenu.SetActive(false);
                
                settingsMenu.SetActive(true);
                settingsMenu.transform.position = new Vector3(-50, 0, 0);
                settingsMenu.transform.DOMoveX(0, transitionTime).onComplete = () => settingsMenuUI.gameObject.SetActive(true);
                break;
            case MenuState.Shop:
                mainMenuUI.gameObject.SetActive(false);
                mainMenu.transform.DOMoveX(50, transitionTime).onComplete = () => mainMenu.SetActive(false);
                shopMenu.SetActive(true);
                shopMenu.transform.position = new Vector3(-50, 0, 0);
                shopMenu.transform.DOMoveX(0, transitionTime).onComplete = () => shopMenuUI.gameObject.SetActive(true);
                break;
            case MenuState.HighScore:
                mainMenuUI.gameObject.SetActive(false);
                mainMenu.transform.DOMoveX(50, transitionTime).onComplete = () => mainMenu.SetActive(false);
                
                highScoreMenu.SetActive(true);
                highScoreMenu.transform.position = new Vector3(-50, 0, 0);
                highScoreMenu.transform.DOMoveX(0, transitionTime).onComplete = () => highScoreMenuUI.gameObject.SetActive(true);
                break;
        }
        SFXManager.Instance.PlayMenuClickSound();
    }
    
    public void SetMenuState(int state)
    {
        SetMenuState((MenuState)state);
    }
    
    public void Customize()
    {
        SetMenuState(MenuState.Shop);
    }
    
    public void Settings()
    {
        SetMenuState(MenuState.Settings);
    }
    
    public void Back()
    {
        SetMenuState(MenuState.MainMenu);
    }
    
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void UpdateSensitivity(float value)
    {
        PlayerPrefs.SetFloat("sensitivity", value);
    }
    
    public void HighScore()
    {
        SetMenuState(MenuState.HighScore);
    }
}
