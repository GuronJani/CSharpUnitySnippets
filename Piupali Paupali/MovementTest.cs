using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float y = Input.GetAxis("Vertical"); // this is up/down player input, both WS and up/down arrow work
        Vector3 movement = new Vector3(1, y, 0);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
