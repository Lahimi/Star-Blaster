using UnityEngine;

public class PathFinding : MonoBehaviour
{
    WaveConfigSO waveConfig;
    Transform[] waypoints;
    int waypointIndex = 0;
    bool isInitialized = false;

    void Start()
    {
        // If waveConfig wasn't set by the Spawner during Instantiate, find it ourselves
        if (waveConfig == null)
        {
            EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
            if (spawner != null)
            {
                SetWaveConfig(spawner.GetCurrentWave());
            }
        }
    }

    public void SetWaveConfig(WaveConfigSO config)
    {
        if (config == null) 
        {
            Debug.LogError($"❌ SetWaveConfig called with null config on {gameObject.name}");
            return;
        }
        
        waveConfig = config;
        SetupPath();
    }

    void SetupPath()
    {
        waypoints = waveConfig.GetWaypoints();
        waypointIndex = 0;
        
        if (waypoints != null && waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            isInitialized = true;
        }
        else
        {
            Debug.LogError($"❌ No waypoints found in WaveConfig for {gameObject.name}");
        }
    }

    void Update()
    {
        if (!isInitialized || waveConfig == null || waypoints == null) return;

        if (waypointIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[waypointIndex].position;
            
            // FIXED LINE: Changed GetMoveSpeed to GetEnemyMoveSpeed
            float moveDelta = waveConfig.GetEnemyMoveSpeed() * Time.deltaTime;
            
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDelta);
            
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                waypointIndex++;
            }
        }
        else
        {
            // Reached the end of the path
            Destroy(gameObject);
        }
    }
}