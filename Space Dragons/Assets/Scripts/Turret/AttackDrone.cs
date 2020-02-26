using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDrone : MonoBehaviour
{
    public GameObject bullet = null;
    public GameObject parent = null;
    public Transform idleLocation = null;

    public float damage;
    public float range;
    public float attackSpeed;
    public float followOffset;
    public float moveSpeed;
    public float myMoveSpeed;
    public float rotationSpeed;


    Vector3 targetPosition = Vector3.zero;
    public int side;

    protected float attackTimer = 0f;

    [SerializeField] GameObject bulletSpawn = null;
    public GameObject enemyToAttack;

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
        if (enemyToAttack)
        {
            targetPosition = enemyToAttack.transform.position;
            Vector3 direction = targetPosition - transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) > .25f) //Stop moving if player gets too close.
            {
                transform.Translate(transform.up * moveSpeed * Time.fixedDeltaTime, Space.World);
            }
            moveSpeed = 10f;
        }
        else
        {
            moveSpeed = WorldManager.Instance.Ship.speed + 1f;
            if(idleLocation)
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
            Vector3 direction = targetPosition - transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) > 0f)
            {
                transform.Translate(transform.up * moveSpeed * Time.fixedDeltaTime, Space.World);
            }
            else
            {
                moveSpeed = WorldManager.Instance.Ship.speed;
                Vector3 direction2 = targetPosition - transform.position + transform.up;
                float angle2 = Mathf.Atan2(direction2.x, direction2.y) * Mathf.Rad2Deg;
                Quaternion rotation2 = Quaternion.AngleAxis(-angle2, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation2, rotationSpeed * Time.deltaTime);
            }
        }

        CheckForAttack();
        CheckForDie();
    }

    public void CheckForAttack()
    {
        if (enemyToAttack)
        {
            if ((enemyToAttack.transform.position - transform.position).magnitude < range)
            {
                Attack();
            }
        }
    }

    public void CheckForDie()
    {
        if(!parent)
        {
            Destroy(gameObject);
        }
        if (GetComponent<Health>().healthCount <= 0)
        {
            parent.GetComponent<AttackDroneBay>().droneCount--;
            WorldManager.Instance.SpawnRandomExplosion(transform.position);
            Destroy(gameObject);
        }
    }
}
