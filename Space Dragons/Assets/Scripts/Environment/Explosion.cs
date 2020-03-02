using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Animator anim;
    private void Start()
    {
       if(!anim) anim = GetComponent<Animator>();
    }

    public void Activate()
    {
        AudioManager.Instance.Play("Explosion01");
        if (!anim) anim = GetComponent<Animator>();
        anim.ResetTrigger("Explosion0");
        anim.ResetTrigger("Explosion1");
        anim.ResetTrigger("Explosion2");
        anim.ResetTrigger("Explosion3");
        anim.SetTrigger("Explosion" + Random.Range(0,4));
    }

    public float damage = 1.0f;
    void Destoyself()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health hp;
        if (collision.TryGetComponent(out hp))
        {
            hp.DealDamage(damage);
        }
    }
}
