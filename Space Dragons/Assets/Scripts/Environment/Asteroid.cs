using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    
    [SerializeField] List<Sprite> asteroidImages = null;
    [SerializeField] List<float> sizes = new List<float>();
    
    public float sizeAndWeight = 1;
    public float maxHp = 50.0f;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Health hp;
    WorldManager worldManager;

    public void Start()
    {
        worldManager = WorldManager.Instance;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = asteroidImages[Random.Range(0, asteroidImages.Count)];

        hp = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        setSizeAndWeight(sizes[Random.Range(0, sizes.Count)]);

        Vector2 randomForce = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
        rb.AddForce(randomForce, ForceMode2D.Force);
        rb.AddTorque(Random.value * 15, ForceMode2D.Force);
    }

    public void setSizeAndWeight(float sizeweight)
    {
        sizeAndWeight = sizeweight;
        hp.healthMax = maxHp / 2 * sizeweight;
        hp.ResetHealth();
        transform.localScale = new Vector3(sizeweight, sizeweight, 1);
        rb.mass = sizeweight;
    }

    public void FixedUpdate()
    {
        if (hp.healthCount < 0.0f)
        {
            KillAsteroid();
        }
    }

    public void KillAsteroid()
    {
        ItemObject item = worldManager.SpawnFromPool("Item", transform.position, transform.rotation).GetComponent<ItemObject>();
        if (item)
        {
            item.itemData = worldManager.GetRandomItemDataStepped();
            item.image.sprite = item.itemData.itemImage;
        }

        AsteroidBreakup breakup = worldManager.SpawnFromPool("AsteroidDestruction", transform.position, Quaternion.identity).GetComponent<AsteroidBreakup>();
        if (breakup) breakup.Activate();
        hp.healthCount = hp.healthMax;
        gameObject.SetActive(false);
        worldManager.AsteroidsToRender.Remove(gameObject);
    }
}

