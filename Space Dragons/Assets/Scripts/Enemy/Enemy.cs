using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject projectile = null;
    [SerializeField] GameObject gunNozzle = null;
    public float speed = 0.5f;
    public float rotationSpeed = 2f;
    public float shootingSpeed = 0.5f;
    public float sightDistance = 10.0f;
    public float attackDamage = 5.0f;

    float wanderTimer = 0.0f;
    float shootingTimer = 2.0f;
    Rigidbody2D rb;
    Vector3 target;
    Health hp;
    GameObject Player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = GetComponent<Health>();
        Player = GameObject.FindGameObjectWithTag("Player");
        target = (transform.position + new Vector3(Random.Range(-sightDistance, +sightDistance), Random.Range(-sightDistance, +sightDistance), 0));
    }

    void Update()
    {
        MoveBasic();

        if (hp.healthCount < 0.0f)
        {
            Destroy(gameObject);
            Debug.Log(this.name + " Died");
        }
    }

    bool IsPlayerInSight()
    {
        if (Player)
        {
            return (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) <= sightDistance);
        } else
        {
            return false;
        }
    }

    void MoveBasic()
    {
        if (IsPlayerInSight())
        {
            target = Player.transform.position;
            shootingTimer -= Time.deltaTime;
            if (shootingTimer < 0.0f)
            {
                shootingTimer = shootingSpeed;
                if (projectile)
                {
                    GameObject projectileGO = (Instantiate(projectile, gunNozzle.transform) as GameObject);
                    Projectile p = projectileGO.GetComponent<Projectile>();
                    p.parent = gunNozzle;
                    p.damage = attackDamage;
                    p.Fire();
                }
            }
        } else
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer < 0.0f)
            {
                target = (transform.position + new Vector3(Random.Range(-sightDistance,+sightDistance), Random.Range(-sightDistance, +sightDistance),0));
                wanderTimer = Random.Range(2.0f, 10.0f);
            }
        }
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        //transform.Translate(transform.up * speed * Time.smoothDeltaTime, Space.World);
        rb.AddForce(transform.up.normalized * speed, ForceMode2D.Force);
    }
}
