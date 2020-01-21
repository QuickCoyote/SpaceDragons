using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float healthMax = 1.0f;
    public float healthCount = 0.0f;

    Animator an = null;
    AudioSource damageSFX;

    public void Start()
    {
        an = GetComponent<Animator>();
        damageSFX = GetComponent<AudioSource>();
        healthCount = healthMax;
    }

    public void ResetHealth()
    {
        healthCount = healthMax;
    }

    public void DealDamage(float dmg)
    {
        healthCount -= dmg;
        if (an) an.SetTrigger("Damage");
        if (damageSFX) damageSFX.Play();

    }
}
