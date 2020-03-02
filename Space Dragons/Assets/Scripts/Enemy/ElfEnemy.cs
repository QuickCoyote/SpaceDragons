using UnityEngine;

public class ElfEnemy : Enemy
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

                Projectile p = projectileGO.GetComponent<Projectile>();
                p.Fire(gunNozzle.transform, attackDamage, gameObject);
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

        Vector3 dirFromAtoB = (transform.position - target).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, Player.transform.up);

        if (dotProd > 0.9)
        {
            transform.Translate(transform.right * speed * Time.smoothDeltaTime, Space.World);
        }

        if (Vector3.Distance(transform.position, target) > .25f) //Stop moving if player gets too close.
        {
            transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
        }
    }
}
