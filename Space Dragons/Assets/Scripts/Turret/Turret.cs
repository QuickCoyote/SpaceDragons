using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    public WorldManager.ePoolTag projectileName = WorldManager.ePoolTag.PROJECTILE_TURRET_DEFAULT;
    public float damage = 5;
    public float range = 1.0f;
    public float attackSpeed = 0.25f;
    public float price = 10.0f;
    public GameObject rotateBoi = null;
    public SpriteRenderer spriteRendererBase = null;
    public SpriteRenderer spriteRendererTurret = null;
    public SpriteRenderer spriteRendererWings = null;
    public SpriteRenderer spriteRendererBadge = null;
    public ShipData.eTurretRarity turretRarity = ShipData.eTurretRarity.COMMON;
    public ShipData data = null;    

    private float rarityModifier = 1.0f;
    protected float attackTimer = 0.0f;

    public Vector3 travelDirection = Vector3.zero;

    protected Queue<Enemy> enemies = new Queue<Enemy>();
    protected Health myHealth = null;
    protected WorldManager worldManager;

    public abstract void Attack();

    protected void Awake()
    {
        myHealth = GetComponent<Health>();
        GetComponent<CircleCollider2D>().radius = range;
    }
    protected void Start()
    {
        worldManager = WorldManager.Instance;
        StartCoroutine("CheckForDie");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = null;

        collision.gameObject.TryGetComponent(out enemy);

        if (enemy)
        {
            enemies.Enqueue(enemy);
        }
    }

    private void FixedUpdate()
    {
    }

    public void ApplyRarity()
    {
        damage *= rarityModifier;
        attackSpeed /= rarityModifier;
    }

    public IEnumerator CheckForDie()
    {
        if(myHealth.healthCount <= 0)
        {
            Explosion explosion = WorldManager.Instance.SpawnFromPool(WorldManager.ePoolTag.EXPLOSION, transform.position, transform.rotation).GetComponent<Explosion>();
            if (explosion) explosion.Activate();
            WorldManager.Instance.Ship.RemoveBodyPart(gameObject, false);
        }

        yield return new WaitForFixedUpdate();
    }

    public void Initialize()
    {
        int badgeColor = 0;
        switch (data.type)
        {
            case ShipData.eTurretType.FLAME:
                badgeColor = 0;
                break;
            case ShipData.eTurretType.HEALING:
                badgeColor = 1;
                break;
            case ShipData.eTurretType.LIGHTNING:
                badgeColor = 2;
                break;
            case ShipData.eTurretType.RUSTY:
                badgeColor = 3;
                break;
            case ShipData.eTurretType.ATTACK_DRONE:
                badgeColor = 4;
                break;
        }

        switch (turretRarity)
        {
            case ShipData.eTurretRarity.COMMON:
                rarityModifier = 1.0f;
                spriteRendererBadge.sprite = data.spriteBadgesCommon[badgeColor];
                break;
            case ShipData.eTurretRarity.RARE:
                rarityModifier = 1.5f;
                spriteRendererBadge.sprite = data.spriteBadgesRare[badgeColor];
                break;
            case ShipData.eTurretRarity.EPIC:
                rarityModifier = 2.0f;
                spriteRendererBadge.sprite = data.spriteBadgesEpic[badgeColor];
                break;
        }

        data.rarity = turretRarity;

        ApplyRarity();
    }
}
