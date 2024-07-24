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

    // New speed-related variables
    public float initialNestSpeed = 5f; // initial speed of the nests
    public float speedIncreaseRate = 1f; // amount by which the speed increases every 10 seconds
    public float maxNestSpeed = 10f; // maximum speed limit

    private float currentMinSpawnRate;
    private float currentMaxSpawnRate;
    private int currentMaxNests;
    private float currentNestSpeed;
    private List<GameObject> activeNests = new List<GameObject>();

    private bool gameStarted = false;

    private void OnEnable()
    {
        currentMinSpawnRate = initialMinSpawnRate;
        currentMaxSpawnRate = initialMaxSpawnRate;
        currentMaxNests = initialMaxNests;
        currentNestSpeed = initialNestSpeed; // Initialize the nest speed
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
        StopCoroutine(IncreaseMaxNests());
        StopCoroutine(IncreaseNestSpeed());
    }

    private void Update()
    {
        if (!gameStarted) return;

        // Remove destroyed nests from the list
        activeNests.RemoveAll(nest => nest == null);

        // Destroy nests that move off-screen to the left
        for (int i = activeNests.Count - 1; i >= 0; i--)
        {
            var nest = activeNests[i];
            if (nest.transform.position.x < cameraTransform.position.x - spawnOffset)
            {
                Destroy(nest);
                activeNests.RemoveAt(i);
                Debug.Log($"Destroyed nest at position: {nest.transform.position.x}");
            }
        }

        // Ensure the spawner follows the camera
        Vector3 spawnerPosition = transform.position;
        spawnerPosition.y = cameraTransform.position.y;
        spawnerPosition.x = cameraTransform.position.x;
        transform.position = spawnerPosition;
    }

    public void StartSpawning()
    {
        gameStarted = true;
        StartCoroutine(IncreaseMaxNests());
        StartCoroutine(IncreaseNestSpeed()); // Start the coroutine to increase nest speed
        ScheduleNextSpawn();
        Debug.Log("Started spawning.");
    }

    public void StopSpawning()
    {
        gameStarted = false;
        CancelInvoke(nameof(Spawn));
        StopCoroutine(IncreaseMaxNests());
        StopCoroutine(IncreaseNestSpeed());
        Debug.Log("Stopped spawning.");
    }

    private void Spawn()
    {
        if (!gameStarted) return;

        if (activeNests.Count >= currentMaxNests)
        {
            ScheduleNextSpawn(); // Schedule the next spawn check
            Debug.Log("Reached max nests, scheduling next spawn.");
            return;
        }

        Vector3 spawnPosition = GetValidSpawnPosition();
        if (spawnPosition != Vector3.zero)
        {
            GameObject newNest = Instantiate(prefab, spawnPosition, Quaternion.identity);
            Nests nestScript = newNest.GetComponent<Nests>();
            if (nestScript != null)
            {
                nestScript.speed = currentNestSpeed; // Set the speed of the new nest
            }
            activeNests.Add(newNest);
            Debug.Log($"Spawned a new nest at position: {spawnPosition} with speed: {currentNestSpeed}. Total nests: {activeNests.Count}");
        }
        else
        {
            Debug.Log("No valid spawn position found.");
        }

        // Increase the spawn rates gradually, but cap the increase
        currentMinSpawnRate = Mathf.Min(initialMinSpawnRate + minGapIncreaseRate * activeNests.Count, initialMaxSpawnRate);
        currentMaxSpawnRate = Mathf.Min(initialMaxSpawnRate + maxGapIncreaseRate * activeNests.Count, initialMaxSpawnRate * 2);
        Debug.Log($"Updated spawn rates: Min: {currentMinSpawnRate}, Max: {currentMaxSpawnRate}");

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
                Debug.Log($"Spawn position too close to another nest: {nest.transform.position.x}");
                return Vector3.zero; // No valid position found, return invalid position
            }
        }

        return potentialPosition;
    }

    private void ScheduleNextSpawn()
    {
        if (!gameStarted) return;

        float spawnDelay = Random.Range(currentMinSpawnRate, currentMaxSpawnRate);
        Debug.Log($"Next spawn scheduled in {spawnDelay} seconds.");
        Invoke(nameof(Spawn), spawnDelay);
    }

    private IEnumerator IncreaseMaxNests()
    {
        while (gameStarted)
        {
            yield return new WaitForSeconds(increaseInterval);
            currentMaxNests++;
            Debug.Log($"Increased max nests to {currentMaxNests}");
        }
    }

    private IEnumerator IncreaseNestSpeed()
    {
        while (gameStarted)
        {
            yield return new WaitForSeconds(10f); // Increase speed every 10 seconds
            currentNestSpeed = Mathf.Min(currentNestSpeed + speedIncreaseRate, maxNestSpeed);
            Debug.Log($"Increased nest speed to {currentNestSpeed}");
        }
    }

    public void ResetSpawner()
    {
        StopSpawning(); // Stop any ongoing spawns
        currentMinSpawnRate = initialMinSpawnRate;
        currentMaxSpawnRate = initialMaxSpawnRate;
        currentMaxNests = initialMaxNests;
        currentNestSpeed = initialNestSpeed; // Reset the nest speed

        // Destroy all active nests
        foreach (var nest in activeNests)
        {
            if (nest != null)
            {
                Destroy(nest);
                Debug.Log($"Destroyed nest during reset.");
            }
        }

        activeNests.Clear(); // Clear the list of active nests
        Debug.Log("Spawner reset.");
    }
}
