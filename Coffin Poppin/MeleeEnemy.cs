using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeleeEnemy : MonoBehaviour
{
    public bool isDead = false;
    AudioSource sfx;
    [SerializeField] AudioClip deathSound;
    Rigidbody rb;
    TrailRenderer tr;
    Light lt;
    [SerializeField] float movePower;
    [SerializeField] float deathTime = 1f;
    GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        lt = GetComponent<Light>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        lt.DOIntensity(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            ApproachPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet") || collision.gameObject.CompareTag("Player") && !isDead)
        {
            Death();
        }
    }

    void ApproachPlayer()
    {
        // Rotate towards player
        rb.transform.LookAt(playerObject.transform.position, Vector3.up);
        // Approach player
        rb.AddRelativeForce(Vector3.forward * movePower, ForceMode.Impulse);
    }

    public void Death()
    {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence() {
        isDead = true;
        rb.detectCollisions = false;
        DOTween.Kill(this);
        PointSystem.AddPoints(1);
        Debug.Log("Hit!");
        sfx.PlayOneShot(deathSound, 0.2f);
        transform.DOScale(0f, deathTime);
        tr.DOTime(0f, deathTime);
        lt.DOIntensity(0f, deathTime);
        yield return new WaitForSeconds(deathTime);
        DOTween.Kill(this);
        Destroy(gameObject);
    }
}