using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPreviewer : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float timeSinceTouch = 0f;
    public float timeToPreview = 1f;
    public float sensitivity = 1f;
    public Vector3 mouseOrigin;
    // Start is called before the first frame update
    void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timeSinceTouch = 0f;
            Vector3 mousePos = Input.mousePosition;
            if (mouseOrigin == Vector3.zero)
            {
                mouseOrigin = mousePos;
            }
            else
            {
                float xDiff = mousePos.x - mouseOrigin.x;
                transform.Rotate(-Vector3.up * (xDiff * rotationSpeed * Time.deltaTime * sensitivity));
                mouseOrigin = mousePos;
            }
            
        } else if (timeSinceTouch < timeToPreview)
        {
            mouseOrigin = Vector3.zero;
            timeSinceTouch += Time.deltaTime;
        }
        else
        {
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
        }
    }

    public void FixedUpdate()
    {
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 1f);
    }
}
