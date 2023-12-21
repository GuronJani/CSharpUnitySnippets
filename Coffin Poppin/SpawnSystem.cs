using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject[] enemies;
    [SerializeField] float spawnTime;
    [SerializeField] float minSpawnTime;
    public bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnProcess());
    }

    private void FixedUpdate()
    {
        if (spawnTime > minSpawnTime)
        {
            spawnTime -= 0.0001f;
        }
    }
    IEnumerator SpawnProcess()
    {
        while (isRunning)
        {
            // Select random spawn point
            Transform selectedPoint = spawnPositions[Random.Range(0, spawnPositions.Length)];
            // Select random enemy
            GameObject selectedEnemy = enemies[Random.Range(0, enemies.Length)];
            // Spawn selected enemy to selected point
            var newEnemy = Instantiate(selectedEnemy, selectedPoint.transform.position, Quaternion.identity);
            selectedPoint.gameObject.transform.DOPunchScale(Vector3.one * 3f, 0.25f);
            // Wait and repeat
            
            yield return new WaitForSeconds(spawnTime);

        }
    }
}
