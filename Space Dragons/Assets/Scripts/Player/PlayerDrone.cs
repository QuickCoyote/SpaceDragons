using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrone : MonoBehaviour
{
    public GameObject bullet = null;
    public Transform idleLocation = null;

    public Vector3 targetPosition = Vector3.zero;

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
    [SerializeField] GameObject bulletSpawn = null;

    private void Start()
    {
        myHealth = GetComponent<Health>();
    }

    public void Attack()
    {
        GameObject projectileGO = (Instantiate(bullet, bulletSpawn.transform.position, transform.rotation, null) as GameObject);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.parentobj = gameObject;
        projectile.Fire();
    }

    void FixedUpdate()
    {
        if (idleLocation)
        {
            switch (side)
            {
                case 0:
                    targetPosition = new Vector3(idleLocation.position.x - 3, idleLocation.position.y, idleLocation.position.z);
                    break;
                case 1:
                    targetPosition = new Vector3(idleLocation.position.x, idleLocation.position.y + 3, idleLocation.position.z);
                    break;
                case 2:
                    targetPosition = new Vector3(idleLocation.position.x + 3, idleLocation.position.y, idleLocation.position.z);
                    break;
            }
        }
        Vector3 direction = targetPosition - transform.position;

        if (Vector3.Distance(transform.position, targetPosition) > .5f)
        {
            moveSpeed = WorldManager.Instance.Ship.speed + 1;
        }
        else
        {
            moveSpeed = WorldManager.Instance.Ship.speed;
            direction = targetPosition - transform.position + transform.up;
        }

        if (Vector3.Distance(transform.position, idleLocation.position) > 100.0f)
        {
            myHealth.healthCount = 0.0f;
        }

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);
        CheckForDie();
    }

    public void CheckForDie()
    {
        if (myHealth.healthCount <= 0)
        {
            WorldManager.Instance.PlayerController.guardDrones.Remove(gameObject);
            WorldManager.Instance.PlayerController.guardDroneCount--;
            WorldManager.Instance.SpawnRandomExplosion(transform.position);
            Destroy(gameObject);
        }
    }
}
