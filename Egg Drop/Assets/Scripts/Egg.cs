using UnityEngine;

public class Egg : MonoBehaviour
{
    public float startX = 0f; // Initial X position
    public float startY = 0f; // Initial Y position
    public float startZ = 0f; // Initial Z position
    public float offsetX = 0f; // Offset for X position
    public float offsetY = 0f; // Offset for Y position
    public float offsetZ = 0f; // Offset for Z position

    private bool isDropped = false;
    private bool hasScored = false;
    
    public Sprite[] crackingSprites; // Array to hold the egg cracking sprites
    public float animationDuration = 1f; // Total duration of the cracking animation
    private SpriteRenderer spriteRenderer;
    private bool isCracking = false;
    private float animationTimer = 0f;
    private int currentSpriteIndex = 0;

    void Start()
    {
        // Set the initial position of the egg
        transform.position = new Vector3(startX, startY, startZ);

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the crackingSprites array is not empty
        if (crackingSprites.Length > 0)
        {
            // Set the initial sprite
            spriteRenderer.sprite = crackingSprites[0];
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDropped) // Left mouse button or screen tap
        {
            DropEgg();
        }

        if (isCracking)
        {
            AnimateCracking();
        }
    }

    void DropEgg()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false; // Enable gravity physics
        rb.gravityScale = 1; // Ensure gravity is enabled
        isDropped = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            FindObjectOfType<GameManager>().GameOver();
        }
        else if (other.gameObject.tag == "Scoring" && !hasScored)
        {
            Nests nest = other.gameObject.GetComponent<Nests>();
            if (nest != null && !nest.HasEgg())
            {
                hasScored = true;
                FindObjectOfType<GameManager>().IncreaseScore(this, nest);
                AttachToNest(nest.transform);
            }
        }
    }

    private void AttachToNest(Transform nestTransform)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true; // Disable further physics interactions
        rb.gravityScale = 0; // Disable gravity

        transform.SetParent(nestTransform); // Set nest as parent
        transform.localPosition = new Vector3(offsetX, offsetY, offsetZ); // Set position with offset
    }

    public void StopCompletely()
    {
        isCracking = true; // Start the cracking animation
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true; // Disable further physics interactions
        rb.gravityScale = 0; // Disable gravity
    }
    
    void AnimateCracking()
    {
        animationTimer += Time.deltaTime;
        float timePerSprite = animationDuration / crackingSprites.Length;

        if (animationTimer >= timePerSprite)
        {
            // Move to the next sprite
            animationTimer -= timePerSprite;
            currentSpriteIndex++;

            if (currentSpriteIndex < crackingSprites.Length)
            {
                spriteRenderer.sprite = crackingSprites[currentSpriteIndex];
            }

            // If the current sprite index reaches 5, stop the animation
            if (currentSpriteIndex >= 5)
            {
                isCracking = false;

                // Trigger Game Over after the animation if necessary
                FindObjectOfType<GameManager>().GameOver();

                // Optionally, destroy the egg object after animation
                // Destroy(gameObject);
            }
        }
    }
}
