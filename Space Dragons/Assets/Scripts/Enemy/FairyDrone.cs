using UnityEngine;

public class FairyDrone : Enemy
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float fairySpeed;
    public Transform idleLocation = null;
    public int side;

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

        target = WorldManager.Instance.Head.transform.position;
        if (IsPlayerInSight())
        {
            Vector3 direction = target - transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) > .25f) //Stop moving if player gets too close.
            {
                transform.Translate(transform.up * speed * Time.fixedDeltaTime, Space.World);
            }
            speed = moveSpeed;
        }
        else
        {

            if (idleLocation)
            {
                target = idleLocation.position;
            }
            if (Vector3.Distance(transform.position, idleLocation.position) > 50.0f)
            {
                Die();
            }

            if (Vector3.Distance(transform.position, target) > 1.0f)
            {
                Vector3 direction = target - transform.position;
                float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                speed = fairySpeed + 3f;
                transform.Translate(transform.up * speed * Time.fixedDeltaTime, Space.World);
            }
            else
            {
                speed = fairySpeed + .05f;
                transform.rotation = Quaternion.Slerp(transform.rotation, idleLocation.rotation, rotationSpeed/10 * Time.deltaTime);
                transform.Translate(transform.up * speed * Time.fixedDeltaTime, Space.World);
            }
        }
    }
}
