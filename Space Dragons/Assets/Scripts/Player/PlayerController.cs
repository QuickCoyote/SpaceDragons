using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    #region Flame

    [Header("Flame Attacks")]
    [SerializeField] float flameSpeed = 0f;
    [SerializeField] float flameLifeSpan = 0f;
    [SerializeField] float flameAttackangle = 0f;
    [SerializeField] float flameAttackSpeed = 0f;
    [SerializeField] float flameAttackDamage = 0f;

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
        float acceptableAngle = 30f;

        Collider2D[] col2D = Physics2D.OverlapCircleAll(transform.position, lightningMaxDistance);

        foreach (Collider2D col in col2D)
        {
            Health hp = null;
            col.TryGetComponent(out hp);
            if (hp)
            {
                objectsWithinRange.Add(hp);
            }
        }

        if(objectsWithinRange.Count > 0)
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
            Vector3 randomPointInFront = Quaternion.Euler(0, 0, Random.Range(-15f, 15f)) * head.transform.up * Random.Range(lightningMinDistance, lightningMaxDistance);
            Shock(randomPointInFront);
        }
    }

    public void ShockNext(Health hp)
    {
        Lightning myLightning = null;
        TryGetComponent(out myLightning);

        if (enemiesShocked == 1)
        {
            Debug.Log("ADDED MY LIGHTNING");
            gameObject.AddComponent<Lightning>().target = hp.transform;
        }
        else
        {
            foreach (Component comp in GetComponents<Component>())
            {
                Lightning lightning = null;
                TryGetComponent(out lightning);

                if (lightning)
                {
                    lightning.RemoveLightning();
                }
            }
        }
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(hp.transform.position, lightningMaxDistance);
        foreach (Collider2D col in enemyColliders)
        {
            Health en = null;
            col.TryGetComponent(out en);

            if (en != null && !en.CompareTag("Turret") && !en.CompareTag("Player"))
            {
                if (objectsShocked.Contains(en))
                {
                    return;
                }
                else
                {
                    if (en != hp)
                    {
                        objectsShocked.Add(en);
                        hp.gameObject.AddComponent<Lightning>().target = en.transform;
                        enemiesShocked++;
                        Debug.Log("Shocked enemies: " + enemiesShocked);
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

        if (enemiesShocked == 1)
        {
            Debug.Log("ADDED MY LIGHTNING");
            GameObject obj = new GameObject();
            obj.transform.position = shockPosition;
            gameObject.AddComponent<Lightning>().target = obj.transform;
        }
        else
        {
            foreach (Component comp in GetComponents<Component>())
            {
                Lightning lightning = null;
                TryGetComponent(out lightning);

                if (lightning)
                {
                    lightning.RemoveLightning();
                }
            }
        }
    }


    #endregion

    #region Healing

    [Header("Healing Attacks")]
    [SerializeField] float healingSpeed = 0f;
    [SerializeField] float healingAmount = 0f;

    #endregion

    #region Guard Drone

    [Header("Guard Drones")]
    [SerializeField] int guardDroneCount = 0;

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

    public enum eFireType
    {
        BASIC,
        FLAMETHROWER,
        LIGHTNING,
        HEALING,
        GUARD_DRONE
    }

    public eFireType fireType = eFireType.BASIC;

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
                ShieldingFire();
                break;
        }
    }

    private void ShieldingFire()
    {
        if (guardDroneCount < 2)
        {
            // Spawn a guard drone
        }
    }

    private void HealthyFire()
    {
        //do a heal boi
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
