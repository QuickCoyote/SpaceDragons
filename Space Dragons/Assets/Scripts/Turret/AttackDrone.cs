using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDrone : MonoBehaviour
{
    public GameObject bullet = null;

    public float damage;
    public float range;
    public float attackSpeed;
    public float followOffset;
    public float moveSpeed;
    public float myMoveSpeed;
    public float rotationSpeed;

    Vector3 targetPosition = Vector3.zero;
    public int side = 1;

    protected float attackTimer = 0f;

    [SerializeField] GameObject bulletSpawn = null;
    public GameObject enemyToAttack;

    public void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackSpeed)
        {
            attackTimer = 0;
            GameObject projectileGO = (Instantiate(bullet, bulletSpawn.transform.position, transform.rotation, transform) as GameObject);
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.parentobj = gameObject;
            projectile.Fire();
        }
    }

    void Update()
    {
        if(enemyToAttack)
        {
            CheckForAttack();
            targetPosition = (enemyToAttack.transform.position - transform.position).normalized;
        }
        else
        {
            targetPosition = ((transform.parent.position + (transform.parent.right * side * followOffset)) - transform.position).normalized;
        }

        if((transform.position - targetPosition).magnitude < 3)
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = myMoveSpeed;
        }

        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        transform.position += targetPosition * moveSpeed * Time.deltaTime;

        CheckForDie();
    }

    public void CheckForAttack()
    {
        if ((enemyToAttack.transform.position - transform.position).magnitude < range)
        {
            Attack();
        }
    }

    public void CheckForDie()
    {
        if(GetComponent<Health>().healthCount <= 0)
        {
            transform.parent.GetComponent<AttackDroneBay>().droneCount--;
            WorldManager.Instance.SpawnRandomExplosion(transform.position);
            Destroy(gameObject);
        }
    }
}
