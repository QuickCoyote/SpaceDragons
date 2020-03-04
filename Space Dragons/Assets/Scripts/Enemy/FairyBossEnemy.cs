using System.Collections.Generic;
using UnityEngine;

public class FairyBossEnemy : Enemy
{
    public Transform droneSpawnPos;
    public Transform droneIdlePos1;
    public Transform droneIdlePos2;
    public Transform droneIdlePos3;
    public Transform droneIdlePos4;
    public List<FairyDrone> drones = new List<FairyDrone>();
    public GameObject attackDronePrefab = null;

    public int droneCount = 0;
    int side = 0;

    new private void Start()
    {
        base.Start();
        SpawnDrone();
        SpawnDrone();
        SpawnDrone();
        SpawnDrone();
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
        foreach (FairyDrone go in drones)
        {
            if (go)
            {
                go.Die();
            }
        }
        base.Die();
    }

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
        if (droneCount < 4)
        {
            SpawnDrone();
        }
        AssignParents();
        SpawnMinions();
    }
    protected override void Move()
    {
        target = Player.transform.position;

        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.Translate(transform.up * speed * Time.fixedDeltaTime, Space.World);
    }

    public void AssignParents()
    {
        side = 0;
        foreach (FairyDrone dro in drones)
        {
            if (dro)
            {
                dro.side = side;
                dro.fairySpeed = speed;
            }
            side++;
            if (side > 3) side = 0;
        }
    }

    public void SpawnDrone()
    {
        drones.Add(Instantiate(attackDronePrefab, droneSpawnPos.position, droneSpawnPos.rotation).GetComponent<FairyDrone>());
        droneCount++;

        side = 0;
        foreach (FairyDrone go in drones)
        {
            if (go)
            {
                go.side = side;
                switch (side)
                {
                    case 0:
                        go.idleLocation = droneIdlePos1;
                        break;
                    case 1:
                        go.idleLocation = droneIdlePos2;
                        break;
                    case 2:
                        go.idleLocation = droneIdlePos3;
                        break;
                    case 3:
                        go.idleLocation = droneIdlePos4;
                        break;
                }
                side++;
                if (side > 3) side = 0;
            }
        }
    }
}
