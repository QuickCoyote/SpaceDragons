using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCluster : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;

    public List<Asteroid> asteroids = new List<Asteroid>();
    public int AsteroidMinimum = 4;
    public int AsteroidMaximum = 10;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        for (int j = 0; j < Random.Range(AsteroidMinimum, AsteroidMaximum); j++)
        {
            asteroids.Add(Instantiate(asteroidPrefab, transform.position + new Vector3(Random.value, Random.value,0), Quaternion.identity, null)); //Select smaller locations for each asteroid
        }
    }

    void FixedUpdate()
    {
        if (asteroids.Count == 0)
        {
            AsteroidManager.Instance.asteroidClusters.Remove(this);
        }
    }
}
