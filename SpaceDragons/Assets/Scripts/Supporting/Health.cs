using UnityEngine;
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

    public void Start()
    {
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
            barTransform.localPosition = new Vector3((healthCount / healthMax) * 3.24f - 3.24f, 0, 0);
            barTransform.parent.rotation = Quaternion.identity;
        }

        if (healthCount > healthMax)
        {
            healthCount = healthMax;
        }
    }

}
