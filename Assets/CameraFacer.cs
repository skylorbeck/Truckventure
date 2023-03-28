using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacer : MonoBehaviour
{
    Camera cam;
    void Start()
    {
        cam = Camera.main;
    }
    
    void Update()
    {
        // look away from the camera (the opposite direction)
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
