using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    
    float xInput;
    float yInput;
    public bool movementAllowed;
    Rigidbody rb;
    [SerializeField] float health = 1f;
    
    int currentHealth;
    [SerializeField] float testMoveSpeed; // set this as the default when happy
    [SerializeField] float currentMoveSpeed;
    [SerializeField] float currentDamageMulti;
    [SerializeField] float currentCooldownMulti;
    [SerializeField] float currentProjectileSpeed;
    [SerializeField] float dashCooldown = 0.2f;
    [SerializeField] GameObject lvlUpWeapon;
    bool dashing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerStats.SetMoveSpeed(testMoveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        if (Input.GetKeyDown(KeyCode.T))
        {
            //LevelUp();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        if (movementAllowed)
        {
            PhysicsMovement();
        }
        
    }

    // Class methods

    void GetInputs()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Dash());
        }

    }

    void PhysicsMovement()
    {
        Vector3 movementVector = new Vector3(xInput, 0, yInput);
        currentMoveSpeed = PlayerStats.GetMoveSpeed();

        if (dashing)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, movementVector.normalized * (currentMoveSpeed * 4), Time.deltaTime * 5f);
        } else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, movementVector.normalized * currentMoveSpeed, Time.deltaTime * 5f);
        }
        
    }

    IEnumerator Dash()
    {
        if (!dashing)
        {
            dashing = true;
            yield return new WaitForSeconds(dashCooldown);
            dashing = false;
        }
    }

    public void LevelUp()
    {
        var newWeapon = Instantiate(lvlUpWeapon);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            health -= (collision.impulse.magnitude * 0.033f);
            GameManager.Instance.UpdateHealthBar(health);
            if (health <= 0)
            {
                StartCoroutine(Death());
            }
        }
    }

    IEnumerator Death()
    {
        movementAllowed = false;
        rb.detectCollisions = false;
        transform.DOScale(Vector3.zero, 1f);
        GameManager.Instance.UpdateGameState(GameState.GameOver);
        yield return new WaitForSeconds(2f);
        
    }

}
