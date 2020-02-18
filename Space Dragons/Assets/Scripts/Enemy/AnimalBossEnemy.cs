using UnityEngine;

public class AnimalBossEnemy : Enemy
{
    [SerializeField] GameObject Turret1 = null;
    [SerializeField] GameObject Turret2 = null;
    [SerializeField] GameObject gunNozzle2 = null;
    [SerializeField] ItemObject itemPrefab = null;
    [SerializeField] MapTargets maptarget = null;

    new private void Start()
    {
        base.Start();
        shootingSpeedIncrease = shootingSpeed / 2;
    }

    new public void Die()
    {
        for (int i = 0; i < lootnum; i++)
        {
            ItemObject g = Instantiate(itemPrefab, transform.position, transform.rotation, null); // drops item in world space
            g.itemData = WorldManager.Instance.GetRandomItemDataWeighted();
            g.image.sprite = g.itemData.itemImage;
        }
        base.Die();
    }

    public float shootingSpeedIncrease = 2.0f;
    public float shootingTimer2 = 0.5f;
    public bool enraged = false;
    public float lootnum = 5.0f;


    [SerializeField] GameObject Minion = null;
    [SerializeField] Transform SpawnPoint = null;
    public float minionTimer = 60.0f;
    public float minionTimerReset = 60.0f;
    public void SpawnMinions()
    {
        minionTimer -= Time.deltaTime;
        if (minionTimer < 0.0f)
        {
            minionTimer = minionTimerReset;
            Instantiate(Minion, SpawnPoint.position, SpawnPoint.rotation, null);
            EnemyWaveManager.Instance.aliveEnemies++;
        }
    }

    protected override void Attack()
    {
        if (hp.healthCount < hp.healthMax/2.0f) shootingSpeed = shootingSpeedIncrease;
        
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

            shootingTimer2 -= Time.deltaTime;
            if (shootingTimer2 < 0.0f)
            {
                shootingTimer2 = shootingSpeed;
                if (projectile)
                {
                    GameObject projectileGO = (Instantiate(projectile, gunNozzle2.transform.position, gunNozzle2.transform.rotation, null) as GameObject);
                    Projectile p = projectileGO.GetComponent<Projectile>();
                    p.Fire(gunNozzle2.transform, attackDamage, gameObject);

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

        Vector3 TurretDirection1 = target - Turret1.transform.position;
        Vector3 TurretDirection2 = target - Turret2.transform.position;
        
        float angle1 = Mathf.Atan2(TurretDirection1.x, TurretDirection1.y) * Mathf.Rad2Deg;
        Quaternion rotation1 = Quaternion.AngleAxis(-angle1, Vector3.forward);
        Turret1.transform.rotation = Quaternion.Slerp(Turret1.transform.rotation, rotation1, rotationSpeed * 2 * Time.deltaTime);

        float angle2 = Mathf.Atan2(TurretDirection2.x, TurretDirection2.y) * Mathf.Rad2Deg;
        Quaternion rotation2 = Quaternion.AngleAxis(-angle2, Vector3.forward);
        Turret2.transform.rotation = Quaternion.Slerp(Turret2.transform.rotation, rotation2, rotationSpeed * 2 * Time.deltaTime);
    }
}
