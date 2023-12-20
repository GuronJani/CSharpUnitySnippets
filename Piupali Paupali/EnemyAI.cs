using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] GameObject target;
    [SerializeField] float health;
    [SerializeField] float deathTime = 0.25f;
    //public GameObject damageText;
    public GameObject expDrop;
    Rigidbody rb;
    Light lt;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lt = GetComponent<Light>();
        if (target == null)
        {
            target = GameManager.Instance.playerObject;
        }
        moveSpeed += Random.Range(-0.09f, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        // Look at player
        Vector3 lookDir = target.transform.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        rb.MoveRotation(lookRot);
        // Approach player
        
        rb.AddForce(lookDir.normalized * moveSpeed, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Deadly"))
        {
            // Hit something, measure force:
            Debug.Log(collision.gameObject + " hit this " + gameObject + " for " + collision.impulse.magnitude);
            if (collision.impulse.magnitude > 0.1f)
            {
                /*
                Vector3 textSpawnPos = new Vector3(transform.position.x, 3, transform.position.z);
                var newDamText = Instantiate(damageText, textSpawnPos, Quaternion.identity);
                int damage = (int)collision.impulse.magnitude;
                TextMeshPro txt = newDamText.GetComponent<TextMeshPro>();
                txt.text = damage.ToString();
                txt.DOFade(0f,1f);
                */
                health -= collision.impulse.magnitude;

                if (health <= 0)
                {
                    StartCoroutine(Death());
                }
            }
        }
    }

    IEnumerator Death()
    {
        rb.detectCollisions = false;
        var newExpPickup = Instantiate(expDrop, transform.position, Quaternion.identity);
        Vector3 deathVector = new Vector3(1.5f, 1.5f, 1.5f);
        transform.DOPunchScale(deathVector, deathTime);
        lt.DOIntensity(0f, deathTime);
        yield return new WaitForSeconds(deathTime);
        transform.DOScale(Vector3.zero, deathTime);
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }
}
