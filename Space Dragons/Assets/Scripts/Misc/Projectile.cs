using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletSpeed = 1000;
    public float lifetime = 1.0f;
    public float damage = 0.0f;
    public GameObject parentobj = null;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if( lifetime < 0.0f)
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
                Debug.Log("Damaged " + collision.gameObject.name);
                collidedHP.DealDamage(damage);
                Destroy(gameObject);

            }
        }
    }

    public void Fire()
    {
        GetComponent<Rigidbody2D>().velocity = (parentobj.transform.up * bulletSpeed * Time.smoothDeltaTime);
    }
}
