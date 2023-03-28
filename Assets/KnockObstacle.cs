using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KnockObstacle : MonoBehaviour
{
    public Rigidbody rb;
    public Collider trigger;
    public Collider collider;
    public float force = 25f;
    public AudioClip[] knockSound;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collider.enabled = false;
            Vector3 direction = transform.position - other.transform.position + Vector3.up;
            rb.AddForce(direction * force * Random.Range(0.5f, 1.5f), ForceMode.Impulse);
            rb.AddTorque(-Vector3.Cross(Vector3.up, direction) * force * Random.Range(0.75f, 1.25f), ForceMode.Impulse);
            SFXManager.Instance.PlaySound(knockSound[Random.Range(0, knockSound.Length)]);
        }
    }
}