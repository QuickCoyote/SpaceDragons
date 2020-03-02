using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public GameObject parentobj = null;
    public GameObject target = null;
    public Vector3 goDirection = Vector3.zero;

    public string sound = "";

    public float bulletSpeed = 20;
    public float rotateSpeed = 5;
    public float lifetime = 4.0f;
    public float resetLifetime = 4.0f;
    public float damage = 0.0f;

    private void Start()
    {
        lifetime = resetLifetime;
    }

    private void FixedUpdate()
    {
        lifetime -= Time.deltaTime;

        Move();

        if (lifetime < 0.0f)
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
        target = null;
        gameObject.SetActive(false);
    }

    public void Fire(Transform firepoint, float Ownerdmg, GameObject owner, GameObject tar)
    {
        parentobj = owner;
        damage = Ownerdmg;
        target = tar;
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
            goDirection = firepoint.transform.up;
        }
        transform.rotation = firepoint.transform.rotation;
    }

    public void Move()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, target.transform.position - transform.position), rotateSpeed);
        goDirection = transform.up;
        transform.position += (goDirection * bulletSpeed * Time.smoothDeltaTime);
    }
}
