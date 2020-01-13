using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] Enemy EnemyPrefab = null;

    public int EnemiesMin = 500;
    public int EnemiesMax = 800;

    public List<Enemy> Enemies = new List<Enemy>();

    void Start()
    {
        Vector2 worldSize = FindObjectOfType<WorldManager>().WorldCorner.position;

        for (int i = 0; i < Random.Range(EnemiesMin, EnemiesMax); i++)
        {
            Vector2 location = new Vector2(Random.Range(-worldSize.x, worldSize.x), Random.Range(-worldSize.y, worldSize.y)); //select spot for cluster
            Enemies.Add(Instantiate(EnemyPrefab, location, Quaternion.identity, transform));
        }
    }

    void Update()
    {
        
    }
}
