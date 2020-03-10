using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlameTurret : Turret
{
    [SerializeField] float burnDamage = 10.0f;
    [SerializeField] float rotationSpeed = 45f;

    public Queue<Enemy> enemiesToBurn = new Queue<Enemy>();
    public int flameAttackangle;
    public int bulletOffsetY;
    public float flameLifeSpan;
    public float flameSpeed;

    public float acceptableDistance = 35f;

    void FixedUpdate()
    {
        if (enemiesToBurn.Count > 0)
        {
            foreach (Enemy enemy in enemiesToBurn)
            {
                if(enemy)
                {
                    enemy.GetComponent<Health>().healthCount -= burnDamage * Time.deltaTime;
                }
                else
                {
                    enemiesToBurn.ToList().Remove(enemy);
                    enemiesToBurn = new Queue<Enemy>(enemiesToBurn);
                }
            }
        }
        if (enemies.Count > 0)
        {
            RotateTurret();
        }
        CheckForDie();
    }

    public void RotateTurret()
    {
        Enemy enemy = null;

        enemy = enemies.Peek();

        if (enemy)
        {
            Vector3 direction = enemy.transform.position - rotateBoi.gameObject.transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            if (Vector3.Angle(rotateBoi.transform.up, direction) < 15)
            {
                if(direction.magnitude < acceptableDistance)
                {
                    Attack();
                }
            }
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            rotateBoi.gameObject.transform.rotation = Quaternion.Slerp(rotateBoi.gameObject.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            enemies.Dequeue();
        }
    }

    public void Burn()
    {
        Quaternion rotAngle = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-flameAttackangle, flameAttackangle));
        Vector3 projectileDirection = rotAngle * rotateBoi.transform.up;

        GameObject projectileGO = worldManager.SpawnFromPool(projectileName, transform.position + (bulletOffsetY * rotateBoi.transform.up), Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.parentobj = gameObject;
        projectile.damage = 0.25f;
        projectile.goDirection = projectileDirection;
        projectile.lifetime = flameLifeSpan;
        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
        projectile.bulletSpeed = flameSpeed;
        projectile.sound = "null";
        projectile.Fire();
    }

    public override void Attack()
    {
        Burn();
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
