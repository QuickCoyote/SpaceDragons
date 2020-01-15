using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletSpeed = 1000;
    public float lifetime = 1.0f;
    public float damage = 0.0f;
    public GameObject parentobj = null;
    public Vector3 goDirection = Vector3.zero;
    public Rigidbody2D rb = null;

    private void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        
        Move();

        if(lifetime < 0.0f)
        {
            Destroy(gameObject);
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
                Destroy(gameObject);
            }
        }
    }

    public void Fire()
    {
        goDirection = parentobj.transform.up;
        transform.rotation = parentobj.transform.rotation;
    }

    public void Move()
    {

        transform.position += (goDirection * bulletSpeed * Time.smoothDeltaTime);
    }
}
