using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] AsteroidCluster asteroidClusterPrefab = null;
    float worldSize = 2000f;
    public int ClusterMinimum = 500;
    public int ClusterMaximum = 800;
    public List<AsteroidCluster> asteroidClusters = new List<AsteroidCluster>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Random.Range(ClusterMinimum, ClusterMaximum); i++)
        {
            Vector2 location = new Vector2(Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize)); //select spot for cluster
            asteroidClusters.Add(Instantiate(asteroidClusterPrefab, location, Quaternion.identity, transform));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
