using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager instance;

    public PlayerMotor playerMotor;

    [Header("UI Elements")]
    public Canvas deadCanvas;
    public TextMeshProUGUI lifeText, asteroidText, finalScoreText, highScoreText;
    public Button retryButton;

    [Header("Game Variables")]
    public int startingLives = 3;
    public int highScore;
    private int livesLeft = 3, asteroidCount = 0;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        retryButton.onClick.AddListener(Retry);
        SetLives(startingLives);
        SetScore(0);

        highScoreText.text = "Highscore " + highScore.ToString();
    }

    /// <summary>
    /// Resets the curent score, lives and scene
    /// </summary>
    public void Retry()
    {
        SetLives(startingLives);
        SetScore(0);

        deadCanvas.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Triggers a hit on the player
    /// </summary>
    public void PlayerHit()
    {
        BumpLife(false);

        //Check if player is dead
        if (livesLeft == 0)
        {
            //Store highscore if higher than previous
            if (highScore < asteroidCount)
            {
                highScore = asteroidCount;
                highScoreText.text = "Highscore " + highScore.ToString();
            }

            //Enable player dead UI
            deadCanvas.enabled = true;
            finalScoreText.text = "Final Score \n" + asteroidCount.ToString();
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Adjust life value by 1
    /// </summary>
    /// <param name="up"></param>
    private void BumpLife(bool up)
    {
        if (up)
            SetLives(livesLeft + 1);
        else
            SetLives(livesLeft - 1);
    }
    /// <summary>
    /// Adjust score value by 1
    /// </summary>
    /// <param name="up"></param>
    public void BumpScore(bool up)
    {
        if (up)
            SetScore(asteroidCount + 1);
        else
            SetScore(asteroidCount - 1);
    }
    /// <summary>
    /// Set the number of lives left
    /// </summary>
    /// <param name="amount"></param>
    private void SetLives(int amount)
    {
        livesLeft = amount;
        lifeText.text = livesLeft.ToString();
    }
    /// <summary>
    /// Set the players score
    /// </summary>
    /// <param name="amount"></param>
    private void SetScore(int amount)
    {
        asteroidCount = amount;
        asteroidText.text = asteroidCount.ToString();
    }
}
