using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    public float damage = 5;
    public float range = 1.0f;
    public float attackSpeed = 0.25f;
    public float price = 10.0f;

    protected float attackTimer = 0.0f;

    protected Queue<Enemy> enemies = new Queue<Enemy>();
    public abstract void Attack();

    protected void Awake()
    {
        GetComponent<CircleCollider2D>().radius = range;
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
}
