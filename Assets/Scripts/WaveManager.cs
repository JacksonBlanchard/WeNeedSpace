using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveState { IN_PROGRESS, COMPLETED }

public class WaveManager : MonoBehaviour
{
    // Wave variables
    private int m_waveNumber = 0;
    private int m_waveWeight = 0;
    private WaveState m_waveState;

    // Timer variables
    private float m_spawnTimer;
    [SerializeField] private float m_timeBetweenSpawns = 1.5f;
    private float m_waveTimer;
    [SerializeField] private float m_timeBetweenWaves = 10f;

    // Enemy variables
    private int m_numEasyEnemies;
    [SerializeField] private int m_easyEnemyWeight;
    [SerializeField] private GameObject m_easyEnemyPrefab;
    private int m_numMediumEnemies;
    [SerializeField] private int m_mediumEnemyWeight;
    [SerializeField] private GameObject m_mediumEnemyPrefab;
    private int m_numHardEnemies;
    [SerializeField] private int m_hardEnemyWeight;
    [SerializeField] private GameObject m_hardEnemyPrefab;

    // Mothership spawn location
    [SerializeField] private Transform m_spawnPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        // Increase the wave variables
        m_waveNumber += 1;
        m_waveWeight = 10 * m_waveNumber;
        
        DetermineWaveEnemies();

        // Initialize the timers
        m_spawnTimer = m_timeBetweenSpawns;
        m_waveTimer = m_timeBetweenWaves;

        // Immediately start the first wave
        m_waveState = WaveState.IN_PROGRESS;
        Debug.Log("Wave " + m_waveNumber + " started!");
    }

    // Update is called once per frame
    void Update()
    {
        // Use only for development purposes. Pressing Enter should destroy all enemies to complete the current wave
        if (Input.GetKey(KeyCode.Return))
        {
            TestCompleteWave();
        }

        // Continue with the wave
        if(m_waveState == WaveState.IN_PROGRESS)
        {
            // Decrease the timer buy each frame duration
            m_spawnTimer -= Time.deltaTime;

            // Once the spawn timer equals zero, spawn the next enemy and reset the spawn timer
            if(m_spawnTimer <= 0)
            {
                SpawnEnemy();
            }

            // If all enemies have been spawned and destroyed, then the wave has been completed
            if(m_numEasyEnemies + m_numMediumEnemies + m_numHardEnemies == 0 && transform.childCount == 0)
            {
                WaveCompleted();
            }
        }        
        // If a wave has been completed, we are inbetween waves
        else if(m_waveState == WaveState.COMPLETED)
        {
            // Decrease the timer by each frame duration
            m_waveTimer -= Time.deltaTime;

            // Once the wave timer hits zero, start the next wave
            if(m_waveTimer <= 0)
            {
                Start();
            }
        }
    }

    public void WaveCompleted()
    {
        m_waveState = WaveState.COMPLETED;
        Debug.Log("Wave " + m_waveNumber + " completed!");
    }

    private void SpawnEnemy()
    {
        // Spawn the easy enemies first
        if (m_numEasyEnemies > 0)
        {
            GameObject easyEnemy = GameObject.Instantiate(m_easyEnemyPrefab, transform, m_spawnPosition);
            easyEnemy.name = "EasyEnemy" + m_numEasyEnemies;
            Debug.Log("Spawned " + easyEnemy.name);
            // Decrease the number of medium enemies left to spawn
            m_numEasyEnemies -= 1;
        }
        // Then spawn the medium enemies
        else if (m_numMediumEnemies > 0)
        {
            GameObject mediumEnemy = GameObject.Instantiate(m_mediumEnemyPrefab, transform, m_spawnPosition);
            mediumEnemy.name = "MediumEnemy" + m_numMediumEnemies;
            Debug.Log("Spawned " + mediumEnemy.name);
            // Decrease the number of medium enemies left to spawn
            m_numMediumEnemies -= 1;
        }
        // Then spawn the hard enemies
        else if (m_numHardEnemies > 0)
        {
            GameObject hardEnemy = GameObject.Instantiate(m_hardEnemyPrefab, transform, m_spawnPosition);
            hardEnemy.name = "HardEnemy" + m_numHardEnemies;
            Debug.Log("Spawned " + hardEnemy.name);
            // Decrease the number of hard enemies left to spawn
            m_numHardEnemies -= 1;
        }

        // Reset the spawn timer
        m_spawnTimer = m_timeBetweenSpawns;
    }

    /// <summary>
    /// Determine how many of each enemy type will be spawned in the next wave.
    /// This is based on the weight of the incoming wave.
    /// </summary>
    private void DetermineWaveEnemies()
    {
        // Generate a random number of hard enemies between 0 and the max number possible given the current wave weight
        m_numHardEnemies = Random.Range(0, (int)(m_waveWeight / m_hardEnemyWeight));
        // Subtract the total enemy weight from the wave weight
        m_waveWeight -= (m_numHardEnemies * m_hardEnemyWeight);

        // Generate a random number of medium enemies between 0 and the max number possible given the current wave weight
        m_numMediumEnemies = Random.Range(0, (int)(m_waveWeight / m_mediumEnemyWeight));
        // Subtract the total enemy weight from the wave weight
        m_waveWeight -= (m_numMediumEnemies * m_mediumEnemyWeight);

        // The rest of the wave is filled with easy enemies
        m_numEasyEnemies = m_waveWeight / m_easyEnemyWeight;
        // Subtract the total enemy weight from the wave weight
        m_waveWeight -= (m_numEasyEnemies * m_easyEnemyWeight);
    }


    void TestCompleteWave()
    {
        foreach(Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
    }
}