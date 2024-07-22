using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float initialMinSpawnRate = 0.01f; // initial minimum time between spawns
    public float initialMaxSpawnRate = 0.2f; // initial maximum time between spawns
    public float minGapIncreaseRate = 0.005f; // rate at which the minimum spawn rate increases
    public float maxGapIncreaseRate = 0.005f; // rate at which the maximum spawn rate increases
    public float minDistanceBetweenNests = 0.5f; // minimum distance between nests
    public int initialMaxNests = 2; // initial maximum number of nests
    public float increaseInterval = 20f; // time interval to increase max nests
    public Transform cameraTransform; // reference to the camera transform
    public float spawnOffset = 5f; // distance from the camera to spawn

    public float fixedY = 0f; // fixed Y position for spawning

    private float currentMinSpawnRate;
    private float currentMaxSpawnRate;
    private int currentMaxNests;
    private List<GameObject> activeNests = new List<GameObject>();

    private void OnEnable()
    {
        currentMinSpawnRate = initialMinSpawnRate;
        currentMaxSpawnRate = initialMaxSpawnRate;
        currentMaxNests = initialMaxNests;
        StartCoroutine(IncreaseMaxNests());
        ScheduleNextSpawn();
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
        StopCoroutine(IncreaseMaxNests());
    }

    private void Update()
    {
        // Remove destroyed nests from the list
        activeNests.RemoveAll(nest => nest == null);

        // Destroy nests that move off-screen to the left
        foreach (var nest in activeNests)
        {
            if (nest.transform.position.x < cameraTransform.position.x - spawnOffset)
            {
                Destroy(nest);
            }
        }

        // Ensure the spawner follows the camera
        Vector3 spawnerPosition = transform.position;
        spawnerPosition.y = cameraTransform.position.y;
        spawnerPosition.x = cameraTransform.position.x;
        transform.position = spawnerPosition;
    }

    private void Spawn()
    {
        if (activeNests.Count >= currentMaxNests)
        {
            ScheduleNextSpawn(); // Schedule the next spawn check
            return;
        }

        Vector3 spawnPosition = GetValidSpawnPosition();
        if (spawnPosition != Vector3.zero)
        {
            GameObject newNest = Instantiate(prefab, spawnPosition, Quaternion.identity);
            activeNests.Add(newNest);
            Debug.Log($"Spawned a new nest at position: {spawnPosition}");
        }
        else
        {
            Debug.Log("No valid spawn position found.");
        }

        // Increase the spawn rates gradually
        currentMinSpawnRate += minGapIncreaseRate;
        currentMaxSpawnRate += maxGapIncreaseRate;

        // Schedule the next spawn
        ScheduleNextSpawn();
    }

    private Vector3 GetValidSpawnPosition()
    {
        // Spawn off-screen to the right of the camera
        float spawnX = cameraTransform.position.x + spawnOffset;
        Vector3 potentialPosition = new Vector3(spawnX, fixedY, transform.position.z);

        // Ensure minimum distance between nests
        foreach (GameObject nest in activeNests)
        {
            if (Mathf.Abs(spawnX - nest.transform.position.x) < minDistanceBetweenNests)
            {
                return Vector3.zero; // No valid position found, return invalid position
            }
        }

        return potentialPosition;
    }

    private void ScheduleNextSpawn()
    {
        float spawnDelay = Random.Range(currentMinSpawnRate, currentMaxSpawnRate);
        Debug.Log($"Next spawn scheduled in {spawnDelay} seconds.");
        Invoke(nameof(Spawn), spawnDelay);
    }

    private IEnumerator IncreaseMaxNests()
    {
        while (currentMaxNests < 7)
        {
            yield return new WaitForSeconds(increaseInterval);
            currentMaxNests++;
            Debug.Log($"Increased max nests to {currentMaxNests}");
        }
    }
}
