using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float sightDistance = 25.0f;
    [SerializeField] GameObject projectile = null;
    public float speed = 3f;
    public float rotationSpeed = 8f;
    public float shootingSpeed = 2.0f;

    float wanderTimer = 0.0f;
    float shootingTimer = 2.0f;
    Rigidbody2D rb;
    Vector3 target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (IsPlayerInSight())
        {
            target = GameObject.FindGameObjectWithTag("Player").transform.position;
            shootingTimer -= Time.deltaTime;
            if (shootingTimer < 0.0f)
            {
                shootingTimer = shootingSpeed;
                if (projectile) Instantiate(projectile);
            }
            Debug.Log("PlayerSighted");

        }
        else
        {
            target = (transform.position + new Vector3(Random.Range(-sightDistance, +sightDistance), Random.Range(-sightDistance, +sightDistance), 0));
            wanderTimer = Random.Range(2.0f, 10.0f);
        }
        Debug.Log(target);
    }

    void Update()
    {
        MoveBasic();
    }

    bool IsPlayerInSight()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
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
            target = GameObject.FindGameObjectWithTag("Player").transform.position;
            shootingTimer -= Time.deltaTime;
            if (shootingTimer < 0.0f)
            {
                shootingTimer = shootingSpeed;
                if (projectile) Instantiate(projectile);
            }
            Debug.Log("PlayerSighted");
                    Debug.Log("New Target:" + target);


        } else
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer < 0.0f)
            {
                target = (transform.position + new Vector3(Random.Range(-sightDistance,+sightDistance), Random.Range(-sightDistance, +sightDistance),0));
                Debug.Log(target);

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
