using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] CanvasGroup fadeCanvasGroup;
    [SerializeField] float fadeDuration = 1f;
    
    public static LevelManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- SCENE LOADING EVENTS ---

    void OnEnable()
    {
        // Tell Unity to run "OnSceneLoaded" every time a new scene starts
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When we enter a new scene, instantly ensure the screen is black, then fade out
        if (fadeCanvasGroup != null)
        {
            StopAllCoroutines(); // Stop any leftover "Fade In" from the previous level
            fadeCanvasGroup.alpha = 1f;
            fadeCanvasGroup.blocksRaycasts = true;
            StartCoroutine(Fade(0f));
        }
    }

    // --- FADING LOGIC ---

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(Fade(1f)); // Fade to Black
        SceneManager.LoadScene(sceneName);     // Load Scene (OnSceneLoaded will handle the fade out)
    }

    IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;

        float startAlpha = fadeCanvasGroup.alpha;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            yield return null;
        }
        
        fadeCanvasGroup.alpha = targetAlpha;
        
        // If it's transparent (0), stop blocking clicks. If it's black (1), block clicks.
        fadeCanvasGroup.blocksRaycasts = (targetAlpha == 1f);
    }
}