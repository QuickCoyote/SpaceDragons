using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrone : MonoBehaviour
{
    public GameObject bullet = null;
    public Transform idleLocation;

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
        Debug.Log("GuardDrone" + side + " - TargetLocation: " + targetPosition);

        Debug.DrawLine(transform.position, targetPosition, Color.red);

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
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) > .5f)
        {
            moveSpeed = WorldManager.Instance.Ship.speed + 1;
            transform.Translate(transform.up * moveSpeed * Time.fixedDeltaTime, Space.World);
        }
        else
        {
            moveSpeed = WorldManager.Instance.Ship.speed;
            Vector3 direction2 = targetPosition - transform.position + transform.up;
            float angle2 = Mathf.Atan2(direction2.x, direction2.y) * Mathf.Rad2Deg;
            Quaternion rotation2 = Quaternion.AngleAxis(-angle2, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation2, rotationSpeed * Time.deltaTime);
            transform.Translate(transform.up * moveSpeed * Time.fixedDeltaTime, Space.World);
        }
        CheckForDie();
    }

    public void CheckForDie()
    {
        if (GetComponent<Health>().healthCount <= 0)
        {
            WorldManager.Instance.PlayerController.guardDrones.Remove(gameObject);
            WorldManager.Instance.PlayerController.guardDroneCount--;
            WorldManager.Instance.SpawnRandomExplosion(transform.position);
            Destroy(gameObject);
        }
    }
}
