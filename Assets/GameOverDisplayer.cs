using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverDisplayer : MonoBehaviour
{
    public static GameOverDisplayer Instance;
    public bool isGameOver = false;
    public Image gameOverPanel;
    public TextMeshProUGUI scoreNumberText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI coinsNumberText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI distanceNumberText;
    public TextMeshProUGUI maxComboText;
    public TextMeshProUGUI maxComboNumberText;
    
    public float fadeDuration = 0.5f;
    public float fadeAmount = .5f;
    
    public void Start()
    {
        Instance = this;
        HideGameOver();
    }
    
    public void DisplayGameOver()
    {
        isGameOver = true;
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.DOFade(0.75f, fadeDuration);
        scoreNumberText.text = SaveSystem.runStats.score.ToString("N0");
        coinsNumberText.text = SaveSystem.runStats.coins.ToString("N0");
        distanceNumberText.text = SaveSystem.runStats.distance.ToString("N0");
        maxComboNumberText.text = SaveSystem.runStats.maxCombo.ToString("N0");
        gameOverText.DOFade(fadeAmount, fadeDuration);
        finalScoreText.DOFade(fadeAmount, fadeDuration);
        scoreNumberText.DOFade(fadeAmount, fadeDuration);
        coinsText.DOFade(fadeAmount, fadeDuration);
        coinsNumberText.DOFade(fadeAmount, fadeDuration);
        distanceText.DOFade(fadeAmount, fadeDuration);
        distanceNumberText.DOFade(fadeAmount, fadeDuration);
        maxComboText.DOFade(fadeAmount, fadeDuration);
        maxComboNumberText.DOFade(fadeAmount, fadeDuration);
    }
    
    public void HideGameOver()
    {
        isGameOver = false;
        gameOverPanel.DOFade(0, fadeDuration);
        gameOverText.DOFade(0, fadeDuration);
        finalScoreText.DOFade(0, fadeDuration);
        scoreNumberText.DOFade(0, fadeDuration);
        coinsText.DOFade(0, fadeDuration);
        coinsNumberText.DOFade(0, fadeDuration);
        distanceText.DOFade(0, fadeDuration);
        distanceNumberText.DOFade(0, fadeDuration);
        maxComboText.DOFade(0, fadeDuration);
        maxComboNumberText.DOFade(0, fadeDuration);
    }

    public void Update()
    {
        if (isGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
