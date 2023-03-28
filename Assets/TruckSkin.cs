using System;
using UnityEngine;
[CreateAssetMenu(fileName = "TruckSkin", menuName = "TruckSkin", order = 0)]
public class TruckSkin: ScriptableObject
{
    public string skinName;
    public GameObject model;
    public uint price;
    public bool isUnlocked;
    
    public void Load()
    {
        if (skinName.Equals("truck"))
        {
            PlayerPrefs.SetInt(skinName, 1);
        }
        isUnlocked = PlayerPrefs.GetInt(skinName, 0) == 1;
    }
    
    public void Unlock()
    {
        isUnlocked = true;
        PlayerPrefs.SetInt(skinName, 1);
    }
    
    public void Lock()
    {
        isUnlocked = false;
        PlayerPrefs.SetInt(skinName, 0);
    }
}