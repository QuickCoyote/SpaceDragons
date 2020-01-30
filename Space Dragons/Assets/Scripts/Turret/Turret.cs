using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
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

    protected Queue<Enemy> enemies = new Queue<Enemy>();
    public abstract void Attack();

    protected void Awake()
    {
        GetComponent<CircleCollider2D>().radius = range;

        switch (turretRarity)
        {
            case ShipData.eTurretRarity.COMMON:
                rarityModifier = 1.0f;
                break;
            case ShipData.eTurretRarity.RARE:
                rarityModifier = 1.5f;
                break;
            case ShipData.eTurretRarity.EPIC:
                rarityModifier = 2.0f;
                break;
        }

        ApplyRarity();
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

    public void ApplyRarity()
    {
        damage *= rarityModifier;
        range *= rarityModifier;
        attackSpeed *= rarityModifier;
    }

    public void CheckForDie()
    {
        if(GetComponent<Health>().healthCount <= 0)
        {
            FindObjectOfType<WorldManager>().SpawnRandomExplosion(transform.position);
            FindObjectOfType<Ship>().RemoveBodyPart(gameObject, false);
        }
    }
}
