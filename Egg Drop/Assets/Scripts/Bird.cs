using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameObject eggPrefab; // Reference to the egg prefab

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.IsGameStarted()) // Check for mouse button press and if the game has started
        {
            DropEgg();
        }
    }

    void DropEgg()
    {
        if (eggPrefab != null)
        {
            Vector3 spawnPosition = transform.position; // Get the bird's position
            GameObject newEgg = Instantiate(eggPrefab, spawnPosition, Quaternion.identity); // Instantiate a new egg
            Egg eggScript = newEgg.GetComponent<Egg>();
            if (eggScript != null)
            {
                eggScript.DropEgg(); // Call DropEgg to enable gravity
            }
        }
    }
}