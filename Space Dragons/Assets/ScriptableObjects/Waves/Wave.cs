using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    [SerializeField] List<string> myEnemies = new List<string>();
    [SerializeField] float minSpawnDistance = -50.0f;
    [SerializeField] float maxSpawnDistance = 50.0f;

    public void StartWave()
    {
        GameObject player = EnemyWaveManager.Instance.Player;

        for (int i = 0; i < myEnemies.Count; i++)
        {
            float randX = Random.Range(minSpawnDistance, maxSpawnDistance);
            float randY = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 spawnPosition = new Vector3(player.transform.position.x + randX, player.transform.position.y + randY, 0.0f);

            WorldManager.Instance.SpawnFromPool(myEnemies[i], spawnPosition, Quaternion.identity).GetComponent<Enemy>().Player = player;
            EnemyWaveManager.Instance.aliveEnemies++;
        }
    }
}
