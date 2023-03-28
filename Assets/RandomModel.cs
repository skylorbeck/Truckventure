using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModel : MonoBehaviour
{
    public GameObject[] models;
    void Start()
    {
        int randomIndex = Random.Range(0, models.Length);
        Instantiate(models[randomIndex], transform.position, transform.rotation, transform);
    }
}
