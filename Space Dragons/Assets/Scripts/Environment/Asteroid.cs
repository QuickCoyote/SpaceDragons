using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject itemtodrop = null;
    public GameObject asteroid = null;

    Health health;
    public float sizeAndWeight = 0;

    Rigidbody2D rb;
    public void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        if (sizeAndWeight == 0) sizeAndWeight = Random.value + 0.2f;
        setSizeAndWeight(sizeAndWeight);

        Vector2 randomForce = new Vector2(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f)); // Sends them in any random direction
        rb.AddForce(randomForce, ForceMode2D.Force);
    }

    public void setSizeAndWeight(float sizeweight)
    {
        sizeAndWeight = sizeweight;
        health.healthMax = 100 * sizeweight;
        health.ResetHealth();
        transform.localScale = new Vector3(sizeweight, sizeweight, 1);
        rb.mass = sizeweight;
    }

    public void Update()
    {
        if (health.healthCount < 0.0f)
        {
            KillAsteroid();
        }
    }

    public void KillAsteroid()
    {
        if (sizeAndWeight > .25f && asteroid) // if larger than a 1/4 asteroid
        {
            for (int i = 0; i < 2; i++)
            {
                Asteroid child = Instantiate(asteroid, transform.parent).GetComponent<Asteroid>(); // creates new asteroids at 1/2 the size
                if (child)
                {
                    child.sizeAndWeight = (sizeAndWeight - 0.20f);
                    GetComponentInParent<AsteroidCluster>().asteroids.Add(child);
                }
            }
        }
        if (itemtodrop)
        {
            Instantiate(itemtodrop, null); // drops item in world space
        }
        GetComponentInParent<AsteroidCluster>().asteroids.Remove(this);
        Destroy(gameObject);
    }
}

