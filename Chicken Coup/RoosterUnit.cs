using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoosterUnit : Unit
{
    // VARIABLES AND REFERENCES
    Rigidbody rb;
    Animator anim;
    AudioSource audioSource;
    UnitManager um;
    [SerializeField] AudioClip fireSFX, deathSFX;

    [SerializeField] GameObject gunSwivel;
    [SerializeField] GameObject muzzlePoint;
    [SerializeField] GameObject bulletPrefab;
    bool canFire = true;
    float fireRate = 0.2f;
    [SerializeField] float bulletSpeed;
    float xInput, yInput;
    Vector3 inputVector;
    [SerializeField] float movePower;
    [SerializeField] int maxVelocity;
    int currentHealth, maxHealth;
    bool gameWon = false;

    // UNITY EVENTS
    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            Death();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WinZone") && !gameWon)
        {
            // Victory!
            gameWon = true;
            Debug.Log("You win!");
        }
    }


    // OVERRIDES
    #region Overrides
    public override void Example()
    {
        Debug.Log("Kukkuluuruu");
    }

    public override void Aim()
    {
        if (isDead) { return; }

        // Rotate body towards movement, if moving
        if (inputVector.normalized.magnitude >= Mathf.Epsilon)
        {
            Quaternion bodyRotation = Quaternion.LookRotation(inputVector, Vector3.up);
            Quaternion smoothedRotation = Quaternion.Slerp(rb.rotation, bodyRotation, Time.deltaTime * 5f);
            rb.MoveRotation(smoothedRotation);
        }
        
        // Rotate swivel towards crosshair
        Vector3 lookVector = Crosshair.instance.transform.position - gunSwivel.transform.position;
        Quaternion swivelRotation = Quaternion.LookRotation(lookVector, Vector3.up);
        gunSwivel.transform.rotation = Quaternion.Slerp(gunSwivel.transform.rotation, swivelRotation, Time.deltaTime * 5f);

    }
    public override void Fire()
    {
        // Fire bullet in the direction of the muzzle
        if (!isDead && canFire)
        {
            StartCoroutine(FiringSystem());
        }

    }

    public override void GetInputs()
    {
        if (isDead) { inputVector = Vector3.zero; return; }

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        inputVector = new Vector3(xInput, 0, yInput);
    }

    public override void Movement()
    {
        if (isDead) { return; }

        rb.AddForce(inputVector.normalized * movePower, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public override void TakeDamage(int pDamage)
    {
        currentHealth -= pDamage;

        if (currentHealth <= 0) { Death(); }
    }
    public override void Death()
    {
        StartCoroutine(DeathSequence());
    }
    #endregion
    // CLASS METHODS

    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        um = FindObjectOfType<UnitManager>();

        isDead = false;
        currentHealth = maxHealth;

        anim.speed = Random.Range(1f, 2.5f);
        fireRate = fireRate * Random.Range(0.9f, 1.1f);
    }

    IEnumerator FiringSystem()
    {
        // bool blocker
        canFire = false;
        // fire loop
        while (!isDead && Input.GetButton("Fire1"))
        {
            // Firing logic
            var newBullet = Instantiate(bulletPrefab, muzzlePoint.transform.position, Quaternion.identity);
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.AddForce(gunSwivel.transform.forward.normalized * bulletSpeed, ForceMode.Impulse);
            gunSwivel.transform.DOPunchPosition((Crosshair.instance.transform.position + gunSwivel.transform.position).normalized * 0.2f, fireRate * 0.5f);
            audioSource.pitch = 1f + Random.Range(-0.05f, 0.05f);
            audioSource.PlayOneShot(fireSFX, 0.5f);
            // fire rate wait
            yield return new WaitForSeconds(fireRate);
        }
        // reset blocker
        canFire = true;
    }

    IEnumerator DeathSequence()
    {
        float fadeTime = 1f;
        // bool blocker
        if (isDead)
        {
            yield break;
        }

        isDead = true;
        //UnitManager.instance.units.Remove(this);
        um.units.Remove(this);
        rb.constraints = RigidbodyConstraints.None;
        anim.speed = 0f;
        audioSource.PlayOneShot(deathSFX, 0.5f);
        transform.DOScale(Mathf.Epsilon, fadeTime);
        yield return new WaitForSeconds(fadeTime * 4f);
        gameObject.SetActive(false);
    }

}
