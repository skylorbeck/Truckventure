using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScorePage : MonoBehaviour
{
    public GameObject contentPanel;
    public GameObject highScoreEntryPrefab;
    
    public void Start()
    {
        CreateHighScoreList();
    }

    private void CreateHighScoreList()
    {
        SaveSystem.saveFile.pastRuns.Sort( (a, b) => b.score.CompareTo(a.score) );
        foreach (var t in SaveSystem.saveFile.pastRuns)
        {
            GameObject entry = Instantiate(highScoreEntryPrefab, contentPanel.transform);
            entry.GetComponent<HighScoreEntry>().SetScore(t);
        }
    }
}
