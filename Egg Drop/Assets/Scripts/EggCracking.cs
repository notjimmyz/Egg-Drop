using UnityEngine;

public class EggCracking : MonoBehaviour
{
    public Sprite[] crackingSprites; // Array to hold the egg cracking sprites
    public float animationDuration = 1f; // Total duration of the cracking animation
    private SpriteRenderer spriteRenderer;
    private bool isCracking = false;
    private float animationTimer = 0f;
    private int currentSpriteIndex = 0;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    public void StartCrackingAnimation()
    {
        isCracking = true;
        animationTimer = 0f;
        currentSpriteIndex = 0;

        // Start the animation immediately by setting the first sprite
        if (crackingSprites.Length > 0)
        {
            spriteRenderer.sprite = crackingSprites[0];
        }
    }

    void Update()
    {
        if (isCracking)
        {
            AnimateCracking();
        }
    }

    void AnimateCracking()
    {
        animationTimer += Time.unscaledDeltaTime; // Use unscaled time for animation
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
            else
            {
                // Animation is done
                isCracking = false;

                // Optionally, destroy the egg object after animation
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Stop the egg's movement
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // Stop all movement
                rb.bodyType = RigidbodyType2D.Kinematic; // Disable physics simulation
            }

            // Update high score immediately
            GameManager.Instance.UpdateHighScore();

            // Show the restart button
            GameManager.Instance.ShowRestartButton();

            // Start the cracking animation
            StartCrackingAnimation();

            // Immediately set the game over state and freeze the game
            GameManager.Instance.GameOver();

            // Freeze the game immediately
            Time.timeScale = 0f;
            Debug.Log("Screen frozen");
        }
    }
}
