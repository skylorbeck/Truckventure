using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CarController : MonoBehaviour
{
    public float horizontalSpeed = 0.25f;
    public ForceMode forceMode = ForceMode.Force;
    public float rotationSpeed = 5f;
    public float maxAngle = 15f;
    public bool isGrounded = false;
    public Vector2 origin = Vector2.zero;

    public Rigidbody rb;

    public Transform car;
    public MeshRenderer carRenderer;

    public CameraController cam;

    public CopCar copCar;
    public float copSpeed = 1f;
    public float copRatio = 0.1f;
    public float health = 100f;
    public float healthMax = 50;
    public bool isAlive => health > 0;
    public float healthRegen = 1f;
    public float healthRegenTimer = 0f;
    public float healthRegenDelay = 3f;
    public float invincibleTimer = 0f;
    public float invincibleDelay = 1f;
    public int flashCount = 5;

    public Vector3 targetPosition;

    public List<Image> healthBars;

    public float sensitivity = 1f;
    public Slider sensitivitySlider;

    public AudioSource skidSound;

    public void Start()
    {
        sensitivitySlider.SetValueWithoutNotify(sensitivity = PlayerPrefs.GetFloat("sensitivity", 1f));
        car = transform.GetChild(0);
        carRenderer = car.GetChild(0).GetComponent<MeshRenderer>();
    }

    public void OnMouseDownC()
    {
        origin = Input.mousePosition;
    }

    public void OnMouseDragC()
    {
        Vector2 currentPosition = Input.mousePosition;
        Vector2 direction = currentPosition - origin;
        origin = currentPosition;
        direction.x *= sensitivity;
        Vector3 target = rb.position + new Vector3(direction.x, 0, 0);
        target.z = 0;
        targetPosition = target;
        car.Rotate(new Vector3(0, direction.x * 1.5f, 0) * (rotationSpeed * 0.1f * Time.smoothDeltaTime));
    }

    public void OnMouseUpC()
    {
        targetPosition = rb.position;
        // origin = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            if (targetPosition.magnitude - rb.position.magnitude > 0.1f)
            {
                rb.AddForce((targetPosition - rb.position) * horizontalSpeed, forceMode);
            }

            car.rotation =
                Quaternion.Lerp(car.rotation, transform.rotation, Time.fixedDeltaTime * rotationSpeed * 0.1f);



            isGrounded = Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.forward - Vector3.up),
                out RaycastHit hit, 2f);

            if (isGrounded)
            {
                var angle = Vector3.Angle(transform.up, hit.normal);
                if (angle > maxAngle)
                {
                    var axis = Vector3.Cross(transform.up, hit.normal);
                    rb.AddTorque(axis * (angle * rotationSpeed));
                }
            }
            else
            {
                var angle = Vector3.Angle(transform.up, Vector3.up);
                if (angle > maxAngle)
                {
                    var axis = Vector3.Cross(transform.up, Vector3.up);
                    rb.AddTorque(axis * (angle * rotationSpeed));
                }

            }


            if (invincibleTimer > 0)
            {
                invincibleTimer -= Time.fixedDeltaTime;
            }

            if (healthRegenTimer > 0)
            {
                healthRegenTimer -= Time.fixedDeltaTime;
            }
            else if (health < healthMax)
            {
                health += healthRegen * Time.fixedDeltaTime;
            }

            if (health > healthMax)
            {
                health = healthMax;
            }

        }

        skidSound.volume = (Mathf.Abs(car.rotation.y) > 0.15f && isGrounded && isAlive? Mathf.Lerp(skidSound.volume,SFXManager.Instance.audioSource.volume,Time.fixedDeltaTime*10) : 0);

        Vector3 copDestination = Vector3.Slerp(copCar.transform.position, transform.position + new Vector3(
            (isAlive ? Mathf.Sin(Time.time) : 0) * 2f *
            (health / healthMax), 0, (-health * copRatio) - 5), Time.fixedDeltaTime * copSpeed);
        copDestination.y =
            Physics.Raycast(new Ray(copCar.transform.position + Vector3.up, Vector3.down + Vector3.forward),
                out RaycastHit copHit, 10f)
                ? copHit.point.y
                : car.position.y;
        copCar.transform.position = copDestination;
        copCar.transform.rotation = Quaternion.Slerp(copCar.transform.rotation,
            copCar.isParked && !isAlive ? Quaternion.Euler(0, -45, 0) : car.rotation, Time.fixedDeltaTime * copSpeed);
        //increase volume as health decreases
        copCar.siren.volume = Mathf.Lerp(copCar.siren.volume,
            isAlive ? SFXManager.Instance.audioSource.volume * (1 - (health / healthMax)) : 0,
            Time.fixedDeltaTime * copSpeed);
    }

    public void Update()
    {
        if (!isAlive) return;
        if (Input.GetMouseButtonDown(0)) OnMouseDownC();
        if (Input.GetMouseButton(0)) OnMouseDragC();
        else OnMouseUpC();
        if (Input.GetMouseButtonUp(0)) OnMouseUpC();
    }

    public void LateUpdate()
    {
        rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles.x, 0, rb.rotation.eulerAngles.z);
        Vector3 pos = transform.position;
        // pos.z = Mathf.Clamp(pos.z, -1, 1);
        pos.z = 0;
        transform.position = pos;

        healthBars.ForEach(h => h.fillAmount = health / healthMax);

    }

    public void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }

    public void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.collider.CompareTag("Wall"))
        {
            // rb.AddForce((transform.position - collisionInfo.contacts[0].point) * horizontalSpeed, forceMode);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.LogWarning("TODO Finish");
            transform.position = RoadManager.Instance.outOfBoundsCube2.transform.position;
            rb.velocity = Vector3.zero;
            targetPosition = RoadManager.Instance.outOfBoundsCube2.transform.position;
            rb.angularVelocity = Vector3.zero;
            Hurt(40);
        }
    }

    public void Hurt(float damage)
    {
        if (invincibleTimer > 0) return;
        RoadManager.Instance.ResetCombo();
        health -= damage;
        healthRegenTimer = healthRegenDelay;
        invincibleTimer = invincibleDelay;
        StartCoroutine(Flash());

        if (!(health <= 0)) return;
        health = 0;
        targetPosition = rb.position;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;

        SaveSystem.ProcessRunEnd();
        GameOverDisplayer.Instance.DisplayGameOver();
    }

    public IEnumerator Flash()
    {

        float flashDelay = (invincibleDelay / flashCount) * 0.5f;
        for (int i = 0; i < flashCount; i++)
        {
            carRenderer.enabled = false;
            yield return new WaitForSeconds(flashDelay);
            carRenderer.enabled = true;
            yield return new WaitForSeconds(flashDelay);
        }
    }

    public void UpdateSensitivity(float value)
    {
        sensitivity = value;
        PlayerPrefs.SetFloat("sensitivity", value);
    }
}