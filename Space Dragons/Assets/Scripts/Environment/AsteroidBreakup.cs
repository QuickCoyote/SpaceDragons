using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBreakup : MonoBehaviour
{
    public ParticleSystem particles;
    public float m_lifetime = 1.0f;
    public float m_lifetimeReset = 1.0f;

    public void Activate()
    {
        AudioManager.Instance.Play("Explosion01");
        if (particles)
        {
            particles.Clear();
            particles.Play();
        }
    }

    private void Update()
    {
        m_lifetime -= Time.deltaTime;
        if (m_lifetime < 0.0f)
        {
            gameObject.SetActive(false);
        }
    }

}
