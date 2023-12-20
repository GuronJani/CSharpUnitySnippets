using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody rb;
    TrailRenderer tr;

    bool hasHit = false;
    float fadeTime = 1f;
    int damage = 1;
    float selfDestructTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();

        StartCoroutine(SelfDestruction());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) { return; }

        hasHit = true;

        if (TryGetComponent<Enemy>(out Enemy enemy))
        {
            // enemy.TakeDamage(damage);
        }

        StartCoroutine(ImpactSequence());
    }

    IEnumerator SelfDestruction() {

        yield return new WaitForSeconds(selfDestructTime);
        StartCoroutine(ImpactSequence());
    }

    IEnumerator ImpactSequence()
    {
        rb.detectCollisions = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        tr.DOTime(0f, fadeTime).SetEase(Ease.Linear);
        transform.DOPunchScale(transform.localScale * 1.2f, fadeTime).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(fadeTime);
        Destroy(gameObject);
    }

}
