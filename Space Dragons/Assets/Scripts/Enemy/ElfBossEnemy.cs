using UnityEngine;

public class ElfBossEnemy : Enemy
{
    [SerializeField] ItemObject itemPrefab = null;
    [SerializeField] GameObject shield = null;

    new private void Start()
    {
        base.Start();
        shield.SetActive(false);
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

    public float teleportTimer = 10.0f;
    public float teleportSpeed = 10.0f;
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
        if (shield && hp.healthCount < hp.healthMax * 0.75f && !shield.activeSelf) shield.SetActive(true);

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
        teleportTimer -= Time.deltaTime;
        if (teleportTimer < 0.0f)
        {
            teleportTimer = teleportSpeed;
            Vector3 newlocation = new Vector3(Random.Range(10.0f, 20.0f), Random.Range(10.0f, 20.0f), 0);
            newlocation.x *= (Random.Range(0, 2) == 0) ? 1 : -1;
            newlocation.y *= (Random.Range(0, 2) == 0) ? 1 : -1;
            WorldManager.Instance.SpawnWarpHole(transform.position);
            animator.SetTrigger("Warp");
            transform.position += newlocation;
            WorldManager.Instance.SpawnWarpHole(transform.position);
        }
        target = Player.transform.position;

        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

}
