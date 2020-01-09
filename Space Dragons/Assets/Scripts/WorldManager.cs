using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;
    float worldSize = 2500f;
    

    public void Start()
    {
        initializeAsteroids();
    }


    private void initializeAsteroids()
    {
        for (int i = 0; i < Random.Range(500,800); i++)
        {
            Vector2 location = new Vector2(Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize)); //select spot for cluster
            for (int j = 0; j < Random.Range(4, 10); j++)
            {
                Instantiate(asteroidPrefab, location + new Vector2(Random.value, Random.value), Quaternion.identity); //Select smaller locations for each asteroid
            }
        }
    }
}

