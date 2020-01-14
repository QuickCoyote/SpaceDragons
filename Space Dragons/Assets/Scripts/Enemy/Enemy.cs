using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject projectile = null;
    [SerializeField] protected GameObject gunNozzle = null;
    [SerializeField] public float speed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public float shootingSpeed;
    [SerializeField] public float sightDistance;
    [SerializeField] public float attackDamage;
    [SerializeField] public EnemyDifficutlty difficulty;

    public enum EnemyDifficutlty
    {
        EASY,
        MEDIUM,
        HARD,
        BOSS,
        NONE
    }
    
    protected float shootingTimer;
    protected Rigidbody2D rb;
    protected Vector3 target;
    protected Health hp;
    protected GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        hp = GetComponent<Health>();
    }

    void FixedUpdate()
    {
        Attack();
        Move();

        if (hp.healthCount < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void Move();
    protected abstract void Attack();

    public bool IsPlayerInSight()
    {
        return (Vector3.Distance(Player.transform.position, transform.position) <= sightDistance);
    }

    
}
