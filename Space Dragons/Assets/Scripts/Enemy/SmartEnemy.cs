using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartEnemy : Enemy
{
    Ship playerShip;
    protected override void Attack()
    {
        if (IsPlayerInSight())
        {
            shootingTimer -= Time.deltaTime;
            if (shootingTimer < 0.0f)
            {
                shootingTimer = shootingSpeed;
                if (projectile)
                {
                    GameObject projectileGO = (Instantiate(projectile, gunNozzle.transform.position, gunNozzle.transform.rotation, null) as GameObject);
                    Projectile p = projectileGO.GetComponent<Projectile>();
                    p.parentobj = gunNozzle;
                    p.damage = attackDamage;
                    p.Fire();
                }
            }
        }
    }

    protected override void Move()
    {
        if (!playerShip) playerShip = Player.GetComponentInParent<Ship>();

        target = playerShip.bodyPartObjects.ElementAt(playerShip.bodyPartObjects.Count - 1).transform.position;
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
    }
}
