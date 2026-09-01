using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [Header("UI Object Names")]
    [SerializeField] string scoreObjName = "Score";
    [SerializeField] string healthObjName = "Health";
    [SerializeField] string waveObjName = "Wave";
    [SerializeField] string iconObjName = "Character_Icon";

    [Header("Evolution Sprites")]
    [SerializeField] Sprite wooperSprite;
    [SerializeField] Sprite quagsireSprite;

    [Header("Persistent Stats")]
    [SerializeField] int playerMaxHealth = 100;
    int playerCurrentHealth;
    int score = 0;
    int highScore = 0;

    TextMeshProUGUI scoreText;
    TextMeshProUGUI healthText;
    TextMeshProUGUI waveText;
    Image playerIcon;
    EnemySpawner spawner;

    public static GameSession Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerCurrentHealth = playerMaxHealth;
            // Load high score from computer memory
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => RefreshUIReferences();

    public void RefreshUIReferences()
    {
        GameObject scoreObj = GameObject.Find(scoreObjName);
        if (scoreObj != null) scoreText = scoreObj.GetComponent<TextMeshProUGUI>();

        GameObject healthObj = GameObject.Find(healthObjName);
        if (healthObj != null) healthText = healthObj.GetComponent<TextMeshProUGUI>();

        GameObject waveObj = GameObject.Find(waveObjName);
        if (waveObj != null) waveText = waveObj.GetComponent<TextMeshProUGUI>();

        GameObject iconObj = GameObject.Find(iconObjName);
        if (iconObj != null) playerIcon = iconObj.GetComponent<Image>();

        spawner = FindFirstObjectByType<EnemySpawner>();
        UpdateIcon();
    }

    void Update()
    {
        if (scoreText == null && SceneManager.GetActiveScene().buildIndex != 0)
        {
            RefreshUIReferences();
        }

        if (scoreText != null) scoreText.text = "SCORE: " + score.ToString("D5");
        if (healthText != null) healthText.text = "HP: " + playerCurrentHealth.ToString();
        
        if (spawner != null && waveText != null)
            waveText.text = "WAVE: " + (spawner.GetCurrentWaveIndex() + 1);
    }

    // --- Stats Management ---

    public int GetPersistentHealth() => playerCurrentHealth;

    public void UpdatePersistentHealth(int newHealthValue)
    {
        playerCurrentHealth = Mathf.Clamp(newHealthValue, 0, playerMaxHealth);
    }

    public void AddToScore(int points) 
    { 
        score += points; 
        if (score > highScore) 
        {
            highScore = score;
            // Save to computer memory
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public void PenaltyScore(int penaltyAmount)
    {
        score -= penaltyAmount;
        if (score < 0) score = 0; 
    }

    public int GetScore() => score;

    public int GetHighScore() => highScore; // Only one version now!

    public void ResetGame()
    {
        score = 0;
        playerCurrentHealth = playerMaxHealth;
    }

    // --- Visuals ---

    void UpdateIcon()
    {
        if (playerIcon == null) return;
        playerIcon.sprite = SceneManager.GetActiveScene().name.Contains("Level3") ? quagsireSprite : wooperSprite;
    }
}