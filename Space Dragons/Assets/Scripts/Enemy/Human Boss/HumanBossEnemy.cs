using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBossEnemy : Enemy
{
    [SerializeField] ItemObject itemPrefab = null;
    [SerializeField] Material lightningMat = null;

    private void Start()
    {
        base.Start();
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

    [Header("Laser Beam Information")]
    public float laserChargeTimer = 0.0f;
    public float LaserChargeTime = 0.0f;
    public float laserFiringTimer = 0.0f;
    public float laserFiringMaxTime = 0.0f;
    public float LaserFireDistance = 20.0f;
    public static float laserBeamDamage = 10f;

    public Material LaserBeamMaterial = null;
    public GameObject LaserBeamHolder = null;

    [Header("EMP Wave Information")]
    public float EMPWaveTimer = 0.0f;
    public float TimeBetweenEMPWaves = 25;
    public float EMPWaveBaseRadius = 2.0f;
    public float EMPWaveGrowthRate = 2.0f;
    public float EMPWaveGrowthTimer = 0;
    public float EMPWaveGrowthMaxTime = 5;
    public static float EMPWaveDamage = 20f;

    public CircleCollider2D OuterRing = null;



    protected override void Attack()
    {

        // This will be the Laser Beam code
        // Need A laser beam line Renderer to draw to max range with a box collider on it that damages enemies inside of it all the time
        if (IsPlayerInSight())
        {
            laserChargeTimer += Time.deltaTime;
            if (laserChargeTimer > LaserChargeTime)
            {
                laserFiringTimer += Time.deltaTime;
                LaserBeamHolder.SetActive(true);

                if (laserFiringTimer > laserFiringMaxTime)
                {
                    laserChargeTimer = 0;
                    laserFiringTimer = 0;
                    LaserBeamHolder.SetActive(false);
                }
            }
        }

        // This is the EMPWave Code;
        EMPWaveTimer += Time.deltaTime;
        if (EMPWaveTimer > TimeBetweenEMPWaves)
        {
            OuterRing.gameObject.SetActive(true);
            EMPWaveGrowthTimer += Time.deltaTime;
            OuterRing.gameObject.DrawCircle(EMPWaveBaseRadius + EMPWaveGrowthTimer * EMPWaveGrowthRate, 2, 0.25f, lightningMat);
            OuterRing.radius = EMPWaveBaseRadius + EMPWaveGrowthTimer * EMPWaveGrowthRate;
            LineRenderer lr = OuterRing.GetComponent<LineRenderer>();
            lr.alignment = LineAlignment.TransformZ;
            lr.receiveShadows = false;
            if (EMPWaveGrowthTimer > EMPWaveGrowthMaxTime)
            {
                EMPWaveTimer = 0;
                EMPWaveGrowthTimer = 0;
                OuterRing.gameObject.SetActive(false);
                Destroy(OuterRing.GetComponent<LineRenderer>());
            }
        }

        SpawnMinions();

    }
    protected override void Move()
    {
        target = Player.transform.position;

        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
    private void OnDestroy()
    {
        for (int i = 0; i < lootnum; i++)
        {
            ItemObject g = Instantiate(itemPrefab, transform.position, transform.rotation, null); // drops item in world space
            g.itemData = WorldManager.Instance.GetRandomItemDataWeighted();
            g.image.sprite = g.itemData.itemImage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = null;

        collision.gameObject.TryGetComponent(out health);

        if (health)
        {
            health.healthCount -= EMPWaveDamage;
        }
    }
}

