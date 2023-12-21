using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float forwardPower;
    [SerializeField] float turnPower;
    [SerializeField] float dashPower;
    [SerializeField] float dashTime;
    bool dashing = false;

    float turnInput;

    Rigidbody rb;
    AudioSource sfx;
    TrailRenderer tr;
    [SerializeField] SpawnSystem ss;

    [SerializeField] AudioSource[] sources;
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip bumpSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] GameObject turret;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float rotationSpeed;
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float recoil = 5f;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] Light muzzleLight;
    bool firing;
    bool gameOver = false;

    [SerializeField] GameObject cameraPivot;
    [SerializeField] GameObject crosshair;
    Vector3 mouseWorldPosition;
    Plane plane = new Plane(Vector3.up, 0);

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI recordText; 
    [SerializeField] Image fadePanel;

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody>();
        sfx = GetComponent<AudioSource>();
        tr = GetComponent<TrailRenderer>();
        // Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        // Scene starters
        PointSystem.ResetPoints();
        bgm.volume = 0f;
        bgm.DOFade(0.3f, 1f);
        fadePanel.fillAmount = 1f;
        fadePanel.DOFillAmount(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            GetInputs();
        }
        RestartSystem();
    }

    private void FixedUpdate()
    {
        if (!gameOver)
        {
            PhysicsMovement();
            UpdateScoreText();
        }
    }

    private void LateUpdate()
    {
        CameraFollow();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameOver && !dashing && collision.gameObject.CompareTag("EnemyBullet"))
        {
            Death();
        }
        else
        {
            sfx.PlayOneShot(bumpSound, 0.65f);
        }
    }

    // Class methods below

    void GetInputs()
    {
        // Coffin turning
        turnInput = Input.GetAxis("Horizontal");

        // Mouse position
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            mouseWorldPosition = ray.GetPoint(distance);
            crosshair.transform.position = mouseWorldPosition;
        }

        if (Input.GetButtonDown("Fire1") && !firing)
        {
            firing = true;
            StartCoroutine(FiringSystem());
        } 

        if (!dashing && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            PointSystem.ResetHighScore();
        }

    }

    IEnumerator Dash()
    {
        dashing = true;
        // Dash sfx here
        yield return new WaitForSeconds(dashTime);
        dashing = false;
    }

    void RestartSystem()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ChangeScene(1, 1f));
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ChangeScene(0, 1f));
        }
    }

    IEnumerator ChangeScene(int sceneNum, float fadeTime)
    {
        // Fade out, wait, load new scene based on parameters
        fadePanel.DOFillAmount(1f, fadeTime);
        bgm.DOFade(0f, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        DOTween.KillAll();
        SceneManager.LoadScene(sceneNum);
    }

    IEnumerator FiringSystem()
    {
        while (firing)
        {
            // Firing an object
            Vector3 shootDir = crosshair.transform.position - transform.position;
            Vector3 spawnPosition = bulletSpawnPoint.transform.position;
            var newBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);
            var bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.AddForce(shootDir.normalized * bulletSpeed, ForceMode.Impulse);
            sfx.PlayOneShot(fireSound, 0.2f);
            muzzleLight.DOIntensity(1f, fireRate * 0.1f);

            // Recoil
            rb.AddForce(-shootDir.normalized * recoil, ForceMode.Impulse);
            yield return new WaitForSeconds(fireRate * 0.5f);
            muzzleLight.DOIntensity(0f, fireRate * 0.1f);
            yield return new WaitForSeconds(fireRate * 0.5f);

            if (Input.GetButton("Fire1") == false)
            {
                firing = false;
            }
        }
    }

    void PhysicsMovement()
    {
        // Coffin turning
        Vector3 torqueVector = new Vector3(0, turnInput * turnPower, 0);
        if (torqueVector != Vector3.zero && !dashing)
        {
            rb.AddRelativeTorque(torqueVector, ForceMode.VelocityChange);
        }

        // Coffin forward movement
        if (dashing)
        {
            rb.AddRelativeForce(Vector3.forward * dashPower, ForceMode.VelocityChange);
        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * forwardPower, ForceMode.VelocityChange);
        }

        // Turret rotation
        Vector3 turretDir = transform.position + crosshair.transform.position;
        turret.transform.LookAt(crosshair.transform.position, Vector3.up);
    }

    void CameraFollow()
    {
        Vector3 cameraTarget = new Vector3(transform.position.x, 0, transform.position.z);
        cameraPivot.transform.position = Vector3.Lerp(cameraPivot.transform.position, cameraTarget, Time.deltaTime * 8);
    }

    public void Death()
    {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        float deathTime = 1f;
        gameOver = true;
        ss.isRunning = false;
        // Check and set High Score
        if (PointSystem.GetPoints() > PointSystem.GetHighScore())
        {
            // New record!
            PointSystem.SetHighScore(PointSystem.GetPoints());
            // High score sound effect jingle?
            sfx.PlayOneShot(winSound, 0.25f);
            // Show highscore text
            recordText.gameObject.SetActive(true);
            UpdateScoreText();
        }

        rb.detectCollisions = false;
        sfx.PlayOneShot(deathSound, 1f);
        transform.DOPunchScale(Vector3.one * 2, 0.2f);
        tr.DOTime(0f, 0.25f);
        yield return new WaitForSeconds(0.2f);
        gameOverText.gameObject.SetActive(true);
        transform.DOScale(Vector3.zero, deathTime);
        yield return new WaitForSeconds(deathTime);
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + PointSystem.GetPoints();
    }
}
