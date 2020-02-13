using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPWave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = null;

        collision.gameObject.TryGetComponent(out health);

        if(health)
        {
            health.healthCount -= HumanBossEnemy.EMPWaveDamage;
        }
    }
}
