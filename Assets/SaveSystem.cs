using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    public Image saveIcon;
    public TextMeshProUGUI saveText;
    public bool loaded = false;
    [SerializeField] private SaveFile saveFileActual;
    public static SaveFile saveFile => Instance.saveFileActual;
    [SerializeField] private RunStats runStatsActual;
    public static RunStats runStats => Instance.runStatsActual;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Save()
    {
        Save("savefile", saveFileActual);/*
        saveIcon.DOKill();
        saveText.DOKill();
        saveIcon.DOFade(1, 0.1f).OnComplete(() => saveIcon.DOFade(0, 1f).SetEase(Ease.OutFlash));
        saveText.DOFade(1, 0.1f).OnComplete(() => saveText.DOFade(0, 1f).SetEase(Ease.OutFlash));*/
    }

    public static void SaveS()
    {
        Instance.Save();
    }

    public void Load()
    {
        Load("savefile", out saveFileActual);
        /*if (saveFile.saveVersion<1)
        {
            Reset();
            saveFile.saveVersion = 1;
        }*/
        

        loaded = true;
    }

    private void Load<T>(string path, out T data) where T : new()
    {
        path = Application.persistentDataPath + "/" + path + ".json";
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
            data = new T();
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }
        else
        {
            string json = File.ReadAllText(path);
            if (json == "")
            {
                Debug.Log("File" + path + " is empty");
                data = new T();
                json = JsonUtility.ToJson(data);
                File.WriteAllText(path, json);
            }
            else
            {
                data = JsonUtility.FromJson<T>(json);
            }
        }
    }

    private void Save<T>(string path, T data)
    {
        path = Application.persistentDataPath + "/" + path + ".json";
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }

        string json = JsonUtility.ToJson(data,true);
        File.WriteAllText(path, json);
    }

    public void Reset()
    {
        saveFileActual = new SaveFile();
    }

    public static void ProcessRunEnd()
    {
        saveFile.runNumber++;
        saveFile.pastRuns.Add(runStats);
        saveFile.coins += runStats.coins;
        if (saveFile.pastRuns.Count > 10)
        {
            saveFile.pastRuns.Sort((a, b) => b.score.CompareTo(a.score));
            saveFile.pastRuns.RemoveAt(saveFile.pastRuns.Count - 1);
        }
        SaveS();
    }

    public static void NewRun()
    {
        Instance.runStatsActual = new RunStats();
        Instance.runStatsActual.runNumber = saveFile.runNumber + 1;
    }
}
[Serializable]
public class SaveFile
{
    public int saveVersion = 1;
    public uint coins = 0;
    public List<RunStats> pastRuns = new List<RunStats>();
    public int runNumber = 0;
}

[Serializable]
public class RunStats
{
    public int runNumber = -1;
    public long dateTime = DateTime.Now.ToFileTime();
    public float score = 0;
    public float distance = 0;
    public float comboMultiplier = 1f;
    public uint combo = 0;
    public uint maxCombo = 0;
    public uint coins =0;
}