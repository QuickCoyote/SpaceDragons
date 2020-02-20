using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : Singleton<AsteroidManager>
{
    [SerializeField] GameObject asteroidBreakupPrefab = null;
    [SerializeField] Asteroid[] asteroidPrefabs;

    [SerializeField] int ClusterNum = 800;
    [SerializeField] int AsteroidMinimum = 2;
    [SerializeField] int AsteroidMaximum = 5;

    public List<Asteroid> asteroids = new List<Asteroid>();

    public int AsteroidsDestroyed = 0;

    void Start()
    {
        Vector2 worldSize = WorldManager.Instance.WorldCorner.position;
        for (int i = 0; i < ClusterNum; i++)
        {
            Vector3 location = new Vector3(Random.Range(-worldSize.x, worldSize.x), Random.Range(-worldSize.y, worldSize.y),0); //select spot for cluster
            for (int j = 0; j < Random.Range(AsteroidMinimum, AsteroidMaximum); j++)
            {
                asteroids.Add(Instantiate(asteroidPrefabs[Random.Range(0,3)], location + new Vector3(Random.value, Random.value, 0), Quaternion.identity, null)); //Select smaller locations for each asteroid
            }
        }
    }

    private void Update()
    {
        if (AsteroidsDestroyed > 8)
        {
            Vector3 location = WorldManager.Instance.Head.transform.position + new Vector3(100, 100, 0); //select spot for cluster
            for (int j = 0; j < Random.Range(AsteroidMinimum, AsteroidMaximum); j++)
            {
                asteroids.Add(Instantiate(asteroidPrefabs[Random.Range(0, 3)], location + new Vector3(Random.value, Random.value, 0), Quaternion.identity, null)); //Select smaller locations for each asteroid
            }
            AsteroidsDestroyed = 0;
        }
    }

    public void SpawnAsteroidDestruction(Vector3 target)
    {
        Instantiate(asteroidBreakupPrefab, target, Quaternion.identity, null);
    }
}
