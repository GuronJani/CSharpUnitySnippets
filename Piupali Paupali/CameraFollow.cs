using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform camTarget;
    [SerializeField] float camSpeed;
    [SerializeField] float camHeight;
    public bool cameraFollowing;

    // Start is called before the first frame update
    void Start()
    {
        if (camTarget == null)
        {
            camTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (cameraFollowing)
        {
            SmoothFollowTarget();
        }
    }

    void SmoothFollowTarget()
    {
        // Find target position
        Vector3 followPos = new Vector3(camTarget.position.x, camHeight, camTarget.position.z);
        // Slerp to position over time
        transform.position = Vector3.Slerp(transform.position, followPos, Time.deltaTime * camSpeed);
    }
}
