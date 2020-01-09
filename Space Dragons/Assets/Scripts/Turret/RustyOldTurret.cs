using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustyOldTurret : Turret
{
    private new void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (enemies.Count > 0)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        // Find closest enemy... BLAST'EM
        Enemy targetEnemy = enemies.Peek();

        if (targetEnemy)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > attackSpeed)
            {
                // Shoot bullet
                attackTimer = 0;
            }
        }
    }


}
