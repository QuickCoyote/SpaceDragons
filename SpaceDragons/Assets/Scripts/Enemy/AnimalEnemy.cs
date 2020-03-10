using UnityEngine;

public class AnimalEnemy : Enemy
{
    protected override void Attack()
    {
        if (IsPlayerInSight())
        {
            shootingTimer -= Time.deltaTime;
            if (shootingTimer < 0.0f)
            {
                shootingTimer = shootingSpeed;
                GameObject projectileGO = worldManager.SpawnFromPool(projectileName, gunNozzle.transform.position, gunNozzle.transform.rotation);
                projectileGO.GetComponent<Projectile>().Fire(gunNozzle.transform, attackDamage, gameObject);
            }
        }
    }

    protected override void Move()
    {
        for (int i = 0; i < worldManager.Ship.bodyPartObjects.Count; i++)
        {
            if (worldManager.Ship.bodyPartObjects[i])
            {
                target = worldManager.Ship.bodyPartObjects[i].transform.position;
            }
        }
        direction = target - transform.position;
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) > .25f) //Stop moving if player gets too close.
        {
            transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
        }
    }
}
