using UnityEngine;

public class EggCracking : MonoBehaviour
{
    public Sprite[] crackingSprites; // Array to hold the egg cracking sprites
    public float animationDuration = 1f; // Total duration of the cracking animation
    private SpriteRenderer spriteRenderer;
    private bool isCracking = false;
    private float animationTimer = 0f;
    private int currentSpriteIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartCrackingAnimation()
    {
        isCracking = true;
        animationTimer = 0f;
        currentSpriteIndex = 0;
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
            else
            {
                // Animation is done
                isCracking = false;
                
                // Trigger Game Over after the animation
                FindObjectOfType<GameManager>().GameOver();

                // Optionally, destroy the egg object after animation
                Destroy(gameObject);
            }
        }
    }
}