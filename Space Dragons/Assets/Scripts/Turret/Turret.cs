using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    public float damage = 5;
    public float range = 1.0f;
    public float attackSpeed = 0.25f;
    public float price = 10.0f;
    public SpriteRenderer spriteRenderer = null;
    public ShipData.eTurretRarity turretRarity = ShipData.eTurretRarity.COMMON;

    private float rarityModifier = 1.0f;
    protected float attackTimer = 0.0f;

    protected Queue<Enemy> enemies = new Queue<Enemy>();
    public abstract void Attack();

    protected void Awake()
    {
        GetComponent<CircleCollider2D>().radius = range;
        spriteRenderer = GetComponent<SpriteRenderer>();

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

    public void Die()   
    {
        if(GetComponent<Health>().healthCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
