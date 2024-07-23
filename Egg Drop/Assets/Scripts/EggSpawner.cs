using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    public GameObject eggPrefab;
    private Transform birdTransform;
    private List<GameObject> activeEggs = new List<GameObject>();

    void Start()
    {
        GameObject bird = GameObject.FindGameObjectWithTag("Bird");
        if (bird != null)
        {
            birdTransform = bird.transform;
        }
        else
        {
            Debug.LogError("Bird object not found. Please ensure the bird has the tag 'Bird'.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.CanDropEggs())
        {
            SpawnEgg();
        }

        activeEggs.RemoveAll(egg => egg == null);
        LogActiveEggs(); // Log the active eggs in the update method
    }

    void SpawnEgg()
    {
        if (birdTransform != null)
        {
            Vector3 spawnPosition = birdTransform.position;
            GameObject newEgg = Instantiate(eggPrefab, spawnPosition, Quaternion.identity);
            activeEggs.Add(newEgg);
            Debug.Log("Egg spawned: " + newEgg.name);
        }
    }

    public void DestroyAllEggs()
    {
        Debug.Log("Destroying all eggs...");
        foreach (var egg in activeEggs)
        {
            if (egg != null)
            {
                Debug.Log("Destroying egg: " + egg.name);
                Destroy(egg);
            }
        }
        activeEggs.Clear();
    }

    private void LogActiveEggs()
    {
        Debug.Log("Active eggs count: " + activeEggs.Count);
        foreach (var egg in activeEggs)
        {
            if (egg != null)
            {
                Debug.Log("Active egg: " + egg.name);
            }
        }
    }
}