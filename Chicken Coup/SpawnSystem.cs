using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnSystem : MonoBehaviour
{
    public static SpawnSystem instance;
    [SerializeField] GameObject enemyPrefab;
    bool isRunning = true;

    private void Awake()
    {
        if (instance == null)
        {
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
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0);
            //UnitManager.instance.UpdateUnitList();
        }
    }

    public void SpawnEnemy()
    {
        if (!isRunning) { return; }

        Vector3 randPos = new Vector3(Random.Range(50, 100), 5f, Random.Range(50, 100));
        var newEnemy = Instantiate(enemyPrefab, randPos, Quaternion.identity);
    }
}
