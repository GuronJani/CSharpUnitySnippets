using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField] Vector3 rotVector;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotVector * Time.fixedDeltaTime);
    }
}
