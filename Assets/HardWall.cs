using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HardWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.MovePosition(collision.rigidbody.position + collision.contacts[0].normal * collision.contacts[0].separation);
        }
    }
}
