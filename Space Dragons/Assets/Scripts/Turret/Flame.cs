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

            FlameTurret turret = parentobj.transform.parent.GetComponent<FlameTurret>();

            if (enemy)
            {
                if(turret.enemiesToBurn.Contains(enemy))
                {
                    return;
                }
                else
                {
                    turret.enemiesToBurn.Enqueue(enemy);
                }
            }
        }
    }

    public new void Fire()
    {
        if(goDirection == null)
        {
            goDirection = parentobj.transform.up;
        }
    }

    public void Move()
    {
        transform.position += (goDirection * bulletSpeed * Time.smoothDeltaTime);
    }
}
