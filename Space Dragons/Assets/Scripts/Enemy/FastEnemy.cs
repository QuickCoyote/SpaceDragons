﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : Enemy
{
    [SerializeField] GameObject Turret = null;
    public float targetChangeDistance;
    public float targetflydistance;
    protected override void Attack()
    {
        Turret.transform.LookAt(Player.transform.position,-Vector3.forward);
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
        if (Vector3.Distance(transform.position, target) < targetChangeDistance)
        {
            Vector3 newDirection = Player.transform.position - transform.position;
            target = transform.position + (newDirection * targetflydistance);
        }

        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
    }
}