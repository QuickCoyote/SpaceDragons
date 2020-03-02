using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : Singleton<AsteroidManager>
{
    [SerializeField] GameObject asteroidBreakupPrefab = null;
    [SerializeField] GameObject[] asteroidPrefabs;

    [SerializeField] int ClusterNum = 800;
    [SerializeField] int AsteroidMinimum = 2;
    [SerializeField] int AsteroidMaximum = 5;

    public int AsteroidsDestroyed = 0;

    WorldManager worldManager;

    [System.Serializable]
    public class Pool
    {
        public string tag = "";
        public GameObject objectPrefab;
        public int maxNumOfObject;
    }

    public List<Pool> Pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();
    void Start()
    {
        worldManager = WorldManager.Instance;
        foreach(Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.maxNumOfObject; i++)
            {
                GameObject obj = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)]);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            objectPools.Add(pool.tag, objectPool);
        }

        for (int i = 0; i < ClusterNum; i++)
        {
            Vector3 location = new Vector3(Random.Range(-150, 150), Random.Range(-150, 150),0);
            for (int j = 0; j < Random.Range(AsteroidMinimum, AsteroidMaximum); j++)
            {
                SpawnFromPool("Asteroid", location + new Vector3(Random.value, Random.value, 0), Quaternion.identity);
            }
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!objectPools.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = objectPools[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        if(Vector3.Distance(objectToSpawn.transform.position, worldManager.transform.position) < 50)
        {
            worldManager.AsteroidsToRender.Add(objectToSpawn);
        }

        objectPools[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    private void FixedUpdate()
    {
        float val = Random.Range(-1, 1);
        float val2 = Random.Range(-1, 1);
        if (val < 0)
        {
            val = -1;
        }
        else
        {
            val = 1;
        }
        if (val2 < 0)
        {
            val2 = -1;
        }
        else
        {
            val2 = 1;
        }
        Vector3 location = new Vector3(Random.Range(50, 100) * val, Random.Range(50, 100) * val2, 0);

        if (AsteroidsDestroyed > 8)
        {
            location += worldManager.Head.transform.position;
            for (int j = 0; j < Random.Range(AsteroidMinimum, AsteroidMaximum); j++)
            {
                SpawnFromPool("Asteroid", location, Quaternion.identity);
            }
            AsteroidsDestroyed = 0;
        }

        foreach(GameObject asteroid in objectPools["Asteroid"])
        {
            if(Vector3.Distance(asteroid.transform.position, worldManager.transform.position) > 150)
            {
                asteroid.transform.position = location;
            }
        }
    }

    public void SpawnAsteroidDestruction(Vector3 target)
    {
        SpawnFromPool("Asteroid", target, Quaternion.identity);
    }
}
