using UnityEngine;

public class AnimalBossEnemy : Enemy
{
    [SerializeField] GameObject Turret1 = null;
    [SerializeField] GameObject Turret2 = null;
    [SerializeField] GameObject gunNozzle2 = null;

    new private void Start()
    {
        base.Start();
        shootingSpeedIncrease = shootingSpeed * 0.5f;
    }

    new public void Die()
    {
        for (int i = 0; i < lootnum; i++)
        {
            ItemObject item = worldManager.SpawnFromPool(WorldManager.ePoolTag.ITEM, transform.position, transform.rotation).GetComponent<ItemObject>();
            if (item)
            {
                item.itemData = worldManager.GetRandomItemDataWeighted();
                item.image.sprite = item.itemData.itemImage;
            }
        }
        base.Die();
    }

    public float shootingSpeedIncrease = 2.0f;
    public float shootingTimer2 = 0.5f;
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
        if (hp.healthCount < hp.healthMax * .05f) shootingSpeed = shootingSpeedIncrease;

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

        shootingTimer2 -= Time.deltaTime;
        if (shootingTimer2 < 0.0f)
        {
            shootingTimer2 = shootingSpeed;
            GameObject projectileGO = worldManager.SpawnFromPool(projectileName, gunNozzle2.transform.position, gunNozzle2.transform.rotation);
            projectileGO.GetComponent<Projectile>().Fire(gunNozzle2.transform, attackDamage, gameObject);
        }

        SpawnMinions();

    }
    protected override void Move()
    {
        Vector3 direction = Vector3.zero;
        float angle;
        Quaternion rotation;

        target = Player.transform.position;

        // This is rotating the actual boss

        direction = target - transform.position;
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // This is getting the direction for the First Turret

        direction = target - Turret1.transform.position;

        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        Turret1.transform.rotation = Quaternion.Slerp(Turret1.transform.rotation, rotation, rotationSpeed * 2 * Time.deltaTime);


        // This is getting the direction for the Second Turret

        direction = target - Turret2.transform.position;

        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        Turret2.transform.rotation = Quaternion.Slerp(Turret2.transform.rotation, rotation, rotationSpeed * 2 * Time.deltaTime);
    }
}
