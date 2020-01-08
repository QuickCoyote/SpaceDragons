using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float sizeAndWeight;
    public GameObject itemtodrop = null;
    public GameObject asteroid = null;

    Health health;
    float driftTimer;
    float minSize = 0.2f;

    Rigidbody2D rb;
    public void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        driftTimer = Random.Range(0.0f, 5.0f);

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
        driftTimer -= Time.deltaTime;
        if (driftTimer < 0.0f)
        {
            Vector2 randomForce = new Vector2(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f)); // Sends them in any random direction
            rb.AddForce(randomForce, ForceMode2D.Force);
            driftTimer = Random.Range(0.0f, 5.0f);
        }


        if (health.healthCount < 0.0f)
        {
            if (health.healthMax > 25.0f && asteroid) // if larger than a 1/4 asteroid
            {
                Asteroid child1 = Instantiate(asteroid, null).GetComponent<Asteroid>(); // creates new asteroids at 1/2 the size
                child1.setSizeAndWeight(sizeAndWeight / 2);
                Asteroid child2 = Instantiate(asteroid, null).GetComponent<Asteroid>();
                child1.setSizeAndWeight(sizeAndWeight / 2);

            }
            if (itemtodrop)
            {
                Instantiate(itemtodrop, null); // drops item in world space
            }
            Destroy(gameObject);
        }
    }

}

