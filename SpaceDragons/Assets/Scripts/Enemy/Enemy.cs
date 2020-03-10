using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject gunNozzle = null;
    [SerializeField] public WorldManager.ePoolTag projectileName = WorldManager.ePoolTag.PROJECTILE_BASIC;
    [SerializeField] public float speed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public float shootingSpeed;
    [SerializeField] public float sightDistance;
    [SerializeField] public float attackDamage;
    [SerializeField] public EnemyDifficutlty difficulty;
    [SerializeField] protected GameObject DamageParticles = null;

    protected Vector3 direction = Vector3.zero;
    protected float angle = 0.0f;
    protected Quaternion rotation;

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
        DamageParticles.SetActive(false);
    }

    void FixedUpdate()
    {
        Attack();
        Move();

        if (hp.healthCount <= 0.0f)
        {
            Die();
        } else if (hp.healthCount < hp.healthMax * 0.5f && !DamageParticles.activeSelf)
        {
            DamageParticles.SetActive(true);
        }
    }

    protected abstract void Move();
    protected abstract void Attack();

    public void Die()
    {
        Explosion explosion = worldManager.SpawnFromPool(WorldManager.ePoolTag.EXPLOSION, transform.position, transform.rotation).GetComponent<Explosion>();
        if (explosion) explosion.Activate();
        EnemyWaveManager.Instance.aliveEnemies--;
        Destroy(gameObject);
    }

    public bool IsPlayerInSight()
    {
        return (Vector3.Distance(Player.transform.position, transform.position) <= sightDistance);
    }

    
}
