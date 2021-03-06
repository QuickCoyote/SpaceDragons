﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : Singleton<AsteroidManager>
{
    [SerializeField] int ClusterNum = 100;
    [SerializeField] int AsteroidMinimum = 2;
    [SerializeField] int AsteroidMaximum = 5;

    public int AsteroidsDestroyed = 0;

    WorldManager worldManager;

    float val = 1;
    float val2 = 1;
    void Start()
    {
        worldManager = WorldManager.Instance;

        for (int i = 0; i < ClusterNum; i++)
        {
            for (int j = 0; j < Random.Range(AsteroidMinimum, AsteroidMaximum); j++)
            {
                GameObject asteroid = worldManager.SpawnFromPool(WorldManager.ePoolTag.ASTEROID, new Vector3(Random.Range(-150, 150), Random.Range(-150, 150), 0) + new Vector3(Random.value, Random.value, 0), Quaternion.identity);
                asteroid.GetComponent<Rigidbody2D>().AddTorque(Random.value * 15, ForceMode2D.Force);
            }
        }
    }

    private void FixedUpdate()
    {
        val = Random.Range(-1, 1);
        val2 = Random.Range(-1, 1);
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
                GameObject asteroid = worldManager.SpawnFromPool(WorldManager.ePoolTag.ASTEROID, location, Quaternion.identity);
                asteroid.GetComponent<Rigidbody2D>().AddTorque(Random.value * 15, ForceMode2D.Force);
            }
            AsteroidsDestroyed = 0;
        }

        Instance.StartCoroutine("MoveAsteroids");
    }

    IEnumerator MoveAsteroids()
    {
        foreach (GameObject asteroid in worldManager.objectPools[WorldManager.ePoolTag.ASTEROID])
        {
            val *= -1;
            val2 *= -1;
            if (Vector3.Distance(asteroid.transform.position, worldManager.transform.position) > 150)
            {
                Vector3 location2 = new Vector3(Random.Range(50, 100) * val, Random.Range(50, 100) * val2, 0);
                location2 += worldManager.Head.transform.position;
                asteroid.transform.position = location2;
            }
        }

        yield return new WaitForSeconds(5f);
    }
}
