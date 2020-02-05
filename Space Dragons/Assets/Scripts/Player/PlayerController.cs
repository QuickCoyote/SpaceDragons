using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject JoystickControls = null;
    [SerializeField] GameObject TouchControls = null;

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
        GUARD_DRONE,
        LASER
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
        if (guardDroneCount < 2)
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
