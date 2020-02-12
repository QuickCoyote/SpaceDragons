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

    [Header("Control UI")]
    [SerializeField] GameObject JoystickControls = null;
    [SerializeField] GameObject TouchControls = null;

    [Header("Attacks")]
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
        objectsShocked = new List<Health>();

        float acceptableAngle = 30f;

        Collider2D[] col2D = Physics2D.OverlapCircleAll(head.transform.position, lightningMaxDistance);

        foreach (Collider2D col in col2D)
        {
            Health hp = null;
            col.TryGetComponent(out hp);
            if (hp)
            {
                objectsWithinRange.Add(hp);
            }
        }

        if (objectsWithinRange[0])
        {
            Vector3 direction = objectsWithinRange[0].transform.position - head.transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            if (Vector3.Angle(transform.up, direction) < acceptableAngle)
            {
                enemiesShocked = 1;
                ShockNext(objectsWithinRange[0]);
            }
        }
        else
        {
            Vector3 direction = head.transform.eulerAngles + new Vector3(0, 0, Random.Range(-acceptableAngle * 0.5f, acceptableAngle * 0.5f));
            Vector3 randomPointInFront = (direction + head.transform.up) * Random.Range(lightningMinDistance, lightningMaxDistance);
            Shock(randomPointInFront);
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

        if (hp != GetComponent<Health>())
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
        Lightning myLightning = null;
        TryGetComponent(out myLightning);

        if (myLightning)
        {
            myLightning.RemoveLightning();
        }
        else
        {
            gameObject.AddComponent<Lightning>();
        }

        if (shockPosition != head.transform.position)
        {
            gameObject.AddComponent<Lightning>().target = shockPosition;
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
            if(enemyHealth)
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

        foreach(Collider2D collider in colliders)
        {
            if(collider.gameObject.layer != 11 && collider.gameObject.layer != 8)
            {
                collider.TryGetComponent(out enemy);

                if(enemy)
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

    private void GuardDroneFire()
    {
        if (guardDroneCount < 2)
        {
            // Spawn a guard drone
        }
    }

    #endregion

    //Controls
    private bool firing = false;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        LoadData();
        JoystickControls.SetActive(PauseMenu.Instance.JoystickControls);
        TouchControls.SetActive(!PauseMenu.Instance.JoystickControls);
    }

    void LoadData()
    {
        money = LoadManager.Instance.saveData.PlayerMoney;
        inventory.items = LoadManager.Instance.saveData.GetItemsAsDictionary();

    }

    public void onPressFire()
    {
        firing = (true);
    }

    public void onReleaseFire()
    {
        firing = (false);
    }

    private void Update()
    {
        JoystickControls.SetActive(PauseMenu.Instance.JoystickControls);
        TouchControls.SetActive(!PauseMenu.Instance.JoystickControls);
    }



    void FixedUpdate()
    {
        if (PauseMenu.Instance.JoystickControls)
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
                if (!UIDetectionManager.Instance.IsPointerOverUIObject())
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
        if (money - amount > 0)
        {
            money -= amount;
            return true;
        }

        return false;
    }

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
}
