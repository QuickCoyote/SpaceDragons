using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Projectile
{
    private void FixedUpdate()
    {
        lifetime -= Time.deltaTime;

        Move();

        if (lifetime < 0.0f)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != parentobj) // make sure its not hitting itself
        {
            Enemy enemy = null;
            collision.gameObject.TryGetComponent(out enemy);

            if (enemy)
            {
                parentobj.transform.parent.GetComponent<FlameTurret>().enemiesToBurn.Enqueue(enemy);
            }
        }
    }

    public void Fire()
    {
        fireSFX = GetComponent<AudioSource>();
        fireSFX.pitch = fireSFX.pitch + Random.Range(-.25f, .25f);
        if (fireSFX) fireSFX.Play();
        goDirection = parentobj.transform.up;
    }

    public void Move()
    {
        transform.position += (goDirection * bulletSpeed * Time.smoothDeltaTime);
    }
}
