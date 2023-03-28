using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSpawner : MonoBehaviour
{
    public GameObject conePrefab;

    public float coneDistance = 0.5f;

    public uint lineLength = 9;

    void Start()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                SpawnLine(1);
                break;
            case 1:
                SpawnLine(2);
                break;
            case 2:
                SpawnLine(3);
                break;
            case 3:
                SpawnRandom();
                break;

        }
    }

    private void SpawnLine(int rows)
    {
        for (int r = 0; r < rows; r++)
        {
            for (int i = 0; i < lineLength; i++)
            {
                // offset by half the total width
                Vector3 position = new Vector3(r * coneDistance - (rows - 1) * coneDistance * 0.5f, 0,
                    i * coneDistance);
                Instantiate(conePrefab, transform).transform.localPosition = position;
            }
        }

    }

    private void SpawnRandom()
    {
        // Instantiate cones in a random pattern
        for (int i = 0; i < lineLength; i++)
        {
            Vector3 position = new Vector3(Random.Range(-2.5f, 2.5f), 0, Random.Range(-2.5f, 2.5f));
            Instantiate(conePrefab, transform).transform.localPosition = position;
        }
    }
    
}