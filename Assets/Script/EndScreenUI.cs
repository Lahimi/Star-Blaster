using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        // Unlock the cursor so the mouse works
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GameSession.Instance != null && scoreText != null)
        {
            scoreText.text = "FINAL SCORE: " + GameSession.Instance.GetScore().ToString("D5");
        }
    }

    // THIS IS WHAT THE RETRY BUTTON CALLS
    public void RetryGame()
    {
        Debug.Log("Retry Clicked!");
        if (GameSession.Instance != null) GameSession.Instance.ResetGame();
        
        // Level1 MUST be in your Build Settings (File > Build Settings)
        SceneManager.LoadScene("Level1");
    }

    // THIS IS WHAT THE MAIN MENU BUTTON CALLS
    public void LoadMainMenu()
    {
        Debug.Log("Menu Clicked!");
        if (GameSession.Instance != null) GameSession.Instance.ResetGame();
        
        // Scene 0 is usually the Main Menu
        SceneManager.LoadScene(0); 
    }
}