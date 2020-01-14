using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCluster : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;

    public List<Asteroid> asteroids = new List<Asteroid>();
    float driftTimer;
    public int AsteroidMinimum = 4;
    public int AsteroidMaximum = 10;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        for (int j = 0; j < Random.Range(AsteroidMinimum, AsteroidMaximum); j++)
        {
            asteroids.Add(Instantiate(asteroidPrefab, transform.position + new Vector3(Random.value, Random.value,0), Quaternion.identity, transform)); //Select smaller locations for each asteroid

        }

        foreach (Asteroid a in asteroids)
        {
            a.gameObject.SetActive(false);
        }
        driftTimer = Random.Range(0.0f, 5.0f);
    }

    void FixedUpdate()
    {
        if (asteroids.Count == 0)
        {
            GetComponentInParent<AsteroidManager>().asteroidClusters.Remove(this);
        }
        driftTimer -= Time.deltaTime;
        if (driftTimer < 0.0f)
        {
            Vector2 randomForce = new Vector2(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f)); // Sends them in any random direction
            rb.AddForce(randomForce, ForceMode2D.Force);
            driftTimer = Random.Range(0.0f, 5.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach (Asteroid a in asteroids)
            {
                a.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            foreach (Asteroid a in asteroids)
            {
                a.gameObject.SetActive(false);
            }
        }
    }
}
