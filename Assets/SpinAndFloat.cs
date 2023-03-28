using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpinAndFloat : MonoBehaviour
{
    public bool isObtained = false;
    public float floatSpeed = 1f;
    public float floatHeight = 1f;
    public float spinSpeed = 10f;
    public float obtainedMultiplier = 2f;
    public float obtainedSpinSpeed = 720f;
    public float obtainedTime = .5f;
    
    public void FixedUpdate()
    {
        if (isObtained)return;
        transform.Rotate(new Vector3(0, spinSpeed * Time.fixedDeltaTime, 0));
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight, transform.position.z);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isObtained = true;
            transform.DORotate(new Vector3(0, obtainedSpinSpeed, 0), obtainedTime, RotateMode.FastBeyond360);
            transform.DOMoveY(transform.position.y+1f, obtainedTime);
            transform.DOScale(transform.localScale.x * obtainedMultiplier, obtainedTime);
        }
    }
}
