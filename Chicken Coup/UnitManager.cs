using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //public static UnitManager instance;
    public List<Unit> units = new List<Unit>();
    public bool isOperational = true;
    [SerializeField] Transform camPivot;
    bool gameOver = false, searching;

    private void Awake()
    {/*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        } */
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateUnitList();
        
    }

    

    // Update is called once per frame
    void Update()
    {
        /*
        if (units.Count <= 0 && !searching)
        {
            // Game over?
            gameOver = true;
            return;
        } */

        if (units.Count <= 0) return;

        foreach (var unit in units)
        {
            unit.GetInputs();
            unit.Aim();

            if (Input.GetButton("Fire1"))
            {
                unit.Fire();
            }
        }

        
    }

    private void FixedUpdate()
    {
        if (units.Count <= 0) return;

        foreach (var unit in units)
        {
            unit.Movement();
        }
    }

    private void LateUpdate()
    {
        CameraFollow();
    }

    // CLASS METHODS

    public void UpdateUnitList()
    {
        searching = true;
        units.Clear();
        units.Capacity = FindObjectsOfType<Unit>().Length;
        units.AddRange(FindObjectsOfType<Unit>());
        searching = false;
    }

    void CameraFollow()
    {
        // FIX: make interpolation target between crosshair and first unit element
        Vector3 middlePosition = Vector3.Lerp(units[0].transform.position, Crosshair.instance.transform.position, 0.5f);
        camPivot.transform.position = Vector3.Lerp(camPivot.transform.position, middlePosition, Time.deltaTime * 1f);
    }

}
