using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTurret : Turret
{
    [SerializeField] float burnDamage = 10.0f;

    Queue<Enemy> enemiesToBurn = new Queue<Enemy>();
    float rotationSpeed = 15.0f;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (enemies.Count > 0)
        {
            RotateTurret();
        }
        CheckForDie();
    }

    public void RotateTurret()
    {
        Enemy enemy = enemies.Peek();
        if (enemy)
        {
            Vector3 direction = enemy.transform.position - rotateBoi.gameObject.transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            if (angle < 15 && angle > -15)
            {
                Attack();
            }
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            rotateBoi.gameObject.transform.rotation = Quaternion.Slerp(rotateBoi.gameObject.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            enemies.Dequeue();
        }
    }

    public void Burn()
    {
        // Need to create a particle system on me, then they need to have collisions, and they need to tell me to enqueue anything with health into enemies to burn.

        foreach(Enemy enemy in enemiesToBurn)
        {
            enemy.GetComponent<Health>().healthCount -= burnDamage;
        }
    }

    public override void Attack()
    {
        Burn();
    }
}
