using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackDroneBay : Turret
{
    public Transform droneSpawnPos1;
    public Transform droneSpawnPos2;

    public Transform droneIdlePos1;
    public Transform droneIdlePos2;

    public List<GameObject> drones = new List<GameObject>();

    public GameObject attackDronePrefab = null;

    public int droneCount = 0;
    int side = 1;

    List<Enemy> myEnemies = new List<Enemy>();

    public override void Attack()
    {
        foreach (GameObject go in drones)
        {
            if (go)
            {
                AttackDrone atk = null;

                if (go.TryGetComponent(out atk))
                {
                    if (enemies.Count > 0)
                    {

                        if (enemies.Peek())
                        {
                            atk.enemyToAttack = enemies.Peek().gameObject;
                        }
                        else
                        {
                            enemies.Dequeue();
                        }
                    }
                }
                else
                {
                    atk.enemyToAttack = null;
                }
            }
        }
    }


    void FixedUpdate()
    {
        if (droneCount < 2)
        {
            SpawnDrone();
        }
        AssignParents();
        if (enemies.Count > 0)
        {
            Attack();
        }
        CheckForDie();
    }

    public void AssignParents()
    {
        foreach (GameObject go in drones)
        {
            if (go)
            {
                go.GetComponent<AttackDrone>().parent = gameObject;
                go.GetComponent<AttackDrone>().side = side;
                side *= -1;
            }
        }
    }

    public void SpawnDrone()
    {
        switch (side)
        {
            case -1:
                if (!drones[0])
                {
                    drones.Add(Instantiate(attackDronePrefab, droneSpawnPos1.position, droneSpawnPos1.rotation, null));
                }
                else
                {
                    drones.Add(Instantiate(attackDronePrefab, droneSpawnPos1.position, droneSpawnPos1.rotation, null));
                }
                break;
            case 1:
                if (!drones[0])
                {
                    drones.Add(Instantiate(attackDronePrefab, droneSpawnPos2.position, droneSpawnPos2.rotation, null));
                }
                else
                {
                    drones.Add(Instantiate(attackDronePrefab, droneSpawnPos2.position, droneSpawnPos2.rotation, null));
                }
                break;
        }
        droneCount++;

        foreach (GameObject go in drones)
        {
            AttackDrone atk = null;

            if (go)
            {
                if (go.TryGetComponent(out atk))
                {
                    atk.side = side;
                    switch (side)
                    {
                        case -1:
                            atk.idleLocation = droneIdlePos1;
                            break;
                        case 1:
                            atk.idleLocation = droneIdlePos2;
                            break;
                    }
                }
                side *= -1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = null;

        if (collision.gameObject.TryGetComponent(out enemy))
        {
            enemies.Enqueue(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = null;

        if (collision.gameObject.TryGetComponent(out enemy))
        {
            enemies.ToList().Remove(enemy);
        }
    }
}
