using UnityEngine;

public class Egg : MonoBehaviour
{
    public Sprite[] crackingSprites;
    public float animationDuration = 1f;
    public float offsetX = 0f;
    public float offsetY = 0f;
    public float offsetZ = 0f;

    private SpriteRenderer spriteRenderer;
    private bool isCracking = false;
    private float animationTimer = 0f;
    private int currentSpriteIndex = 0;
    private bool isDropped = false;
    private bool hasScored = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void DropEgg()
    {
        if (!isDropped)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isDropped = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nests") && !hasScored)
        {
            Nests nest = collision.gameObject.GetComponent<Nests>();
            if (nest != null && !nest.HasEgg())
            {
                hasScored = true;
                GameManager.Instance.IncreaseScore(this, nest);
                AttachToNest(nest.transform);
            }
        }
        else if (collision.gameObject.CompareTag("Ground") && !hasScored)
        {
            StartCrackingAnimation();
        }
    }

    private void AttachToNest(Transform nestTransform)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        transform.SetParent(nestTransform);
        transform.localPosition = new Vector3(offsetX, offsetY, offsetZ);
    }

    public void StopCompletely()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }

    public void StartCrackingAnimation()
    {
        if (!isCracking)
        {
            isCracking = true;
            animationTimer = 0f;
            currentSpriteIndex = 0;
        }
    }

    void AnimateCracking()
    {
        animationTimer += Time.deltaTime;
        float timePerSprite = animationDuration / crackingSprites.Length;

        if (animationTimer >= timePerSprite)
        {
            animationTimer -= timePerSprite;
            currentSpriteIndex++;

            if (currentSpriteIndex < crackingSprites.Length)
            {
                spriteRenderer.sprite = crackingSprites[currentSpriteIndex];
            }
            else
            {
                isCracking = false;
                GameManager.Instance.GameOver();
                DestroyEgg();
            }
        }
    }

    void OnBecameInvisible()
    {
        DestroyEgg();
    }

    void DestroyEgg()
    {
        GameManager.Instance.EggDestroyed(gameObject);
        Destroy(gameObject);
    }
}