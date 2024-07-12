using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    public GameObject eggPrefab; // The egg prefab to instantiate
    public float startX = 0f; // Starting X position for the egg drop
    public float startY = 0f; // Starting Y position for the egg drop
    public float startZ = 0f; // Starting Z position for the egg drop

    private List<GameObject> activeEggs = new List<GameObject>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button or screen tap
        {
            SpawnAndDropEgg();
        }

        // Remove destroyed eggs from the list
        activeEggs.RemoveAll(egg => egg == null);
    }

    void SpawnAndDropEgg()
    {
        // Instantiate a new egg at the specified starting position
        Vector3 spawnPosition = new Vector3(startX, startY, startZ);
        GameObject newEgg = Instantiate(eggPrefab, spawnPosition, Quaternion.identity);
        activeEggs.Add(newEgg);

        // Set the Rigidbody2D body type to Dynamic to enable gravity
        Rigidbody2D rb = newEgg.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}