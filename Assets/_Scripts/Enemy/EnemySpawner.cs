using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    private Transform _playerTransform;
    public List<Wave> Waves; // A list of all waves in the game
    public int CurrentWaveCount; // The index of the current wave

    [Header("Spawner Attributes")]
    private float _spawnTimer; // Timer use to determine when to spawn the next enemy

    public float WaveInterval; // The interval between each waves
    public int EnemiesAlive;
    public int MaxEnemiesAllowed; // The maximum enemy allowed in the map
    public bool IsMaxEnemiesReached = false; // A flag indicates if the maximum number of enemies has been reached

    [Header("Spawn Positions")]
    public List<Transform> RelativeSpawnPoints; // A list to store all the relative spawn points of enemies

    [System.Serializable]
    public class Wave
    {
        public string WaveName;
        public List<EnemyGroup> EnemyGroups;
        public int WaveQuota; //The total number of enemies to spawn in this wave
        public float SpawnInterval; //The interval at which to spawn enemies
        public int SpawnCount; //The number of enemies already spawned in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject EnemyPrefab; //List of enemy prefabs for this wave
        public string EnemyName;
        public int EnemyCount; //The number of enemies to spawn in this wave
        public int SpawnCount; //The number of enemies of this type that already spawned in this wave
    }

    private void Start()
    {
        _playerTransform = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    private void Update()
    {
        if (CurrentWaveCount < Waves.Count && Waves[CurrentWaveCount].SpawnCount == 0) //Check if the wave has ended spawning and the next wave should start spawning
        {
            StartCoroutine(BeginNextWave());
        }

        _spawnTimer += Time.deltaTime;

        // check if its time to spawn the next enemies
        if (_spawnTimer >= Waves[CurrentWaveCount].SpawnInterval)
        {
            _spawnTimer = 0;
            SpawnEnemies();
        }
    }

    private IEnumerator BeginNextWave()
    {
        //Wave for waveInterval seconds before starting the next wave.
        yield return new WaitForSeconds(WaveInterval);

        // If there are more waves to start after the current wave, move on to the next wave
        if (CurrentWaveCount < Waves.Count - 1)
        {
            CurrentWaveCount++;
            CalculateWaveQuota();
        }
    }

    private void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in Waves[CurrentWaveCount].EnemyGroups)
        {
            currentWaveQuota += enemyGroup.EnemyCount;
        }

        Waves[CurrentWaveCount].WaveQuota = currentWaveQuota;
        //Debug.LogWarning(currentWaveQuota);
    }

    /// <summary>
    /// This method will stop spawning enemies if the amount of enemies on the map is maximum.
    /// The method will only spawn enemies in a particular wave until it is time for the next wave's enemies to be spawned.
    /// </summary>

    private void SpawnEnemies()
    {
        // Check if the minimum number of enemies in the wave have been spawned
        if (Waves[CurrentWaveCount].SpawnCount < Waves[CurrentWaveCount].WaveQuota && !IsMaxEnemiesReached)
        {
            // Spawn each type of enemy until the quota is filled
            foreach (var enemyGroup in Waves[CurrentWaveCount].EnemyGroups)
            {
                // check if the minimum number of enemies of this type have been spawned
                if (enemyGroup.SpawnCount < enemyGroup.EnemyCount)
                {
                    //Limit the number of enemies that can be spawned at once
                    if (EnemiesAlive >= MaxEnemiesAllowed)
                    {
                        IsMaxEnemiesReached = true;
                        return;
                    }

                    //Spawn the enemy at a random position close to the player
                    Instantiate(enemyGroup.EnemyPrefab, _playerTransform.position + RelativeSpawnPoints[Random.Range(0, RelativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.SpawnCount++;
                    Waves[CurrentWaveCount].SpawnCount++;
                    EnemiesAlive++;
                }
            }
        }

        //Reset the IsMaxEnemiesReached flag if the number of enemies alive has dropped below the max amount
        if (EnemiesAlive < MaxEnemiesAllowed)
        {
            IsMaxEnemiesReached = false;
        }
    }

    // Call this function when the enemies is killed
    public void OnEnemyKilled()
    {
        EnemiesAlive--;
    }
}