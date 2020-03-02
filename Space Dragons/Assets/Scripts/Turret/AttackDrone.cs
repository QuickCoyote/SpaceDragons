using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDrone : MonoBehaviour
{
    public GameObject bullet = null;
    public GameObject parent = null;
    public GameObject enemyToAttack;
    public GameObject bulletSpawn = null;
    public Transform idleLocation = null;

    public float damage;
    public float range;
    public float attackSpeed;
    public float followOffset;
    public float moveSpeed;
    public float myMoveSpeed;
    public float rotationSpeed;
    public int side;

    protected float attackTimer = 0f;

    private Health myHealth = null;
    private Vector3 targetPosition = Vector3.zero;


    private void Start()
    {
        myHealth = GetComponent<Health>();
    }

    public void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackSpeed)
        {
            attackTimer = 0;
            GameObject projectileGO = (Instantiate(bullet, bulletSpawn.transform.position, transform.rotation, null) as GameObject);
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.parentobj = gameObject;
            projectile.Fire();
        }
    }

    void FixedUpdate()
    {
        if(!parent)
        {
            Destroy(gameObject);
        }


        Vector3 direction = targetPosition - transform.position;

        if (enemyToAttack)
        {
            targetPosition = enemyToAttack.transform.position;
            direction = targetPosition - transform.position;
            moveSpeed = 10.0f;
        }
        else
        {

            if (idleLocation)
            {
                targetPosition = idleLocation.position;
            }
            else
            {
                switch(side)
                {
                    case -1:
                        idleLocation = parent.GetComponent<AttackDroneBay>().droneIdlePos1;
                        break;
                    case 1:
                        idleLocation = parent.GetComponent<AttackDroneBay>().droneIdlePos2;
                        break;
                }
            }
            if (Vector3.Distance(transform.position, idleLocation.position) > 50.0f)
            {
                Destroy(gameObject);
            }

            direction = targetPosition - transform.position;

            if (Vector3.Distance(transform.position, targetPosition) > .5f)
            {
                moveSpeed = WorldManager.Instance.Ship.speed + 1;
            }
            else
            {
                moveSpeed = WorldManager.Instance.Ship.speed;
            }
        }

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.Translate(transform.up * moveSpeed * Time.fixedDeltaTime, Space.World);

        CheckForAttack();
        CheckForDie();
    }

    public void CheckForAttack()
    {
        if (enemyToAttack)
        {
            if (Vector3.Distance(enemyToAttack.transform.position, transform.position) < range)
            {
                Attack();
            }
        }
    }

    public void CheckForDie()
    {
        if (myHealth.healthCount <= 0)
        {
            parent.GetComponent<AttackDroneBay>().droneCount--;
            Explosion explosion = WorldManager.Instance.SpawnFromPool("Explosion", transform.position, transform.rotation).GetComponent<Explosion>();
            if (explosion) explosion.Activate();
            Destroy(gameObject);
        }
    }
}
