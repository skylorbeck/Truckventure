using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RoadSegment : MonoBehaviour
{
    public RoadSegment[] linkedSegments;
    public GameObject[] obstacleSpawnPoints;
    public GameObject[] buildingSpawnPointsLeft;
    public GameObject[] buildingSpawnPointsRight;
    
    public GameObject endPoint;

    public float camDistance = 5f;
    
    public bool isPlayerOn = true;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOn = false;
        }
    }
    
    public void SpawnObstacles(GameObject[] linkedObstacles)
    {
        if (obstacleSpawnPoints.Length == 0) return;
        if (linkedObstacles.Length == 0) return;
        foreach (var spawnPoint in obstacleSpawnPoints)
        {
            if (Random.Range(0, 2) == 0)
            {
                GameObject obstacle = Instantiate(linkedObstacles[Random.Range(0, linkedObstacles.Length)], transform);
                var position = spawnPoint.transform.position;
                position.x += Random.Range(-.5f, .5f);
                position.z += Random.Range(-.5f, .5f);
                obstacle.transform.position = position;
                // obstacle.SetActive(true);
            }

            spawnPoint.SetActive(false);
        }
    }

    public void SpawnBuildings(GameObject[] linkedBuildings)
    {
        foreach (GameObject spawnPoint in buildingSpawnPointsLeft)
        {
            GameObject building = Instantiate(linkedBuildings[Random.Range(0, linkedBuildings.Length)], transform);
            building.transform.position = spawnPoint.transform.position;
            building.SetActive(true);
            spawnPoint.SetActive(false);
        }
        
        foreach (GameObject spawnPoint in buildingSpawnPointsRight)
        {
            GameObject building = Instantiate(linkedBuildings[Random.Range(0, linkedBuildings.Length)], transform);
            building.transform.position = spawnPoint.transform.position;
            building.transform.rotation = Quaternion.Euler(0,building.transform.rotation.eulerAngles.y - 180, 0);
            building.SetActive(true);
            spawnPoint.SetActive(false);
        }
    }
}