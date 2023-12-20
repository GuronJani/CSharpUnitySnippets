using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public bool isRunning;
    public float spawnTime;
    public GameObject[] enemies;
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (isRunning)
        {
            // Select random enemy
            GameObject selectedEnemy = enemies[Random.Range(0, enemies.Length)];
            // Select random spawn spot
            Transform selectedSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
            // spawn selected
            var newEnemy = Instantiate(selectedEnemy, selectedSpot.transform.position, Quaternion.identity);
            // Wait and reloop
            yield return new WaitForSeconds(spawnTime);

        }
    }

}
