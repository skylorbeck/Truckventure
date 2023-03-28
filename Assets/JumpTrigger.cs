using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    public float jumpForce = 500;
    public ForceMode forceMode = ForceMode.Impulse;
    public AudioClip[] jumpSound;
   public void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
            other.attachedRigidbody.AddForce(Vector3.up * jumpForce, forceMode);
            SFXManager.Instance.PlaySound(jumpSound[UnityEngine.Random.Range(0, jumpSound.Length)]);
      }
   }
}
