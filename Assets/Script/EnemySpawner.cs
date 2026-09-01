using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] WaveConfigSO[] waveConfigs;
    [SerializeField] float timeBetweenWaves = 1f;
    [SerializeField] BossDialogue bossDialogue;
    [SerializeField] int bossWaveIndex = 4; 
    [SerializeField] BossDialogue victoryDialogue; 
    [SerializeField] string nextLevelName;

    WaveConfigSO currentWave;
    int currentWaveIndex = 0;
    List<GameObject> activeEnemies = new List<GameObject>();
    bool dialogueActive = false;

    void Start() { StartCoroutine(RunWaves()); }

    IEnumerator RunWaves()
    {
        while (currentWaveIndex < waveConfigs.Length)
        {
            currentWave = waveConfigs[currentWaveIndex];

            if (currentWaveIndex == bossWaveIndex && bossDialogue != null)
            {
                dialogueActive = true;
                bossDialogue.StartBossDialogue();
                yield return new WaitUntil(() => dialogueActive == false);
            }
            
            yield return StartCoroutine(RunCurrentWave());
            
            if (currentWaveIndex == bossWaveIndex && victoryDialogue != null)
            {
                dialogueActive = true;
                victoryDialogue.StartBossDialogue();
                yield return new WaitUntil(() => dialogueActive == false);
                
                if (!string.IsNullOrEmpty(nextLevelName))
                    SceneManager.LoadScene(nextLevelName);
            }

            currentWaveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
    
    IEnumerator RunCurrentWave()
    {
        yield return StartCoroutine(SpawnEnemies());
        while (activeEnemies.Count > 0)
        {
            activeEnemies.RemoveAll(item => item == null);
            yield return null; 
        }
    }
    
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < currentWave.GetEnemyCount(); i++)
        {
            GameObject enemy = Instantiate(currentWave.GetEnemyPrefab(i), currentWave.GetStartingWaypoint().position, Quaternion.identity, transform);
            activeEnemies.Add(enemy);
            Health h = enemy.GetComponent<Health>();
            if (h != null) h.OnDeath += () => activeEnemies.Remove(enemy);
            yield return new WaitForSeconds(currentWave.GetRandomEnemySpawnTime());
        }
    }

    public int GetCurrentWaveIndex() => currentWaveIndex;
    public WaveConfigSO GetCurrentWave() => currentWave;
    public void OnDialogueEnded() => dialogueActive = false;
}