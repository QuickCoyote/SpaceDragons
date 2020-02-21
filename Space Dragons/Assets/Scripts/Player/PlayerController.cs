using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Basic Variables

    public enum eFireType
    {
        BASIC,
        FLAMETHROWER,
        LIGHTNING,
        HEALING,
        GUARD_DRONE
    }

    public eFireType fireType = eFireType.BASIC;

    [Header("Attacks")]
    public float attackSpeed = 0.25f;
    public float attackTimer = 0.0f;

    public int money = 100;
    public float attackDamage = 25.0f;
    public Inventory inventory = null;

    public GameObject head = null;
    public Health headHealth = null;
    public GameObject headBullet = null;
    public GameObject[] headBullets = null;

    [SerializeField] float bulletOffsetY = 1.0f;

    [SerializeField] float basicAttackSpeed = 0f;
    [SerializeField] float basicAttackDamage = 0f;

    float dt = 0;

    #endregion

    #region Basic

    public void BasicFire()
    {
        GameObject projectileGO = (Instantiate(headBullet, head.transform.position + (bulletOffsetY * head.transform.up), Quaternion.identity, null) as GameObject);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.parentobj = head;
        projectile.damage = attackDamage;
        projectile.Fire();
    }

    #endregion

    #region Flame

    [Header("Flame Attacks")]
    [SerializeField] float flameSpeed = 0f;
    [SerializeField] float flameLifeSpan = 0f;
    [SerializeField] float flameAttackangle = 0f;
    [SerializeField] float flameAttackSpeed = 0f;
    [SerializeField] float flameAttackDamage = 0f;

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


    #endregion

    #region Lightning

    [Header("Lightning Attacks")]
    [SerializeField] float lightningAttackSpeed = 0f;
    [SerializeField] float lightningAttackDamage = 0f;
    [SerializeField] float lightningMaxDistance = 0f;
    [SerializeField] float lightningMinDistance = 0f;

    int enemiesShocked = 0;

    List<Health> objectsShocked = new List<Health>();
    List<Health> objectsWithinRange = new List<Health>();

    private void FireLightning()
    {
        objectsWithinRange = new List<Health>();
        objectsShocked = new List<Health>();

        float acceptableAngle = 30f;

        Collider2D[] col2D = Physics2D.OverlapCircleAll(head.transform.position, lightningMaxDistance);

        foreach (Collider2D col in col2D)
        {
            Health hp = null;
            col.TryGetComponent(out hp);
            if (hp)
            {
                if (hp != headHealth)
                {
                    Vector3 direction = col.transform.position - head.transform.position;
                    if (Vector3.Angle(head.transform.up, direction) < acceptableAngle)
                    {
                        objectsWithinRange.Add(hp);
                    }
                }
            }
        }

        if (objectsWithinRange.Count > 0)
        {
            Vector3 direction = objectsWithinRange[0].transform.position - head.transform.position;
            if (Vector3.Angle(head.transform.up, direction) < acceptableAngle)
            {
                enemiesShocked = 1;
                ShockNext(objectsWithinRange[0]);
            }
        }
        else
        {
            Quaternion rotAngle = head.transform.rotation * Quaternion.Euler(0, 0, Random.Range(-acceptableAngle * 0.5f, acceptableAngle * 0.5f));
            Vector3 randomDirection = (rotAngle * head.transform.up);

            randomDirection = (randomDirection.normalized * lightningMaxDistance) + head.transform.parent.position;

            Debug.DrawLine(head.transform.position, randomDirection, Color.red, 5);
            Shock(randomDirection);
        }
    }

    public void ShockNext(Health hp)
    {
        Lightning myLightning = null;
        TryGetComponent(out myLightning);

        if (myLightning)
        {
            myLightning.RemoveLightning();
            myLightning = gameObject.AddComponent<Lightning>();
        }
        else
        {
            myLightning = gameObject.AddComponent<Lightning>();
        }

        if (hp != headHealth)
        {
            myLightning.target = hp.transform.position;
        }
        else
        {
            return;
        }

        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(hp.transform.position, lightningMaxDistance);
        foreach (Collider2D col in enemyColliders)
        {
            Health en = null;
            col.TryGetComponent(out en);

            if (en != null && !(en.gameObject.layer == 11) && !en.CompareTag("Player"))
            {
                if (objectsShocked.Contains(en))
                {
                    continue;
                }
                else
                {
                    if (en != hp)
                    {
                        objectsShocked.Add(en);
                        hp.gameObject.AddComponent<Lightning>().target = en.transform.position;
                        enemiesShocked++;
                        ShockNext(en);
                    }
                }
                en.healthCount -= lightningAttackDamage;
            }
        }
    }

    public void Shock(Vector3 shockPosition)
    {
        Debug.Log("Shock Position: " + shockPosition);
        Lightning myLightning = null;
        TryGetComponent(out myLightning);

        if (myLightning)
        {
            myLightning.RemoveLightning();
        }

        if (shockPosition != head.transform.position)
        {
            head.gameObject.AddComponent<Lightning>().target = shockPosition;
        }
        else
        {
            return;
        }
    }


    #endregion

    #region Healing

    [Header("Healing Attacks")]
    [SerializeField] float healingSpeed = 0f;
    [SerializeField] float healingAmount = 0f;
    Health HealthImHealing = null;

    private void HealthyFire()
    {
        dt += Time.deltaTime;
        if (dt >= healingSpeed)
        {
            if (HealthImHealing.healthCount >= HealthImHealing.healthMax)
            {
                HealthImHealing = FindObjectToHeal();
            }

            // Deal Damage to Enemy
            Health enemyHealth = FindEnemyToDamage();
            if (enemyHealth)
            {
                enemyHealth.healthCount -= healingAmount * Time.deltaTime;
            }

            // Heal
            HealthImHealing.healthCount += healingAmount * Time.deltaTime;
        }
    }

    public Health FindEnemyToDamage()
    {
        Health enemy = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(head.transform.position, 10);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer != 11 && collider.gameObject.layer != 8)
            {
                collider.TryGetComponent(out enemy);

                if (enemy)
                {
                    return enemy;
                }
            }
        }

        return enemy;
    }

    public Health FindObjectToHeal()
    {
        List<GameObject> turretobjs = WorldManager.Instance.Ship.bodyPartObjects;
        Health turretToHeal = null;

        foreach (GameObject obj in turretobjs)
        {
            Health health;
            if (obj == null)
            {
                return GetComponent<Health>();
            }
            obj.TryGetComponent(out health);

            if (health != null)
            {
                if (turretToHeal == null)
                {
                    turretToHeal = obj.GetComponent<Health>();
                }
                else if (obj.GetComponent<Health>().healthCount < turretToHeal.healthCount)
                {
                    turretToHeal = obj.GetComponent<Health>();
                }
            }
        }

        return turretToHeal;
    }

    #endregion

    #region Guard Drone

    [Header("Guard Drones")]
    [SerializeField] int guardDroneCount = 0;
    [SerializeField] GameObject guardDrone = null;
    public GameObject droneHolder = null;
    public GameObject DronePosition1;
    public GameObject DronePosition2;
    public GameObject DronePosition3;

    private void GuardDroneFire()
    {
        if (guardDroneCount < 3)
        {
            SpawnDrone();
        }
    }

    public void SpawnDrone()
    {
        Instantiate(guardDrone, head.transform.position, Quaternion.identity, droneHolder.transform);
        guardDroneCount++;

        for (int i = 0; i < droneHolder.transform.childCount; i++)
        {
            PlayerDrone atk = null;
            transform.GetChild(i).TryGetComponent(out atk);

            if (atk)
            {
                atk.side = i;
            }
        }
    }


    #endregion

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
        if (PlayerPrefs.GetInt("JoystickControls") == 0)
        {
            if (firing)
            {
                attackTimer += Time.deltaTime;

                if (attackTimer > attackSpeed)
                {
                    Fire();
                    attackTimer = 0;
                }
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (!UIManager.Instance.IsPointerOverUIObject())
                {
                    attackTimer += Time.deltaTime;
                    if (attackTimer > attackSpeed)
                    {
                        Fire();
                        attackTimer = 0;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            money += 1000;
        }
    }

    #region Money
    public void AddMoney(int amount)
    {
        if (money + amount < int.MaxValue)
        {
            money += amount;
        }
        else if ((long)(money + amount) > int.MaxValue)
        {
            money = int.MaxValue - 1;
        }
    }

    public bool RemoveMoney(int amount)
    {
        if (money - amount >= 0)
        {
            money -= amount;
            return true;
        }

        return false;
    }

    public string ReturnMoney()
    {
        string returncount = "";
        int switchCase = 0;
        switchCase = money < 10000 ? 0
            : money >= 10000 && money < 100000 ? 1
            : money >= 100000 && money < 1000000 ? 2
            : money >= 1000000 && money < 1000000000 ? 3
            : 4;
        switch (switchCase)
        {
            case 0:
                returncount = money.ToString();
                break;
            case 1:
                int thousands = money / 1000;
                int hundreds = money % 1000;
                char[] hundie = { '0' };
                if (hundreds >= 100)
                {
                    hundie = hundreds.ToString().ToCharArray();
                }
                returncount = thousands.ToString() + "." + hundie[0] + "k";
                break;
            case 2:
                thousands = money / 1000;
                returncount = thousands.ToString() + "k";
                break;
            case 3:
                int millions = money / 1000000;
                returncount = millions.ToString() + "m";
                break;
            case 4:
                int billions = money / 1000000000;
                returncount = billions.ToString() + "b";
                break;
            default:
                break;
        }
        return returncount;
    }
    #endregion

    #region Firing and Attacks
    public void SwitchFireMode(Ship.eMotherShip eMotherShipType)
    {
        switch (eMotherShipType)
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
                attackSpeed = healingSpeed;
                attackDamage = healingAmount;
                HealthyFire();
                break;
            case eFireType.GUARD_DRONE:
                fireType = eFireType.GUARD_DRONE;
                GuardDroneFire();
                break;
        }
    }

    private bool firing = false;
    public void onPressFire()
    {
        firing = (true);
    }

    public void onReleaseFire()
    {
        firing = (false);
    }
    #endregion
}
