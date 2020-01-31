using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : Singleton<AsteroidManager>
{
    [SerializeField] GameObject asteroidBreakupPrefab = null;
    int ClusterMinimum = 500;
    int ClusterMaximum = 600;
    [SerializeField] Asteroid asteroidPrefab;
    public List<Asteroid> asteroids = new List<Asteroid>();
    int AsteroidMinimum = 4;
    int AsteroidMaximum = 8;

    public int AsteroidsDestroyed = 0;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 worldSize = WorldManager.Instance.WorldCorner.position;
        for (int i = 0; i < Random.Range(ClusterMinimum, ClusterMaximum); i++)
        {
            Vector3 location = new Vector3(Random.Range(-worldSize.x, worldSize.x), Random.Range(-worldSize.y, worldSize.y),0); //select spot for cluster
            for (int j = 0; j < Random.Range(AsteroidMinimum, AsteroidMaximum); j++)
            {
                asteroids.Add(Instantiate(asteroidPrefab, location + new Vector3(Random.value, Random.value, 0), Quaternion.identity, null)); //Select smaller locations for each asteroid
            }
        }
    }

    private void Update()
    {
        if (AsteroidsDestroyed > 8)
        {
            Vector3 location = WorldManager.Instance.Player.transform.position + new Vector3(100, 100, 0); //select spot for cluster
            for (int j = 0; j < Random.Range(AsteroidMinimum, AsteroidMaximum); j++)
            {
                asteroids.Add(Instantiate(asteroidPrefab, location + new Vector3(Random.value, Random.value, 0), Quaternion.identity, null)); //Select smaller locations for each asteroid
            }
            AsteroidsDestroyed = 0;
        }
    }

    public void SpawnAsteroidDestruction(Vector3 target)
    {
        Instantiate(asteroidBreakupPrefab, target, Quaternion.identity, null);
    }
}
