using UnityEngine;

public class HeavyEnemy : Enemy
{
    public float stoppingDistance;

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
                    p.Fire(gunNozzle.transform, attackDamage, gameObject);
                }
            }
        }
    }

    protected override void Move()
    {
        target = Player.transform.position;

        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) > stoppingDistance) //Stop moving if player gets too close.
        {
            transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
        }
    }

}
