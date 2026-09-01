using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class BossDialogue : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] string[] dialogueLines;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject player;
    [SerializeField] bool isVictoryDialogue; 

    int currentLineIndex = 0;
    bool isDialogueActive = false;

    public void StartBossDialogue()
    {
        Debug.Log("Dialogue Started: " + gameObject.name);
        isDialogueActive = true;
        
        if (canvasGroup != null) 
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        
        if (player != null) player.GetComponent<PlayerController>().enabled = false;
        
        currentLineIndex = 0;
        ShowCurrentLine();
    }

    // THIS IS WHAT THE BUTTON CALLS
    public void NextDialogue()
    {
        if (!isDialogueActive) return;

        Debug.Log("Advancing Dialogue. Current Index: " + currentLineIndex);
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length) 
        {
            ShowCurrentLine();
        }
        else 
        {
            Debug.Log("End of dialogue reached.");
            EndDialogue();
        }
    }

    void ShowCurrentLine()
    {
        if (dialogueText != null) dialogueText.text = dialogueLines[currentLineIndex];
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        if (canvasGroup != null) 
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }

        if (!isVictoryDialogue)
        {
            MusicPlayer.Instance?.PlayBossMusic();
            if (boss != null) boss.SetActive(true);
        }
        
        if (player != null) player.GetComponent<PlayerController>().enabled = true;
        
        // Signal the spawner
        FindFirstObjectByType<EnemySpawner>()?.OnDialogueEnded();
    }

    void Update()
    {
        // Check if Space is pressed while dialogue is open
        if (isDialogueActive && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextDialogue();
        }
    }
}