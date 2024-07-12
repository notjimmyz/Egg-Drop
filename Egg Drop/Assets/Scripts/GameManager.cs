using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText; // Drag your TMP object here in the inspector
    public GameObject restartButton; // Drag your Restart Button here in the inspector
    private int score;
    private bool gameOver = false;

    private void Start()
    {
        UpdateScoreText();
        if (restartButton != null)
        {
            restartButton.SetActive(false); // Hide the restart button at the start
        }
    }

    public void GameOver()
    {
        gameOver = true;
        if (scoreText != null)
        {
            scoreText.text = "Game Over";
        }
        Debug.Log("Game Over");
        StopAll();
        if (restartButton != null)
        {
            restartButton.SetActive(true); // Show the restart button when the game is over
        }
    }

    public void IncreaseScore(Egg egg, Nests nest)
    {
        if (gameOver) return;

        score++;
        nest.SetEgg(); // Mark the nest as having an egg
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    private void StopAll()
    {
        // Stop all nests
        foreach (var nest in FindObjectsOfType<Nests>())
        {
            nest.StopMoving();
        }

        // Stop all eggs
        foreach (var egg in FindObjectsOfType<Egg>())
        {
            egg.StopCompletely();
        }
    }

    public void RestartGame()
    {
        // Reset the score
        score = 0;
        UpdateScoreText();

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}