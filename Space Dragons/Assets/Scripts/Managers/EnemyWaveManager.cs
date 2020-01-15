using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();

    [SerializeField] int StartingEnemies = 5;
    [SerializeField] AnimationCurve difficultyScale = null;

    [SerializeField] float minSpawnDistance = 25.0f;
    [SerializeField] float maxSpawnDistance = 50.0f;

    [SerializeField] float waveSpawnTimer = 25.0f;
    

    public int currentWave = 0;

    public GameObject Player = null;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        CreateSceneParameters sceneParams = new CreateSceneParameters();

        Scene newScene = SceneManager.GetActiveScene();

        

    }
}
