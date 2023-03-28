using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HurtObstacle : MonoBehaviour
{
    public AudioClip[] hurtSound;
    public float damage = 1f;
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.GetComponents<Collider>().ToList().ForEach(c => c.enabled = false);
            collision.gameObject.GetComponent<CarController>().Hurt(damage);
            SFXManager.Instance.PlaySound(hurtSound[UnityEngine.Random.Range(0, hurtSound.Length)]);
        }
    }
}
