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
    public float attackDamage = 25.0f;

    [SerializeField] float bulletOffsetY = 1.0f;
    [SerializeField] float basicAttackSpeed = 0f;
    [SerializeField] float basicAttackDamage = 0f;

    public int money = 100;

    public Inventory inventory = null;
    WorldManager worldManager;
    Ship ship;

    float dt = 0;

    #endregion

    #region Basic

    public void BasicFire()
    {
        GameObject projectileGO = worldManager.SpawnFromPool(WorldManager.ePoolTag.PROJECTILE_PLAYER_DEFAULT, ship.head.transform.position + (bulletOffsetY * ship.head.transform.up), Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.parentobj = ship.head;
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
        Vector3 projectileDirection = rotAngle * ship.head.transform.up;

        GameObject projectileGO = worldManager.SpawnFromPool(WorldManager.ePoolTag.PROJECTILE_PLAYER_FIRE, ship.head.transform.position + (bulletOffsetY * ship.head.transform.up), Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.parentobj = ship.head;
        projectile.damage = attackDamage;
        projectile.goDirection = projectileDirection;
        projectile.lifetime = flameLifeSpan;
        projectile.bulletSpeed = flameSpeed + WorldManager.Instance.Ship.speed;
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
    [SerializeField] GameObject myTargetIcon = null;

    int enemiesShocked = 0;

    List<Health> objectsShocked = new List<Health>();
    List<Health> objectsWithinRange = new List<Health>();

    private void FireLightning()
    {
        objectsWithinRange = new List<Health>();
        objectsShocked = new List<Health>();

        float acceptableAngle = 30f;

        Collider2D[] col2D = Physics2D.OverlapCircleAll(ship.head.transform.position, lightningMaxDistance);

        foreach (Collider2D col in col2D)
        {
            Health hp = null;
            col.TryGetComponent(out hp);
            if (hp)
            {
                if (hp != ship.headHealth)
                {
                    if (hp.gameObject.layer != 11 && hp.gameObject.layer != 8)
                    {
                        Vector3 direction = col.transform.position - ship.head.transform.position;
                        if (Vector3.Angle(ship.head.transform.up, direction) < acceptableAngle)
                        {
                            objectsWithinRange.Add(hp);
                        }
                    }
                }
            }
        }

        if (objectsWithinRange.Count > 0)
        {
            Vector3 direction = objectsWithinRange[0].transform.position - ship.head.transform.position;
            myTargetIcon.SetActive(true);
            myTargetIcon.transform.position = objectsWithinRange[0].transform.position;
            if (Vector3.Angle(ship.head.transform.up, direction) < acceptableAngle)
            {
                enemiesShocked = 1;
                ShockNext(objectsWithinRange[0]);
            }
        }
        else
        {
            myTargetIcon.SetActive(false);
            Quaternion rotAngle = ship.head.transform.rotation * Quaternion.Euler(0, 0, Random.Range(-acceptableAngle * 0.5f, acceptableAngle * 0.5f));
            Vector3 randomDirection = (rotAngle * ship.head.transform.up);
            randomDirection = (randomDirection.normalized * lightningMaxDistance) + ship.head.transform.parent.position;

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

        if (hp != ship.headHealth)
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

        if (shockPosition != ship.head.transform.position)
        {
            ship.head.gameObject.AddComponent<Lightning>().target = shockPosition;
        }
        else
        {
            return;
        }
    }


    #endregion

    #region Healing

    [Header("Healing Attacks")]
    [SerializeField] AnimationCurve LeechWidthCurve = null;
    [SerializeField] Material LeechLineMat = null;
    [SerializeField] float healingSpeed = 0f;
    [SerializeField] float healingAmount = 0f;
    Health HealthImHealing = null;

    Health prevEnemyHealth = null;
    Health enemyHealth = null;

    private void HealthyFire()
    {
        if (!HealthImHealing)
        {
            HealthImHealing = FindObjectToHeal();
        }

        dt += Time.deltaTime;
        LineRenderer enemyLR = null;
        if (dt >= healingSpeed)
        {
            if (HealthImHealing)
            {
                if (HealthImHealing.healthCount >= HealthImHealing.healthMax)
                {
                    HealthImHealing = FindObjectToHeal();
                }

                // Deal Damage to Enemy
                prevEnemyHealth = enemyHealth;
                enemyHealth = FindEnemyToDamage();
                if (enemyHealth)
                {
                    if (prevEnemyHealth != enemyHealth)
                    {
                        Destroy(prevEnemyHealth.GetComponent<LineRenderer>());
                    }

                    enemyHealth.healthCount -= healingAmount * Time.deltaTime;
                    if (!enemyHealth.gameObject.TryGetComponent(out enemyLR))
                    {
                        enemyLR = enemyHealth.gameObject.AddComponent<LineRenderer>();
                        enemyLR.startWidth = 0.5f;
                        enemyLR.endWidth = 0.5f;
                        enemyLR.material = LeechLineMat;
                        enemyLR.sortingLayerName = "Default";
                        enemyLR.sortingOrder = 95;
                        Color myColor = Color.red;
                        myColor.a = .5f;
                        enemyLR.startColor = myColor;

                        myColor = Color.green;
                        myColor.a = .5f;
                        enemyLR.endColor = myColor;
                    }
                }

                // Heal
                HealthImHealing.healthCount += healingAmount * Time.fixedDeltaTime;
            }
        }
        if(enemyHealth)
        {
            Vector3[] points = new Vector3[2];

            points[0] = enemyHealth.transform.position;
            points[1] = ship.head.transform.position;

            enemyLR.SetPositions(points);
        }
    }

    public Health FindEnemyToDamage()
    {
        Health enemy = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(ship.head.transform.position, 10);

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
        GameObject[] turretobjs = WorldManager.Instance.Ship.bodyPartObjects.ToArray();
        Health turretToHeal = null;

        foreach (GameObject obj in turretobjs)
        {
            Health health;
            if (!obj)
            {
                turretToHeal = GetComponent<Health>();
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
    [SerializeField] GameObject guardDrone = null;
    [SerializeField] float guardDroneFireTime = 0.75f;

    public List<GameObject> guardDrones = new List<GameObject>();
    public GameObject droneHolder = null;
    public GameObject DronePosition1;
    public GameObject DronePosition2;
    public GameObject DronePosition3;
    public int guardDroneCount = 0;

    private void GuardDroneFire()
    {
        foreach (GameObject go in guardDrones)
        {
            PlayerDrone drone = null;

            if (go.TryGetComponent(out drone))
            {
                drone.Attack();
            }
        }
    }

    public void SpawnDrone()
    {
        guardDroneCount++;
        guardDrones.Add(Instantiate(guardDrone, ship.head.transform.position, Quaternion.identity, null));

        for (int i = 0; i < guardDrones.Count; i++)
        {
            PlayerDrone atk = null;

            if (guardDrones[i].TryGetComponent(out atk))
            {
                atk.side = i;
            }
        }

        AlignDrones();
    }

    public void AlignDrones()
    {
        foreach (GameObject go in guardDrones)
        {
            PlayerDrone drone = go.GetComponent<PlayerDrone>();
            switch (drone.side)
            {
                case 0:
                    drone.targetPosition = ship.head.transform.right + ship.head.transform.position;
                    break;
                case 1:
                    drone.targetPosition = ship.head.transform.up + ship.head.transform.position;
                    break;
                case 2:
                    drone.targetPosition = -ship.head.transform.right + ship.head.transform.position;
                    break;
            }
        }
    }

    #endregion

    void Start()
    {
        inventory = GetComponent<Inventory>();
        worldManager = WorldManager.Instance;
        ship = worldManager.Ship;
        LoadData();
    }

    void LoadData()
    {
        money = LoadManager.Instance.saveData.PlayerMoney;
        inventory.items = LoadManager.Instance.saveData.GetItemsAsDictionary();
    }

    void FixedUpdate()
    {
        if (!(fireType == eFireType.LIGHTNING))
        {
            myTargetIcon.SetActive(false);
        }

        if (fireType == eFireType.GUARD_DRONE)
        {
            if (guardDroneCount < 3)
            {
                SpawnDrone();
            }

            AlignDrones();
        }
        else
        {
            if(guardDroneCount > 0)
            {
                foreach(GameObject drone in guardDrones)
                {
                    Destroy(drone);
                }
            }
        }

        for (int i = 0; i < guardDrones.Count; i++)
        {
            if (!guardDrones[i])
            {
                guardDrones.RemoveAt(i);
                guardDroneCount--;
            }
        }

        if (objectsWithinRange.Count > 0)
        {
            if (objectsWithinRange[0])
            {
                myTargetIcon.SetActive(true);
                myTargetIcon.transform.position = objectsWithinRange[0].transform.position;
            }
        }

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
                float thousands = money * .0001f;
                int hundreds = money % 1000;
                char[] hundie = { '0' };
                if (hundreds >= 100)
                {
                    hundie = hundreds.ToString().ToCharArray();
                }
                returncount = ((int)thousands).ToString() + "." + hundie[0] + "k";
                break;
            case 2:
                thousands = money * .0001f;
                returncount = ((int)thousands).ToString() + "k";
                break;
            case 3:
                float millions = money * .0000001f;
                returncount = ((int)millions).ToString() + "m";
                break;
            case 4:
                float billions = money * .0000000001f;
                returncount = ((int)billions).ToString() + "b";
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
                attackSpeed = basicAttackSpeed;
                attackDamage = basicAttackDamage;
                BasicFire();
                break;
            case eFireType.FLAMETHROWER:
                attackSpeed = flameAttackSpeed;
                attackDamage = flameAttackDamage;
                FlameFire();
                break;
            case eFireType.LIGHTNING:
                attackSpeed = lightningAttackSpeed;
                attackDamage = lightningAttackDamage;
                FireLightning();
                break;
            case eFireType.HEALING:
                attackSpeed = healingSpeed;
                attackDamage = healingAmount;
                HealthyFire();
                break;
            case eFireType.GUARD_DRONE:
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
