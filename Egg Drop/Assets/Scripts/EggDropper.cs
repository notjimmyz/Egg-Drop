using UnityEngine;

public class EggDropper : MonoBehaviour
{
    public GameObject eggPrefab; // Reference to the egg prefab

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for mouse button press
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