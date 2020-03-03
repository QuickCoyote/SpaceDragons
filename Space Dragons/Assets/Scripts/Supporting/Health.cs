﻿using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public GameObject healthbarObj = null;
    [SerializeField] public Transform barTransform = null;
    [SerializeField] public SpriteRenderer hbRenderer = null;

    Slider healthbar = null;
    public int objectIndex = 0;

    public float healthMax = 1.0f;
    public float healthCount = 0.0f;
    Animator an = null;

    public void Start()
    {
        an = GetComponent<Animator>();
        if (healthbarObj)
        {
            healthbar = healthbarObj.GetComponent<Slider>();
            if (healthbar)
            {
                healthbar.maxValue = healthMax;
            }
        } else
        {

        }
        
        healthCount = healthMax;


    }

    public void ResetHealth()
    {
        healthCount = healthMax;
    }

    public void DealDamage(float dmg)
    {
        healthCount -= dmg;
        if (an)
        {
            an.SetTrigger("Damage");
        }
    }

    public void FixedUpdate()
    {
        if (healthbarObj)
        {
            if (healthbar)
            {
                healthbar.maxValue = healthMax;
                healthbar.value = healthCount;
                healthbar.transform.rotation = Quaternion.identity;
            }
            else
            {
                healthbar = healthbarObj.GetComponent<Slider>();
            }
        }
        else
        {
            barTransform.localScale = new Vector3(healthCount / healthMax, 1, 1);
            barTransform.localPosition = new Vector3( hbRenderer.bounds.extents.x * (healthCount / healthMax) - hbRenderer.bounds.extents.x, 0, 0);
        }

        if (healthCount > healthMax)
        {
            healthCount = healthMax;
        }
    }

}
