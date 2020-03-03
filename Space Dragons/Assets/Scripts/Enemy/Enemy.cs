using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject gunNozzle = null;
    [SerializeField] public string projectileName = "";
    [SerializeField] public float speed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public float shootingSpeed;
    [SerializeField] public float sightDistance;
    [SerializeField] public float attackDamage;
    [SerializeField] public EnemyDifficutlty difficulty;
    [SerializeField] public Animator animator;
    [SerializeField] protected GameObject DamageParticles = null;

    public enum EnemyDifficutlty
    {
        EASY,
        MEDIUM,
        HARD,
        BOSS,
        NONE
    }
    
    public GameObject Player;

    protected float shootingTimer;
    protected Rigidbody2D rb;
    protected Vector3 target;
    protected Health hp;
    protected WorldManager worldManager;
    protected void Start()
    {
        worldManager = WorldManager.Instance;
        Player = worldManager.Head;
        rb = GetComponent<Rigidbody2D>();
        hp = GetComponent<Health>();
        animator = GetComponent<Animator>();
        DamageParticles.SetActive(false);
        if (projectileName == "") projectileName = "BasicProjectile";
    }

    void FixedUpdate()
    {
        Attack();
        Move();

        if (hp.healthCount <= 0.0f)
        {
            Die();
        } else if (hp.healthCount < hp.healthMax/2 && !DamageParticles.activeSelf)
        {
            DamageParticles.SetActive(true);
        }
    }

    protected abstract void Move();
    protected abstract void Attack();

    public void Die()
    {
        Explosion explosion = worldManager.SpawnFromPool("Explosion", transform.position, transform.rotation).GetComponent<Explosion>();
        if (explosion) explosion.Activate();
        EnemyWaveManager.Instance.aliveEnemies--;
        Destroy(gameObject);
    }

    public bool IsPlayerInSight()
    {
        return (Vector3.Distance(Player.transform.position, transform.position) <= sightDistance);
    }

    
}
