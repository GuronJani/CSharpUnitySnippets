using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExperiencePickup : MonoBehaviour
{
    [SerializeField] int expValue = 1;
    GameObject target;
    Light lt;

    private void Start()
    {
        lt = GetComponent<Light>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Activate pull sequence
            target = other.gameObject;

            StartCoroutine(PickedUp());
        }
    }

    IEnumerator PickedUp()
    {
        Progression.SetExp(expValue);
        GameManager.Instance.UpdateExpSlider();
        Debug.Log("Exp: " + Progression.GetExp());
        transform.DOMove(target.gameObject.transform.position, 1f);
        transform.DOScale(Vector3.zero, 1f);
        lt.DOIntensity(0f,1f);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }
}
