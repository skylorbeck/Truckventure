using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    public float distance = 10f;
    

    public float speed = 2f;
    
    public void Start()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
        }
    }

    public void FixedUpdate()
    {
        //move the camera forward with the target but keep it at the same height and x,y position
        var camPos = transform.position;
        var targetPos = target.transform.position;
        var targetCamPos = new Vector3(targetPos.x, targetPos.y+distance, targetPos.z - distance);
        transform.rotation = Quaternion.LookRotation(targetPos - targetCamPos);
        camPos = Vector3.Slerp( camPos,targetCamPos, Time.fixedDeltaTime * speed);
        transform.position = camPos;
    }
    
}
