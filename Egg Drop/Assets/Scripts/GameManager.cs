using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text scoreText;
    public TMP_Text highScoreText; // Reference to the high score text
    public TMP_Text tapToStartText; // Reference to the "Tap to Start" text
    public GameObject restartButton;
    public GameObject eggPrefab;
    public Transform birdTransform; // Use bird's transform as drop position
    public Spawner spawner; // Reference to the spawner script

    private int score;
    private int highScore;
    private bool gameOver = false;
    private bool canDropEgg = true;
    private GameObject currentEgg;
    private bool gameStarted = false; // Track if the game has started
    private bool canDropEggs = false; // Track if eggs can be dropped

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
        score = 0; // Initialize score to 0
        highScore = PlayerPrefs.GetInt("HighScore", 0); // Load the high score
        UpdateScoreText(); // Set initial score text
        UpdateHighScoreText(); // Set initial high score text

        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }
        if (tapToStartText != null)
        {
            tapToStartText.gameObject.SetActive(true); // Show "Tap to Start" text
        }
        if (spawner != null)
        {
            spawner.StopSpawning(); // Ensure the spawner is not active at the start
        }
    }

    private void Update()
    {
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        canDropEggs = false; // Disable egg dropping initially
        Debug.Log("Game started");
        if (tapToStartText != null)
        {
            tapToStartText.gameObject.SetActive(false); // Hide "Tap to Start" text
        }
        if (spawner != null)
        {
            spawner.StartSpawning(); // Start spawning nests
        }
        StartCoroutine(EnableEggDroppingAfterDelay(1f)); // Enable egg dropping after a 1-second delay
    }

    private IEnumerator EnableEggDroppingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDropEggs = true; // Allow egg dropping after the delay
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public bool CanDropEggs()
    {
        return canDropEggs;
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
        if (gameOver) return;

        gameOver = true;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // Save the high score
        }
        if (scoreText != null)
        {
            scoreText.text = "Game Over";
        }
        UpdateHighScoreText();
        Debug.Log("Game Over");
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }
        if (spawner != null)
        {
            spawner.StopSpawning();
        }
    }

    public void IncreaseScore(Egg egg, Nests nest)
    {
        if (gameOver) return;
        score++;
        UpdateScoreText();
        if (currentEgg == egg.gameObject)
        {
            currentEgg = null;
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString(); // Update score text with just the number
        }
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "TOP " + highScore.ToString(); // Update high score text with "TOP" prefix
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
