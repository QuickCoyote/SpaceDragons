using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcBossEnemy : Enemy
{
    [SerializeField] ItemObject itemPrefab = null;
    [SerializeField] MapTargets maptarget = null;

    new private void Start()
    {
        base.Start();
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

    public float lootnum = 5.0f;
    public float flameDamage = 5.0f;
    public float fireWaveTimer = 0.5f;
    public float fireWaveReset = 0.5f;
    public float firewaveRotateSpeed = 15.0f;
    [SerializeField] Projectile FireProjectile = null;
    [SerializeField] Transform FireNozzle = null;
    [SerializeField] GameObject FireGun = null;

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
        if (IsPlayerInSight())
        {
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0.0f)
            {
                shootingTimer = shootingSpeed;
                if (projectile)
                {
                    GameObject projectileGO = (Instantiate(projectile, gunNozzle.transform.position, gunNozzle.transform.rotation, null) as GameObject);

                    HomingMissile p = projectileGO.GetComponent<HomingMissile>();
                    p.Fire(gunNozzle.transform, attackDamage, gameObject, WorldManager.Instance.Head);
                }
            }
        }

        fireWaveTimer -= Time.deltaTime;
        if (fireWaveTimer <= 0.0f)
        {
            fireWaveTimer = fireWaveReset;
            if (FireProjectile)
            {
                FireNozzle.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-30, 30));
                Projectile p  = Instantiate(FireProjectile, FireNozzle.position, FireNozzle.rotation, null);
                p.Fire(FireNozzle, flameDamage, gameObject);

            }
        }

        FireGun.transform.Rotate(Vector3.forward, firewaveRotateSpeed * Time.deltaTime);
        FireNozzle.localRotation = Quaternion.identity;
    }
    protected override void Move()
    {
        target = Player.transform.position;

        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
