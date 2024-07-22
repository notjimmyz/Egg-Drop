using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text scoreText;
    public GameObject restartButton;
    public GameObject eggPrefab;
    public Transform birdTransform; // Use bird's transform as drop position

    private int score;
    private bool gameOver = false;
    private bool canDropEgg = true;
    private GameObject currentEgg;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateScoreText();
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameOver && canDropEgg && currentEgg == null)
        {
            SpawnAndDropEgg();
        }
    }

    private void SpawnAndDropEgg()
    {
        if (birdTransform != null && eggPrefab != null && currentEgg == null)
        {
            currentEgg = Instantiate(eggPrefab, birdTransform.position, Quaternion.identity);
            Egg eggScript = currentEgg.GetComponent<Egg>();
            if (eggScript != null)
            {
                eggScript.DropEgg();
            }
            canDropEgg = false;
            Invoke("ResetDropCooldown", 0.5f);
        }
    }

    private void ResetDropCooldown()
    {
        canDropEgg = true;
    }

    public void GameOver()
    {
        if (gameOver) return; // Prevent multiple GameOver calls

        gameOver = true;
        if (scoreText != null)
        {
            scoreText.text = "Game Over";
        }
        Debug.Log("Game Over");
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }
    }

    public void IncreaseScore(Egg egg, Nests nest)
    {
        if (gameOver) return;
        score++;
        UpdateScoreText();
        if (currentEgg == egg.gameObject)
        {
            currentEgg = null; // Reset the current egg reference only if it matches
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EggDestroyed(GameObject egg)
    {
        if (currentEgg == egg)
        {
            currentEgg = null;
        }
    }
}
