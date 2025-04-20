using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class GameUIManager : MonoBehaviour
{
    [Header("High Score")]
    public int highScore = 0;
    private string highScoreFilePath;

    [Header("Pause Settings")]
    public GameObject settingsPanel;
    public GameObject dimBackground; // <- assign a semi-transparent black image in the Canvas
    public GameObject menutext;

    private bool isPaused = false;

    [Header("Score Settings")]
    [Tooltip("Seconds of survival per point")]

    [Header("Score Multiplier Settings")]
    public int scoreMilestone = 10;        // Every X points
    public int currentMultiplier = 1;      // Current multiplier
    public int multiplierIncrement = 2;    // How much to multiply by

    public float secondsPerPoint = 2f;       // award 1 point every 2 seconds
    public TMP_Text scoreText;               // drag your TextMeshPro ScoreText here
    private bool isGameOver = false; // Track if the game is over
    void Start()
    {
        highScoreFilePath = Path.Combine(Application.persistentDataPath, "highscore.txt");
        LoadHighScore();
        UpdateScoreDisplay();
    }
    private float timeAccumulator = 0f;

    public GameObject gameOverScreen;
    private int score = 0;
    public int score_duplicate = 0; 

    private void CheckForMultiplierIncrease()
    {
        if (score >= scoreMilestone)
        {
            currentMultiplier += multiplierIncrement;   // e.g. 1 → 2 → 3
            scoreMilestone *= 2;                        // double the milestone
        }
    }


    void Update()
    {
        // 1) Pause / Resume logic
        if (Input.GetKeyDown(KeyCode.Escape)&& !isGameOver) // Check if the game is not over
        {
            // Toggle pause state
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;

            if (isPaused) OpenSettings();
            else            ResumeGame();
        }
        score_duplicate = score; // duplicate score for other scripts
        // 2) Score accumulation ONLY when not paused
        if (!isPaused)
        {
            timeAccumulator += Time.deltaTime;
            if (timeAccumulator >= secondsPerPoint)
            {
                int pointsToAdd = Mathf.FloorToInt(timeAccumulator / secondsPerPoint);
                score += pointsToAdd * currentMultiplier;
                timeAccumulator -= pointsToAdd * secondsPerPoint;
                CheckForMultiplierIncrease();
                UpdateScoreDisplay();

            }
        }

        // 3) High score logic
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
        }
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {score}\nHigh Score: {highScore}";
    }

    public void OpenSettings()
    {   
        Time.timeScale = 0f;
        settingsPanel.SetActive(true);
        if (dimBackground != null)
            dimBackground.SetActive(true);
        menutext.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        settingsPanel.SetActive(false);
        if (dimBackground != null)
            dimBackground.SetActive(false);
        menutext.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false; // Reset game over state
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    private void LoadHighScore()
    {
        if (File.Exists(highScoreFilePath))
        {
            string saved = File.ReadAllText(highScoreFilePath);
            int.TryParse(saved, out highScore);
        }
        else
        {
            highScore = 0;
            File.WriteAllText(highScoreFilePath, "0"); // create file on first launch
        }
    }

    private void SaveHighScore()
    {
        File.WriteAllText(highScoreFilePath, highScore.ToString());
    }

    public void gameOver(){
        isGameOver = true;
        gameOverScreen.SetActive(true);
    }
}
