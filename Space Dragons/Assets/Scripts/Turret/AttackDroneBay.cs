using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackDroneBay : Turret
{
    public Transform droneSpawnPos1;
    public Transform droneSpawnPos2;

    public GameObject[] drones = new GameObject[2];

    public GameObject attackDronePrefab = null;

    public int droneCount = 0;
    int side = 1;

    List<Enemy> myEnemies = new List<Enemy>();

    public override void Attack()
    {
        foreach (GameObject go in drones)
        {
            AttackDrone atk = null;

            if (go.TryGetComponent(out atk))
            {
                atk.enemyToAttack = myEnemies[0].gameObject;
            }
            else
            {
                atk.enemyToAttack = null;
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
        Attack();
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
                    drones[1] = Instantiate(attackDronePrefab, droneSpawnPos1.position, droneSpawnPos1.rotation, null);
                }
                else
                {
                    drones[0] = Instantiate(attackDronePrefab, droneSpawnPos1.position, droneSpawnPos1.rotation, null);
                }
                break;
            case 1:
                if (!drones[0])
                {
                    drones[1] = Instantiate(attackDronePrefab, droneSpawnPos2.position, droneSpawnPos2.rotation, null);
                }
                else
                {
                    drones[0] = Instantiate(attackDronePrefab, droneSpawnPos2.position, droneSpawnPos2.rotation, null);
                }
                break;
        }

        foreach (GameObject go in drones)
        {
            AttackDrone atk = null;

            if (go)
            {
                if (go.TryGetComponent(out atk))
                {
                    atk.side = side;
                }
                side *= -1;
            }
        }
        droneCount++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = null;

        if (collision.gameObject.TryGetComponent(out enemy))
        {
            myEnemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = null;

        if (collision.gameObject.TryGetComponent(out enemy))
        {
            myEnemies.Remove(enemy);
        }
    }
}
