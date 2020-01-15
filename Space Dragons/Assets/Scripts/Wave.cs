using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    [SerializeField] List<Enemy> myEnemies = new List<Enemy>();
    [SerializeField] float minSpawnDistance = 25.0f;
    [SerializeField] float maxSpawnDistance = 50.0f;

    [SerializeField] GameObject player = null;

    public void StartWave()
    {
        for (int i = 0; i < myEnemies.Count; i++)
        {
            float randX = Random.Range(minSpawnDistance, maxSpawnDistance);
            float randY = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 spawnPosition = new Vector3(player.transform.position.x + randX, player.transform.position.y + randY, 0.0f);

            Instantiate(myEnemies[i], spawnPosition, Quaternion.identity, null).GetComponent<Enemy>().Player = player;
        }
    }
}
