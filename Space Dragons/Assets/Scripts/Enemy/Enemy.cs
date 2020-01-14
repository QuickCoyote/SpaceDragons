using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject projectile = null;
    [SerializeField] GameObject gunNozzle = null;
    public float speed = 3f;
    public float rotationSpeed = 5f;
    public float shootingSpeed = 0.5f;
    public float sightDistance = 25.0f;
    public float attackDamage = 5.0f;

    float shootingTimer = 2.0f;
    float targetDistance = 1.0f;
    Rigidbody2D rb;
    Vector3 target;
    Health hp;
    GameObject Player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = GetComponent<Health>();
    }

    void FixedUpdate()
    {
        MoveBasic();

        if (hp.healthCount < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    bool IsPlayerInSight()
    {
        return (Vector3.Distance(Player.transform.position, transform.position) <= sightDistance);
    }

    void MoveBasic()
    {
        target = Player.transform.position;

        if (IsPlayerInSight())
        {
            shootingTimer -= Time.deltaTime;
            if (shootingTimer < 0.0f)
            {
                shootingTimer = shootingSpeed;
                if (projectile)
                {
                    GameObject projectileGO = (Instantiate(projectile, gunNozzle.transform.position, gunNozzle.transform.rotation, null) as GameObject);
                    Projectile p = projectileGO.GetComponent<Projectile>();
                    p.parentobj = gunNozzle;
                    p.damage = attackDamage;
                    p.Fire();
                }
            }
        }

        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        //rb.AddForce(transform.up.normalized * speed, ForceMode2D.Force);
        transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
    }
}
