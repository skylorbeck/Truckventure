using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerModelLoader : MonoBehaviour
{
    public GameObject currentModel;
    public GameObject selectedText;
    public bool PreviewMode;
    public int modelIndex = 0;
    public TruckSkin[] skins;
    void Start()
    {
        if (!PreviewMode)
        {
            modelIndex = PlayerPrefs.GetInt("modelIndex", 0);
        } else
        {
            selectedText.SetActive(modelIndex == PlayerPrefs.GetInt("modelIndex"));
        }
        skins[modelIndex].Load();
        currentModel = Instantiate(skins[modelIndex].model, transform.position, transform.rotation, transform);
    }
    
    public void ReloadModel()
    {
        Destroy(currentModel);
        if (!PreviewMode)
        {
            modelIndex = PlayerPrefs.GetInt("modelIndex", 0);
        } 
        currentModel = Instantiate(skins[modelIndex].model, transform.position, transform.rotation, transform);
    }

    public void SelectModel()
    {
        selectedText.SetActive(true);
        PlayerPrefs.SetInt("modelIndex", modelIndex);
    }
    
    public void UnlockModel()
    {
        skins[modelIndex].Unlock();
    }

    public void Deselect()
    {
        selectedText.SetActive(false);
    }
}
