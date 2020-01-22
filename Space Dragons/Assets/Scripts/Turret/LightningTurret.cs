using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTurret : Turret
{
    public float chainDistance = 15.0f;

    List<Enemy> shockedBois = new List<Enemy>();

    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackSpeed)
        {
            Attack();
        }
        CheckForDie();
    }

    public override void Attack()
    {
        if (enemies.Count > 0)
        {
            ShockNext(enemies.Peek());
        }
    }

    public void ShockNext(Enemy enemy)
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(enemy.transform.position, chainDistance);
        foreach (Collider2D col in enemyColliders)
        {
            Enemy en = null;
            col.TryGetComponent(out en);
            if (en != null)
            {
                if (shockedBois.Contains(en))
                {
                    return;
                }
                else
                {
                    shockedBois.Add(en);
                    en.GetComponent<Health>().healthCount -= damage;
                    ShockNext(en);
                }
            }
        }
    }
}
