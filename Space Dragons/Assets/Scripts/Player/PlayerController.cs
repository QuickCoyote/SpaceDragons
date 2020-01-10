using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float attackSpeed = 0.25f;
    public float attackTimer = 0.0f;

    public int money = 100;

    Inventory inventory = null;

    [SerializeField] GameObject head = null;
    [SerializeField] GameObject headBullet = null;

    [SerializeField] float bulletOffsetY = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        //shoot from head
        attackTimer += Time.deltaTime;

        if(attackTimer > attackSpeed)
        {
            GameObject projectileGO = (Instantiate(headBullet, head.transform.position + (bulletOffsetY * head.transform.up), Quaternion.identity, transform) as GameObject);
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.parent = head;
            projectile.GetComponent<Rigidbody2D>().AddForce(projectile.parent.transform.up * projectile.bulletSpeed * Time.smoothDeltaTime);

            attackTimer = 0;
        }
    }
}
