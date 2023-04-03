using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DamageNumbersPro;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoadManager : MonoBehaviour
{
    public static RoadManager Instance;
    public CameraController cam;
    public Camera mainCam;
    public RoadSegment startingSegment;
    public List<RoadSegment> activeSegments = new List<RoadSegment>();
    public float speed = 20;
    public float acceleration = .1f;
    public float maxSpeed = 35;
    public CarController car;
    public TextMeshProUGUI scoreText;
    public bool UIScore = true;
    public DamageNumberMesh damageNumberMesh;
    public DamageNumberGUI damageNumberGUI;
    public Canvas canvas;
    public float comboTime = 2f;
    public float comboTimer;
    public Image comboBar;
    public TextMeshProUGUI comboText;

    public Biome biome;
    public List<Biome> biomes = new List<Biome>();
    public int piecesPassed = 0;
    public int piecesPerBiome = 5;

    public GameObject outOfBoundsCube;
    public GameObject outOfBoundsCube2;

    public int desiredSegmentCount = 3;

    public void Start()
    {
        Instance = this;
        // Input.backButtonLeavesApp=true;
        Application.targetFrameRate = 60;
        MusicManager.Instance.ChangeMode(false);
        RandomizeBiome();
        BeginRun();
    }

    private void BeginRun()
    {
        activeSegments.Clear();
        SaveSystem.NewRun();
        RoadSegment segment = Instantiate(startingSegment);
        activeSegments.Add(segment);
    }

    public void FixedUpdate()
    {
        if (!car.isAlive)
        {
            return;
        }
        speed += acceleration * Time.fixedDeltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);
        SaveSystem.runStats.score += speed * Time.fixedDeltaTime;
        SaveSystem.runStats.distance += speed * Time.fixedDeltaTime;
        comboTimer -= Time.fixedDeltaTime;
        comboBar.fillAmount = comboTimer / comboTime;
        if (comboTimer <= 0)
        {
            SaveSystem.runStats.comboMultiplier = 1f;
            comboText.text = "";
        }
        scoreText.text = SaveSystem.runStats.score.ToString("N0");
        cam.distance = activeSegments[0].camDistance;
        List<RoadSegment> segmentsToRemove = new List<RoadSegment>();
        activeSegments.ForEach(segment =>
        {
            var position = segment.transform.position;
            position = new Vector3(position.x, position.y, position.z - speed * Time.fixedDeltaTime);
            segment.transform.position = position;
            if (!segment.isPlayerOn)
            {
                segmentsToRemove.Add(segment);
            }
        });

        segmentsToRemove.ForEach(segment =>
        {
            activeSegments.Remove(segment);
            Destroy(segment.gameObject);
        });
        
        
        while (activeSegments.Count < desiredSegmentCount)
        {
            GenerateNextSegment();
        }

        outOfBoundsCube.transform.position = new Vector3(car.transform.position.x,activeSegments[0].transform.position.y -15f,car.transform.position.z);
        outOfBoundsCube2.transform.position = new Vector3(0,activeSegments[0].transform.position.y +15f,0);
    }
    
    public void GenerateNextSegment()
    {
        RoadSegment lastSegment = activeSegments[^1];
        RoadSegment nextSegment = Instantiate(lastSegment.linkedSegments[Random.Range(0, lastSegment.linkedSegments.Length)],transform);
        nextSegment.transform.position = lastSegment.endPoint.transform.position;
        activeSegments.Add(nextSegment);
        nextSegment.SpawnObstacles(biome.obstacles);
        nextSegment.SpawnBuildings(biome.buildings);
        piecesPassed++;
        if (piecesPassed >= piecesPerBiome)
        {
            piecesPassed = 0;
            RandomizeBiome();
        }
    }

    private void RandomizeBiome()
    {
        biome = biomes[Random.Range(0, biomes.Count)];
    }

    public void AddScore(float f,Vector3 position)
    {
        float scoreToAdd = f * SaveSystem.runStats.comboMultiplier;
        if (UIScore)
        {
            DamageNumber dnGUI = damageNumberGUI.Spawn(mainCam.WorldToScreenPoint(position), scoreToAdd);
            dnGUI.transform.SetParent(canvas.transform);
        }
        else
        {
            damageNumberMesh.Spawn(position, scoreToAdd);
        }
        SaveSystem.runStats.comboMultiplier += .1f;
        SaveSystem.runStats.combo++;
        SaveSystem.runStats.combo++;
        comboText.text ="x"+ SaveSystem.runStats.comboMultiplier.ToString("N1");
        comboTimer = comboTime;
        SaveSystem.runStats.score += scoreToAdd;
    }

    public void ResetCombo()
    {
        SaveSystem.runStats.comboMultiplier = 1f;
        comboText.text = "";
        comboTimer = 0f;
        if (SaveSystem.runStats.combo > SaveSystem.runStats.maxCombo)
        {
            SaveSystem.runStats.maxCombo = SaveSystem.runStats.combo;
        }
        SaveSystem.runStats.combo = 0;
    }
}
