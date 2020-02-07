using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackDroneBay : Turret
{
    int droneCount = 0;

    public Transform droneSpawnPos1;
    public Transform droneSpawnPos2;

    public GameObject attackDronePrefab = null;

    public override void Attack()
    {
        if (droneCount < 2)
        {
            SpawnDrone();
        }
    }

    int side = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        Attack();
    }

    public void SpawnDrone()
    {
        side *= -1;
        switch (side)
        {
            case -1:
                Instantiate(attackDronePrefab, droneSpawnPos1.position, droneSpawnPos1.rotation, transform);
                break;
            case 1:
                Instantiate(attackDronePrefab, droneSpawnPos2.position, droneSpawnPos2.rotation, transform);
                break;
        }

        if (enemies.Count > 0)
        {
            bool doneBefore = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                AttackDrone atk = null;
                transform.GetChild(i).TryGetComponent(out atk);

                if (atk)
                {
                    atk.enemyToAttack = enemies.Peek().gameObject;
                    if(!doneBefore)
                    {
                    atk.side = side;
                    doneBefore = true;
                    }
                    else
                    {
                        atk.side = side * -1;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = null;

        collision.gameObject.TryGetComponent(out enemy);

        if (enemy)
        {
            enemies.Enqueue(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = null;

        collision.gameObject.TryGetComponent(out enemy);

        if (enemy)
        {
            enemies.ToList().Remove(enemy);
            enemies = new Queue<Enemy>(enemies);
        }
    }
}
