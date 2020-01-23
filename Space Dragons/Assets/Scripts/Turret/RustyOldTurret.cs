using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RustyOldTurret : Turret
{
    [SerializeField] GameObject bullet = null;
    [SerializeField] float rotationSpeed = 45f;
    [SerializeField] float bulletOffsetY = 1f;

    void FixedUpdate()
    {
        if (enemies.Count > 0)
        {
            RotateTurret();
        }
        CheckForDie();
    }

    public void RotateTurret()
    {
        Enemy enemy = enemies.Peek();
        if (enemy)
        {
            Vector3 direction = enemy.transform.position - rotateBoi.gameObject.transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            if (angle < 5 && angle > -5)
            {
                Attack();
            }
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            rotateBoi.gameObject.transform.rotation = Quaternion.Slerp(rotateBoi.gameObject.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            enemies.Dequeue();
        }
    }

    public override void Attack()
    {
        Enemy targetEnemy = enemies.Peek();
        attackTimer += Time.deltaTime;

        if (attackTimer > attackSpeed)
        {
            GameObject projectileGO = (Instantiate(bullet, transform.position + (bulletOffsetY * transform.up), rotateBoi.transform.rotation, transform) as GameObject);
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.parentobj = rotateBoi;
            projectile.Fire();

            attackTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = null;

        collision.gameObject.TryGetComponent(out enemy);

        if (enemy)
        {
            enemies.Enqueue(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = null;

        collision.gameObject.TryGetComponent(out enemy);

        if (enemy)
        {
            enemies.ToList().Remove(enemy);
            enemies = new Queue<Enemy>(enemies);
        }
    }
}
