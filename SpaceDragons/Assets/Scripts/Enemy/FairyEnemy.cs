using UnityEngine;

public class FairyEnemy : Enemy
{
    [SerializeField] GameObject Turret = null;
    public float targetChangeDistance;
    public float targetflydistance;
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
        if (Vector3.Distance(transform.position, target) < targetChangeDistance)
        {
            direction = Player.transform.position - transform.position;
            target = transform.position + (direction * targetflydistance);
        }

        direction = target - transform.position;
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) > .25f) //Stop moving if player gets too close.
        {
            transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
        }

        // This is setting the turret's rotation

        direction = Player.transform.position - transform.position;
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

        Turret.transform.rotation = Quaternion.Slerp(Turret.transform.rotation, rotation, rotationSpeed * 2 * Time.deltaTime);
    }
}
