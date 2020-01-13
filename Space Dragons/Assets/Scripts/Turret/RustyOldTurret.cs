using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RustyOldTurret : Turret
{
    [SerializeField] GameObject turretSprite = null;

    [SerializeField] GameObject bullet = null;
    [SerializeField] float rotationSpeed = 45f;
    [SerializeField] float bulletOffsetY = 1f;

    private new void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (enemies.Count > 0)
        {
            RotateTurret();
        }
    }

    public void RotateTurret()
    {
        Enemy enemy = enemies.Peek();
        if (enemy)
        {
            Vector3 direction = enemy.transform.position - turretSprite.transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            if (angle < 5)
            {
                Attack();
            }
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            turretSprite.transform.rotation = Quaternion.Slerp(turretSprite.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    public override void Attack()
    {
        // Find closest enemy... BLAST'EM
        Enemy targetEnemy = enemies.Peek();

        if (targetEnemy)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > attackSpeed)
            {
                GameObject projectileGO = (Instantiate(bullet, transform.position + (bulletOffsetY * transform.up), Quaternion.identity, transform) as GameObject);
                Projectile projectile = projectileGO.GetComponent<Projectile>();
                projectile.parent = gameObject;
                projectile.GetComponent<Rigidbody2D>().AddForce(projectile.parent.transform.up * projectile.bulletSpeed * Time.smoothDeltaTime);

                attackTimer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = null;

        collision.gameObject.TryGetComponent(out enemy);

        if (enemy)
        {
            enemies.Enqueue(enemy);
            Debug.Log(name + ": ADDED AN ENEMY");
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
