using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopCar : MonoBehaviour
{
    public bool isParked = false;
    
    public GameObject redLight;
    public GameObject blueLight;
    public float lightFlashSpeed = 0.5f;
    public float lightFlash = 0;
    
    public AudioSource siren;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isParked = true;
        }
    }

    public void FixedUpdate()
    {
        lightFlash += Time.fixedDeltaTime * (isParked ? 1 : 2);
        if (lightFlash > lightFlashSpeed)
        {
            lightFlash = 0;
            redLight.SetActive(!redLight.activeSelf);
            blueLight.SetActive(!blueLight.activeSelf);
        }
        
        
    }
}
