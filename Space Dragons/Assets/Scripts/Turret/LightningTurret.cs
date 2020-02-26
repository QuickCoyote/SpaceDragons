using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightningTurret : Turret
{
    public float chainDistance = 15.0f;
    public float rotationSpeed = 15.0f;

    int enemiesShocked = 1;
    List<Enemy> shockedBois = new List<Enemy>();

    void FixedUpdate()
    {
        if (enemies.Count > 0)
        {
            List<Enemy> enemiesL = enemies.ToList<Enemy>();
            for (int i = 0; i < enemiesL.Count; i++)
            {
                if(enemiesL[i] == null)
                {
                    enemiesL.Remove(enemiesL[i]);
                }
            }
            enemies = new Queue<Enemy>(enemiesL);
            
            if(enemies.Count > 0)
            {
                RotateTurret();
                if ((enemies.Peek().transform.position - transform.position).magnitude > range)
                {
                    Lightning myLightning = null;

                    if (TryGetComponent(out myLightning))
                    {
                        Destroy(myLightning);
                    }
                }
            }
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
            if (Vector3.Angle(rotateBoi.transform.up, direction) < 15)
            {
                enemiesShocked = 1;
                ShockNext(enemy);
            }
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            rotateBoi.gameObject.transform.rotation = Quaternion.Slerp(rotateBoi.gameObject.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            enemies.Dequeue();
        }
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
        Lightning myLightning = null;
        TryGetComponent(out myLightning);

        if (enemiesShocked == 1)
        {
            gameObject.AddComponent<Lightning>().target = enemy.transform.position;
        }
        else
        {
            foreach (Component comp in GetComponents<Component>())
            {
                Lightning lightning = null;
                gameObject.TryGetComponent(out lightning);

                if (lightning)
                {
                    lightning.RemoveLightning();
                }
            }
        }
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
                    if (en != enemy)
                    {
                        shockedBois.Add(en);
                        enemy.gameObject.AddComponent<Lightning>().target = en.transform.position;
                        enemiesShocked++;
                        Debug.Log("Shocked enemies: " + enemiesShocked);
                        ShockNext(en);
                    }
                    en.GetComponent<Health>().healthCount -= damage;
                }
            }
        }
    }
}
