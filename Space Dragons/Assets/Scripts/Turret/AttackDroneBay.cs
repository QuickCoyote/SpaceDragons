using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackDroneBay : Turret
{
    public Transform droneSpawnPos1;
    public Transform droneSpawnPos2;

    public GameObject attackDronePrefab = null;

    public int droneCount = 0;
    int side = 1;

    public override void Attack()
    {
        if (droneCount < 2)
        {
            SpawnDrone();
        }
    }


    void FixedUpdate()
    {
        Attack();
        CheckForDie();
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
        droneCount++;

        for (int i = 0; i < transform.childCount; i++)
        {
            AttackDrone atk = null;
            transform.GetChild(i).TryGetComponent(out atk);

            if (atk)
            {
                atk.side = side;
            }
            side *= -1;
        }

        if (enemies.Count > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                AttackDrone atk = null;
                transform.GetChild(i).TryGetComponent(out atk);

                if (atk)
                {
                    if(enemies.Peek())
                    {
                        atk.enemyToAttack = enemies.Peek().gameObject;
                    }
                    else
                    {
                        atk.enemyToAttack = null;
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
