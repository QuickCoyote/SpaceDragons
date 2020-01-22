using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : Singleton<AsteroidManager>
{
    [SerializeField] AsteroidCluster asteroidClusterPrefab = null;
    [SerializeField] GameObject asteroidBreakupPrefab = null;
    public int ClusterMinimum = 300;
    public int ClusterMaximum = 500;
    public List<AsteroidCluster> asteroidClusters = new List<AsteroidCluster>();

    // Start is called before the first frame update
    void Start()
    {
        Vector2 worldSize = WorldManager.Instance.WorldCorner.position;
        for (int i = 0; i < Random.Range(ClusterMinimum, ClusterMaximum); i++)
        {
            Vector2 location = new Vector2(Random.Range(-worldSize.x, worldSize.x), Random.Range(-worldSize.y, worldSize.y)); //select spot for cluster
            asteroidClusters.Add(Instantiate(asteroidClusterPrefab, location, Quaternion.identity, transform));
        }
    }

    public void SpawnAsteroidDestruction(Vector3 target)
    {
        Instantiate(asteroidBreakupPrefab, target, Quaternion.identity, null);
    }
}
