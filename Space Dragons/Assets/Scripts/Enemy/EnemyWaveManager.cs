using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWaveManager : Singleton<EnemyWaveManager>
{
    [SerializeField] float waveSpawnTimer = 25.0f;

    [SerializeField] List<Wave> waves = new List<Wave>();
  
    public int currentWave = 0;
    public int cycleCount = 0;
    public GameObject Player = null;

    float dt = 0.0f;

    void Start()
    {
        Player = WorldManager.Instance.Player;
    }

    void FixedUpdate()
    {
        dt += Time.deltaTime;

        if(dt >= waveSpawnTimer)
        {
            for(int i = 0; i <= cycleCount; i++)
            {
                waves[currentWave].StartWave();
            }
            dt = 0.0f;
            currentWave++;
        }

        if(currentWave == 10)
        {
            currentWave = 0;
            cycleCount++;
        }
    }

    public void ReduceCycleCount(int amount)
    {
        if(cycleCount - amount > 0)
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
