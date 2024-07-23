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
    }

    void SpawnEgg()
    {
        if (birdTransform != null)
        {
            Vector3 spawnPosition = birdTransform.position;
            GameObject newEgg = Instantiate(eggPrefab, spawnPosition, Quaternion.identity);
            activeEggs.Add(newEgg);
        }
    }
}