using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime = 0.5f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence() {
        Vector3 punchVector = new Vector3(2,2,2);
        rb.velocity = Vector3.zero;
        rb.detectCollisions = false;
        transform.DOPunchScale(punchVector, lifeTime);
        yield return new WaitForSeconds(lifeTime);
        DOTween.Kill(this);
        Destroy(gameObject);
    }
}
