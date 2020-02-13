using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWaveManager : Singleton<EnemyWaveManager>
{
    [SerializeField] float waveSpawnTimer = 25.0f;

    [SerializeField] List<Wave> waves = new List<Wave>();
    [SerializeField] TextMeshProUGUI WaveText = null;
    [SerializeField] TextMeshProUGUI EnemiesText = null;

    public int currentWave = 0;
    public int cycleCount = 0;
    public GameObject Player = null;

    public int aliveEnemies = 0;

    float dt = 0.0f;

    void Start()
    {
        Player = WorldManager.Instance.Head;
        currentWave = LoadManager.Instance.saveData.CurrentWave;
        cycleCount = LoadManager.Instance.saveData.CurrentCycle;
        if (currentWave != 0)
        {
            for (int i = 0; i <= cycleCount; i++)
            {
                waves[currentWave].StartWave();
            }
            dt = 0.0f;
        }
    }

    void FixedUpdate()
    {
        dt += Time.deltaTime;

        if (aliveEnemies == 0)
        {
            if (dt >= waveSpawnTimer)
            {
                for (int i = 0; i <= cycleCount; i++)
                {
                    waves[currentWave].StartWave();
                }
                dt = 0.0f;
                currentWave++;
            }
        }

        if (currentWave == 10)
        {
            currentWave = 0;
            cycleCount++;
        }

        WaveText.text = "Wave: " + (currentWave + (cycleCount * 10));
        EnemiesText.text = "Enemies Alive: " + aliveEnemies;
    }

    public void ReduceCycleCount(int amount)
    {
        if (cycleCount - amount > 0)
        {
            cycleCount -= amount;
        }
        else
        {
            cycleCount = 0;
        }
    }

    public void IncreaseCycleCount(int amount)
    {
        cycleCount += amount;
    }
}
