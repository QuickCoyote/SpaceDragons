using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpHole : MonoBehaviour
{
   public float m_lifetime = 3.0f;

    private void Update()
    {
        m_lifetime -= Time.deltaTime;
        if (m_lifetime < 0.0f)
        {
            GetComponent<Animator>().SetTrigger("Die");
        }
    }

    void Destoyself()
    {
        Destroy(gameObject);
    }
}
