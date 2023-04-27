using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 20;
    private float turboSpeedMultiplier = 7f;
    private GameObject focalPoint;
    //public ParticleSystem smoke;

    public bool turboAvailable;
    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    public float turboDuration = 3.0f;
    public int turboWait = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup
    private float boost = 10;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        turboAvailable = true;
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
       float verticalInput = Input.GetAxis("Vertical");
       playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 
        // Set powerup indicator position to beneath player

        Moveforward();
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    playerRb.AddForce(focalPoint.transform.forward * boost,ForceMode.Impulse);
        //}
    }

    private void Moveforward()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space) && turboAvailable)
        {
            playerRb.AddForce(focalPoint.transform.forward * boost, ForceMode.Impulse);
            TurboCooldown();
           // smoke.Play();
        }
        else
        {
            playerRb.AddForce(focalPoint.transform.forward * speed * verticalInput);
        }
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    IEnumerator TurboCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        turboAvailable = true;
    }


    IEnumerator PowerupCooldown() // Coroutine to count down powerup duration
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }


    private void OnCollisionEnter(Collision other) // If Player collides with enemy
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }
}
