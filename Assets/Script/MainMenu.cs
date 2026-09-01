using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] string firstLevelName = "Level1";

    [Header("UI Panels")]
    [SerializeField] GameObject howToPlayPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject scorePanel;

    [Header("High Score Display")]
    [SerializeField] TextMeshProUGUI highScoreText;

    void Start()
    {
        // Ensure all panels are hidden when the menu first loads
        ToggleHowToPlay(false);
        ToggleCredits(false);
        ToggleScorePanel(false);

        // Pre-load the high score so it's ready
        UpdateHighScoreUI();
    }

    // --- Button Functions ---

    public void StartGame()
    {
        if (GameSession.Instance != null) 
        {
            GameSession.Instance.ResetGame();
        }
        
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadSceneWithFade(firstLevelName);
        }
    }

    // This is the function your "Score Button" needs to trigger
    public void ToggleScorePanel(bool isOpen)
    {
        if(scorePanel != null) 
        {
            scorePanel.SetActive(isOpen);
            
            if(isOpen)
            {
                UpdateHighScoreUI();
            }
        }
    }

    void UpdateHighScoreUI()
    {
        if (highScoreText != null && GameSession.Instance != null)
        {
            // Pulls the high score we stored in GameSession
            highScoreText.text = "PERSONAL BEST: " + GameSession.Instance.GetHighScore().ToString("D5");
        }
        else if (highScoreText != null)
        {
            highScoreText.text = "BEST: 00000";
        }
    }

    public void ToggleHowToPlay(bool isOpen) => howToPlayPanel?.SetActive(isOpen);
    public void ToggleCredits(bool isOpen) => creditsPanel?.SetActive(isOpen);

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

        // This tells the Unity Editor to stop playing
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // This closes the actual game once it is exported
            Application.Quit();
        #endif
    }
}