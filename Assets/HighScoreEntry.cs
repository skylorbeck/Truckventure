using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighScoreEntry : MonoBehaviour, IPointerClickHandler
{
    public Tweener animation;
    public RunStats runStats;

    public TextMeshProUGUI scoreText; //also run number
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI comboText; //max combo
    public TextMeshProUGUI coinsText;

    public LayoutElement layoutElement;

    public float closedHeight = 250;
    public float openHeight = 800;


    public void SetScore(RunStats runStats)
    {
        this.runStats = runStats;
        UpdateText();
    }

    private void UpdateText()
    {
        scoreText.text = "Run " + runStats.runNumber + " : " + runStats.score.ToString("N0");
        dateText.text = DateTime.FromFileTime(runStats.dateTime).ToString("dd.MM.yyyy");
        distanceText.text = "Dist: "+runStats.distance.ToString("N0");
        comboText.text = "Combo: "+runStats.maxCombo.ToString("N0");
        coinsText.text = "Coins: "+runStats.coins.ToString("N0");
    }

    public void FadeTextIn()
    {
        dateText.DOComplete();
        distanceText.DOComplete();
        comboText.DOComplete();
        coinsText.DOComplete();

        dateText.DOFade(1, 0.5f);
        distanceText.DOFade(1, 0.5f);
        comboText.DOFade(1, 0.5f);
        coinsText.DOFade(1, 0.5f);
    }

    public void FadeTextOut()
    {
        dateText.DOComplete();
        distanceText.DOComplete();
        comboText.DOComplete();
        coinsText.DOComplete();

        dateText.DOFade(0, 0.5f);
        distanceText.DOFade(0, 0.5f);
        comboText.DOFade(0, 0.5f);
        coinsText.DOFade(0, 0.5f);
    }

    public void Expand()
    {
        animation?.Complete();
        animation = DOTween.To(() => layoutElement.preferredHeight, x => layoutElement.preferredHeight = x, openHeight,
            0.5f);
        FadeTextIn();
    }

    public void Collapse()
    {
        animation?.Complete();
        animation = DOTween.To(() => layoutElement.preferredHeight, x => layoutElement.preferredHeight = x,
            closedHeight, 0.5f);
        FadeTextOut();
    }

    public void Toggle()
    {
        if (Math.Abs(layoutElement.preferredHeight - closedHeight) < 0.05f)
        {
            Expand();
        }
        else
        {
            Collapse();
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }
}
