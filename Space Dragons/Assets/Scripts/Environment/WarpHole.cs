using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpHole : MonoBehaviour
{
   public float m_lifetime = 3.0f;
   public float m_lifetimeReset = 3.0f;

    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Activate()
    {
        m_lifetime = m_lifetimeReset;
    }

    private void Update()
    {
        m_lifetime -= Time.deltaTime;
        if (m_lifetime < 0.0f)
        {
            anim.SetTrigger("Die");
        }
    }

    void Destoyself()
    {
        gameObject.SetActive(false);
    }
}
