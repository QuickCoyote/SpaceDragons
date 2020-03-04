using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    [SerializeField] List<GameObject> myEnemies = new List<GameObject>();
    [SerializeField] float minSpawnDistance = 50.0f;
    [SerializeField] float maxSpawnDistance = 75.0f;

    public void StartWave()
    {
        GameObject player = EnemyWaveManager.Instance.Player;

        for (int i = 0; i < myEnemies.Count; i++)
        {
            Vector3 newlocation = new Vector3(Random.Range(minSpawnDistance, maxSpawnDistance), Random.Range(minSpawnDistance, maxSpawnDistance), 0);
            newlocation += player.transform.position;
            newlocation.x *= Random.Range(-1, 1);
            newlocation.y *= Random.Range(-1, 1);

            Instantiate(myEnemies[i], newlocation, Quaternion.identity, null).GetComponent<Enemy>().Player = player;
            EnemyWaveManager.Instance.aliveEnemies++;
        }
    }
}
