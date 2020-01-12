using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float sizeAndWeight;
    public GameObject itemtodrop = null;
    public GameObject asteroid = null;

    Health health;
    float minSize = 0.2f;

    Rigidbody2D rb;
    public void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();

        setSizeAndWeight(Random.value + minSize);

        transform.localScale = new Vector3(sizeAndWeight, sizeAndWeight, 1);
        rb.mass = sizeAndWeight;

        Vector2 randomForce = new Vector2(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f)); // Sends them in any random direction
        rb.AddForce(randomForce, ForceMode2D.Force);
    }

    public void setSizeAndWeight(float sizeweight)
    {
        sizeAndWeight = sizeweight;
        health.healthMax = 100 * sizeAndWeight;
        health.ResetHealth();
    }

    public void Update()
    {
        if (health.healthCount < 0.0f)
        {
            if (health.healthMax > 25.0f && asteroid) // if larger than a 1/4 asteroid
            {
                Asteroid child1 = Instantiate(asteroid, transform.parent).GetComponent<Asteroid>(); // creates new asteroids at 1/2 the size
                child1.setSizeAndWeight(sizeAndWeight / 2);
                GetComponentInParent<AsteroidCluster>().asteroids.Add(child1);
                Asteroid child2 = Instantiate(asteroid, transform.parent).GetComponent<Asteroid>();
                child2.setSizeAndWeight(sizeAndWeight / 2);
                GetComponentInParent<AsteroidCluster>().asteroids.Add(child2);
            }
            if (itemtodrop)
            {
                Instantiate(itemtodrop, null); // drops item in world space
            }
            Debug.Log(this.name + " Destroyed");

            GetComponentInParent<AsteroidCluster>().asteroids.Remove(this);
            Destroy(gameObject);
        }
    }

}

