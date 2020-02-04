using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public GameObject healthbarObj = null;
    Slider healthbar = null;
    public float healthMax = 1.0f;
    public float healthCount = 0.0f;

    Animator an = null;
    AudioSource damageSFX;

    public void Start()
    {
        an = GetComponent<Animator>();
        if (healthbarObj)
        {
            healthbar = healthbarObj.GetComponent<Slider>();
        }
        healthCount = healthMax;
        if (healthbar)
        {
            healthbar.maxValue = healthMax;
        }
    }

    public void ResetHealth()
    {
        healthCount = healthMax;
    }

    public void DealDamage(float dmg)
    {
        healthCount -= dmg;
        if (an) an.SetTrigger("Damage");
        //        AudioManager.Instance.Play("");
    }

    public void Update()
    {
        if (healthbar)
        {
            healthbar.value = healthCount;
        }
        if (healthbar)
        {
            healthbar.transform.rotation = Quaternion.identity;
        }

        if (healthCount > healthMax)
        {
            healthCount = healthMax;
        }
    }
}
