using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinWheel : MonoBehaviour
{
    public TextMeshProUGUI skinName;
    public TextMeshProUGUI skinPrice;
    public Button buyButton;
    public Button selectButton;
    public PlayerModelLoader[] playerActualModel;
    public TextMeshProUGUI coinsText;
    public List<PlayerModelLoader> modelLoaders;
    public float curModel = 0;
    public int curModelIndex = 0;
    public float distance = 2.5f;
    
    private void Start()
    {
        curModelIndex = PlayerPrefs.GetInt("modelIndex", 0);
        foreach (PlayerModelLoader loader in modelLoaders)
        {
            loader.transform.rotation = Quaternion.identity;
        }

        for (var i = 1; i < modelLoaders[0].skins.Length; i++)
        {
            var skin = playerActualModel[0].skins[i];
            PlayerModelLoader pml = Instantiate(modelLoaders[0], transform);
            pml.PreviewMode = true;
            pml.modelIndex = i;
            pml.transform.rotation = Quaternion.identity;
            pml.ReloadModel();
            modelLoaders.Add(pml);
        }

        UpdateText();
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        curModel = Mathf.Lerp(curModel, curModelIndex, Time.deltaTime * 5);
        for (var index = 0; index < modelLoaders.Count; index++)
        {
            var modelLoader = modelLoaders[index];
            
            Vector3 modelPos = modelLoader.transform.localPosition;
            modelPos.x = Mathf.Cos((index - curModel) * Mathf.PI / modelLoaders.Count*modelLoaders.Count*.5f) * distance;
            modelPos.z = Mathf.Sin((index - curModel) * Mathf.PI / modelLoaders.Count*modelLoaders.Count*.5f) * distance;
            modelPos.x -= distance;
            modelLoader.transform.localPosition = modelPos;
            
            Vector3 modelScale = modelLoader.transform.localScale;
            modelScale = Vector3.one * Mathf.Clamp01( (1 - Mathf.Abs(index - curModel) / modelLoaders.Count * modelLoaders.Count*.5f));
            modelLoader.transform.localScale = modelScale;
        }
    }
    
    public void NextModel()
    {
        curModelIndex = Mathf.Clamp(curModelIndex + 1, 0, modelLoaders.Count - 1);
        UpdateText();
        SFXManager.Instance.PlayMenuClickSound();
    }
    
    public void PrevModel()
    {
        curModelIndex = Mathf.Clamp(curModelIndex - 1, 0, modelLoaders.Count - 1);
        UpdateText();
        SFXManager.Instance.PlayMenuClickSound();
    }

    public void UpdateText()
    {
        coinsText.text = SaveSystem.saveFile.coins.ToString("N0");  
        skinName.text = modelLoaders[curModelIndex].skins[modelLoaders[curModelIndex].modelIndex].skinName;
        if (modelLoaders[curModelIndex].skins[modelLoaders[curModelIndex].modelIndex].isUnlocked)
        {
            selectButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
            skinPrice.text = "Unlocked";
        } else
        {
            selectButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            skinPrice.text = modelLoaders[curModelIndex].skins[modelLoaders[curModelIndex].modelIndex].price.ToString();
        }
    }
    
    public void BuyModel()
    {

        if (SaveSystem.saveFile.coins >= modelLoaders[curModelIndex].skins[modelLoaders[curModelIndex].modelIndex].price)
        {
            SFXManager.Instance.PlayMenuClickSound();
            SaveSystem.saveFile.coins -= modelLoaders[curModelIndex].skins[modelLoaders[curModelIndex].modelIndex].price;
            SaveSystem.SaveS();
            modelLoaders[curModelIndex].UnlockModel();
            UpdateText();
        }
    }
    
    public void SelectModel()
    {
        SFXManager.Instance.PlayMenuClickSound();
        modelLoaders.ForEach(loader => loader.Deselect());
        modelLoaders[curModelIndex].SelectModel();
        UpdateText();
        foreach (PlayerModelLoader loader in playerActualModel)
        {
            loader.ReloadModel();
        }
        PlayerPrefs.Save();
    }
}
