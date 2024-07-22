using UnityEngine;

public class Egg : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isDropped = false;
    private SpriteRenderer spriteRenderer;
    public Vector2 offset; // Offset for positioning the egg when attached to the nest

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Ensure Rigidbody2D is Dynamic
            rb.gravityScale = 1; // Ensure gravity scale is positive
        }
    }

    public void DropEgg()
    {
        Debug.Log("DropEgg called");
        if (!isDropped)
        {
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // Enable gravity physics
                rb.gravityScale = 1; // Ensure gravity scale is positive
                isDropped = true;
                Debug.Log("Egg is now dropping");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name + " with tag " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Scoring"))
        {
            Debug.Log("Collision with Scoring object");
            Nests nest = collision.gameObject.GetComponent<Nests>();
            if (nest != null && !nest.HasEgg())
            {
                nest.AttachEgg(gameObject, offset); // Attach egg with offset
                nest.SetEgg(); // Mark the nest as having an egg
                GameManager.Instance.IncreaseScore(this, nest); // Increment score
                StopEgg();
            }
        }
    }

    public void StopEgg()
    {
        Debug.Log("Stopping egg");
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop the egg's movement
            rb.angularVelocity = 0f; // Stop any rotation
            rb.bodyType = RigidbodyType2D.Kinematic; // Disable physics simulation
        }
    }

    public void SetSortingLayerInFront()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Nests"; // Ensure the egg's sorting layer is "Nests"
            spriteRenderer.sortingOrder = 100; // Ensure the egg's sorting order is 100
        }
    }
}
