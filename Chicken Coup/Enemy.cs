using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    TrailRenderer trailRenderer;
    Light lg;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField] float moveSpeed;
    Unit targetUnit;
    bool isDead = false;
    int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        trailRenderer = GetComponent<TrailRenderer>();
        lg = GetComponent<Light>();
        targetUnit = FindAnyObjectByType<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float maxVelocity = 20f;
        float scareDistance = 4f;
        Vector3 movementVector;

        if (targetUnit.gameObject.activeInHierarchy == false)
        {
            targetUnit = FindAnyObjectByType<Unit>();
        }

        // Determine behavior
        if (Vector3.Distance(transform.position, Crosshair.instance.transform.position) <= scareDistance)
        {
            // Evasive maneuvers
            movementVector = transform.position + Crosshair.instance.transform.position;
        } else
        {
            // Chase target
            movementVector = targetUnit.transform.position - transform.position;
        }

        // apply physics
        rb.AddForce(movementVector.normalized * moveSpeed, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet")) {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if (isDead) { return; }

        health--;
        audioSource.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)], 0.5f);
        if (health < 0) {
            StartCoroutine(DeathSequence());
        } else
        {
            
        }
    }

    IEnumerator DeathSequence()
    {
        if (isDead) { yield break; }

        isDead = true;
        float fadeTime = 1f;
        rb.detectCollisions = false;
        audioSource.PlayOneShot(deathSFX, 0.5f);
        transform.DOScale(0f, fadeTime);
        trailRenderer.DOTime(0f, fadeTime);
        lg.DOIntensity(0f, fadeTime);
        yield return new WaitForSeconds(fadeTime * 4f);

        SpawnSystem.instance.SpawnEnemy();
        Destroy(gameObject);
    }

}
