using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float attackSpeed = 0.25f;
    public float attackTimer = 0.0f;

    public int money = 100;
    public float attackDamage = 25.0f;
    public Inventory inventory = null;

    public GameObject head = null;
    public GameObject headBullet = null;
    public GameObject[] headBullets = null;

    [SerializeField] float bulletOffsetY = 1.0f;

    [SerializeField] float basicAttackSpeed = 0f;
    [SerializeField] float basicAttackDamage = 0f;

    [SerializeField] float flameSpeed = 0f;
    [SerializeField] float flameLifeSpan = 0f;
    [SerializeField] float flameAttackangle = 0f;
    [SerializeField] float flameAttackSpeed = 0f;
    [SerializeField] float flameAttackDamage = 0f;

    [SerializeField] float lightningAttackSpeed = 0f;
    [SerializeField] float lightningAttackDamage = 0f;

    [SerializeField] float healingAttackSpeed = 0f;
    [SerializeField] float healingAttackDamage = 0f;

    [SerializeField] int guardDroneCount = 0;

    [SerializeField] float laserAttackSpeed = 0f;
    [SerializeField] float laserAttackDamage = 0f;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        LoadData();

    }
    void LoadData()
    {
        money = LoadManager.Instance.saveData.PlayerMoney;
        inventory.items = LoadManager.Instance.saveData.GetItemsAsDictionary();
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > attackSpeed)
            {
                Fire();
                attackTimer = 0;
            }
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public bool RemoveMoney(int amount)
    {
        if (money - amount > 0)
        {
            money -= amount;
            return true;
        }

        return false;
    }

    public enum eFireType
    {
        BASIC,
        FLAMETHROWER,
        LIGHTNING,
        HEALING,
        GUARD_DRONE,
        LASER
    }

    public eFireType fireType = eFireType.BASIC;

    public void SwitchFireMode(Ship.eMotherShip eMotherShipType)
    {
        switch(eMotherShipType)
        {
            case Ship.eMotherShip.BASIC:
                fireType = eFireType.BASIC;
                break;
            case Ship.eMotherShip.FLAMETHROWER:
                fireType = eFireType.FLAMETHROWER;
                break;
            case Ship.eMotherShip.LIGHTNING:
                fireType = eFireType.LIGHTNING;
                break;
            case Ship.eMotherShip.HEALING:
                fireType = eFireType.HEALING;
                break;
            case Ship.eMotherShip.GUARD_DRONE:
                fireType = eFireType.GUARD_DRONE;
                break;
            case Ship.eMotherShip.LASER:
                fireType = eFireType.LASER;
                break;
        }
    }

    public void Fire()
    {
        switch (fireType)
        {
            case eFireType.BASIC:
                fireType = eFireType.BASIC;
                attackSpeed = basicAttackSpeed;
                attackDamage = basicAttackDamage;
                BasicFire();
                break;
            case eFireType.FLAMETHROWER:
                fireType = eFireType.FLAMETHROWER;
                attackSpeed = flameAttackSpeed;
                attackDamage = flameAttackDamage;
                FlameFire();
                break;
            case eFireType.LIGHTNING:
                fireType = eFireType.LIGHTNING;
                attackSpeed = lightningAttackSpeed;
                attackDamage = lightningAttackDamage;
                FireLightning();
                break;
            case eFireType.HEALING:
                fireType = eFireType.HEALING;
                attackSpeed = healingAttackSpeed;
                attackDamage = healingAttackDamage;
                HealthyFire();
                break;
            case eFireType.GUARD_DRONE:
                fireType = eFireType.GUARD_DRONE;
                ShieldingFire();
                break;
            case eFireType.LASER:
                fireType = eFireType.LASER;
                attackSpeed = laserAttackSpeed;
                attackDamage = laserAttackDamage;
                LaserFire();
                break;
        }
    }


    private void LaserFire()
    {

    }

    private void ShieldingFire()
    {
        if(guardDroneCount < 2)
        {
            // Spawn a guard drone
        }
    }

    private void HealthyFire()
    {

    }

    private void FireLightning()
    {

    }

    private void FlameFire()
    {
        Quaternion rotAngle = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-flameAttackangle, flameAttackangle));
        Vector3 projectileDirection = rotAngle * head.transform.up;

        GameObject projectileGO = (Instantiate(headBullet, head.transform.position + (bulletOffsetY * head.transform.up), Quaternion.identity, null) as GameObject);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.parentobj = head;
        projectile.damage = attackDamage;
        projectile.goDirection = projectileDirection;
        projectile.lifetime = flameLifeSpan;
        projectile.bulletSpeed = flameSpeed;
        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        projectile.sound = "null";
        projectile.Fire();
    }

    public void BasicFire()
    {
        GameObject projectileGO = (Instantiate(headBullet, head.transform.position + (bulletOffsetY * head.transform.up), Quaternion.identity, null) as GameObject);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.parentobj = head;
        projectile.damage = attackDamage;
        projectile.Fire();
    }

}
