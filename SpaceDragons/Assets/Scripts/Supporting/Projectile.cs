using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletSpeed;
    public float lifetime = 2f;
    public float resetLifetime = 2f;
    public float damage = 0.0f;
    public GameObject parentobj = null;
    public Vector3 goDirection = Vector3.zero;
    public string sound = "";

    private void Start()
    {
        lifetime = resetLifetime;
    }

    private void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        
        Move();

        if(lifetime < 0.0f)
        {
            ResetForPool();
        }
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != parentobj) // make sure its not hitting itself
        {
            Health collidedHP = collision.gameObject.GetComponent<Health>();
            if (collidedHP)
            {
                collidedHP.DealDamage(damage);
                ResetForPool();
            }
        }
    }

    public void ResetForPool()
    {
        lifetime = resetLifetime;
        parentobj = null;
        goDirection = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void Fire(Transform firepoint, float Ownerdmg, GameObject owner)
    {
        parentobj = owner;
        damage = Ownerdmg;
        if (sound != "null")
        {
            if (sound == "" || sound == null)
            {
                AudioManager.Instance.Play("Fire01");
            }
            else
            {
                AudioManager.Instance.Play(sound);
            }
        }
        goDirection = firepoint.transform.up;
        transform.rotation = firepoint.transform.rotation;
    }

    public void Fire()
    {
        if (sound != "null")
        {
            if (sound == "" || sound == null)
            {
                AudioManager.Instance.Play("Fire01");
            }
            else
            {
                AudioManager.Instance.Play(sound);
            }
        }
        if (goDirection == Vector3.zero)
        {
            goDirection = parentobj.transform.up;
        }
        transform.rotation = parentobj.transform.rotation;
    }

    public void Move()
    {
        transform.position += (goDirection * bulletSpeed * Time.smoothDeltaTime);
    }
}
