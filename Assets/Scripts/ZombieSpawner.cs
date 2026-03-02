using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    
    [Header("Spawn Settings")]
    public float initialSpawnDelay = 3f;
    public float minimumSpawnDelay = 0.5f;
    public float spawnDelayDecreaseRate = 0.1f; // How much faster it spawns each time
    
    [Header("Area Settings")]
    public float spawnRadius = 15f; // Minimum distance from center (or player) to spawn
    
    private float currentSpawnDelay;

    void Start()
    {
        currentSpawnDelay = initialSpawnDelay;
        if (zombiePrefab != null)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnDelay);
            
            SpawnZombie();
            
            // Increase spawn rate over time
            currentSpawnDelay -= spawnDelayDecreaseRate;
            if (currentSpawnDelay < minimumSpawnDelay)
            {
                currentSpawnDelay = minimumSpawnDelay;
            }
        }
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    private void SpawnZombie()
    {
        // Get a random point on a circle
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        // Optionally add extra random distance
        randomCircle += randomCircle.normalized * Random.Range(0f, 5f);
        
        Vector3 spawnPosition = new Vector3(randomCircle.x, 1f, randomCircle.y);
        
        // Let's project it onto the NavMesh to make sure it's valid
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
        }

        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
    }
}
