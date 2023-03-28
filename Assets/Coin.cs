using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveSystem.runStats.coins++;
            SFXManager.Instance.PlayCoinSound();
        }
    }
}
