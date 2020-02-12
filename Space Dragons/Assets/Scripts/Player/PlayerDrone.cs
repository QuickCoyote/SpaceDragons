using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrone : MonoBehaviour
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
    public int side;

    protected float attackTimer = 0f;

    [SerializeField] GameObject bulletSpawn = null;

    public void Attack()
    {
        GameObject projectileGO = (Instantiate(bullet, bulletSpawn.transform.position, transform.rotation, transform) as GameObject);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.parentobj = gameObject;
        projectile.Fire();
    }

    void FixedUpdate()
    {
        moveSpeed = 3f;
        
        switch(side)
        {
            case 0:
                targetPosition = WorldManager.Instance.PlayerController.DronePosition1.transform.position;
                break;
            case 1:
                targetPosition = WorldManager.Instance.PlayerController.DronePosition2.transform.position;
                break;
            case 2:
                targetPosition = WorldManager.Instance.PlayerController.DronePosition3.transform.position;
                break;
        }

        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) > .25f)
        {
            transform.Translate(transform.up * moveSpeed * Time.smoothDeltaTime, Space.World);
        }

        transform.up = WorldManager.Instance.Ship.head.transform.up;

        CheckForDie();
    }

    public void CheckForDie()
    {
        if (GetComponent<Health>().healthCount <= 0)
        {
            transform.parent.GetComponent<AttackDroneBay>().droneCount--;
            WorldManager.Instance.SpawnRandomExplosion(transform.position);
            Destroy(gameObject);
        }
    }
}
