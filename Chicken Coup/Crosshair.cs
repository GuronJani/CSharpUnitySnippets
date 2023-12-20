using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public static Crosshair instance;
    Plane plane = new Plane(Vector3.up, 0);

    private void Awake()
    {
        // Singleton pattern
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCrosshairPosition();
    }

    public void UpdateCrosshairPosition()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance) + Vector3.up * 0.25f;
        }
    }

}
