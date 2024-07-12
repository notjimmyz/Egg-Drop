using UnityEngine;

public class EggDropper : MonoBehaviour
{
    private bool isDropped = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDropped) // Left mouse button or screen tap
        {
            DropEgg();
        }
    }

    void DropEgg()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Enable gravity physics
            isDropped = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nest"))
        {
            // Disable the Rigidbody2D to stop the egg from moving
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // Stop the egg's movement
                rb.angularVelocity = 0f; // Stop any rotation
                rb.isKinematic = true; // Disable physics simulation
            }

            // Add points to the score
            ScoreManager.instance.AddScore(1);

            // Destroy the egg after a short delay (if desired)
            // Destroy(gameObject, 0.5f); // Delay in seconds
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Destroy the egg when it goes off the screen
    }
}