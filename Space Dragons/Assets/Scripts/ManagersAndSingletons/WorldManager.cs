using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [SerializeField] public Transform WorldCorner = null;
    [SerializeField] public ItemData[] Items = null;
    [SerializeField] public GameObject Head;
    [SerializeField] public PlayerController PlayerController;
    [SerializeField] public Ship Ship;
    [SerializeField] public Material lightningMat = null;
    [SerializeField] public EnemyWaveManager enemyWaveManager = null;
    [SerializeField] Enemy[] enemiesToRender = null;
    public Asteroid[] AsteroidsToRender = null;

    [System.Serializable]
    public class Pool
    {
        public ePoolTag tag;
        public GameObject objectPrefab;
        public int maxNumOfObject;
    }
    public enum ePoolTag
    {
        ASTEROID,
        ASTEROID_BREAKUP,
        EXPLOSION,
        ITEM,
        WARPHOLE,
        PROJECTILE_BASIC,
        PROJECTILE_PLAYER_DEFAULT,
        PROJECTILE_PLAYER_FIRE,
        PROJECTILE_DRONE,
        PROJECTILE_TURRET_DEFAULT,
        PROJECTILE_TURRET_FLAME,
        PROJECTILE_BOSS_FLAME,
        PROJECTILE_BOSS_HOMING
    }
    public Dictionary<ePoolTag, Queue<GameObject>> objectPools = new Dictionary<ePoolTag, Queue<GameObject>>();
    public Pool[] GenericPools;
    public Pool[] ProjectilePools; //Separated mostly for ease of access
    private float dt = 0.0f;

    private void Awake()
    {
        base.Awake();
        if (!Head)
        {
            Head = GameObject.FindGameObjectWithTag("Player");
        }
        enemyWaveManager = EnemyWaveManager.Instance;
        PlayerController = FindObjectOfType<PlayerController>();
        Ship = FindObjectOfType<Ship>();
        foreach (Pool pool in GenericPools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.maxNumOfObject; i++)
            {
                GameObject obj = Instantiate(pool.objectPrefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            objectPools.Add(pool.tag, objectPool);
        }

        foreach (Pool pool in ProjectilePools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.maxNumOfObject; i++)
            {
                GameObject obj = Instantiate(pool.objectPrefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            objectPools.Add(pool.tag, objectPool);
        }

        AsteroidsToRender = FindObjectsOfType<Asteroid>();
        StartCoroutine("DisplayAsteroids");
        StartCoroutine("MoveEnemies");
    }


    IEnumerator MoveEnemies()
    {
        while (true)
        { 
            //enemiesToRender = GameObject.FindGameObjectsWithTag("Enemy");
            enemiesToRender = FindObjectsOfType<Enemy>();
            //foreach (GameObject go in enemiesToRender)
            foreach (Enemy go in enemiesToRender)
            {
                if (Vector3.Distance(go.transform.position, Head.transform.position) > 75)
                {
                    go.transform.position = Head.transform.position + (go.transform.position - Head.transform.position) * 0.75f;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator DisplayAsteroids()
    {
        while (true)
        {
            foreach (Asteroid asteroid in AsteroidsToRender)
            {
                if (Vector3.Distance(asteroid.transform.position, Head.transform.position) > 50)
                {
                    asteroid.gameObject.SetActive(true);
                }
            }

            yield return new WaitForSeconds(5f);
        }
    }

    #region Spawner Methods
    public GameObject SpawnFromPool(ePoolTag tag, Vector3 position, Quaternion rotation)
    {
        if (!objectPools.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        GameObject objectToSpawn = objectPools[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectPools[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    #endregion

    #region Return Item Methods

    public ItemData GetRandomItemData()
    {
        return Items[Random.Range(0, Items.Length)];
    }

    public ItemData GetRandomItemDataWeighted() // Better odds, more even possibilities.
    {
        float probablility = Random.Range(0, 100);
        ItemData.eItemRarity rarity = ItemData.eItemRarity.COMMON;

        if (probablility > 98)
        {
            rarity = ItemData.eItemRarity.LEGENDARY;
        }
        else if (probablility > 90)
        {
            rarity = ItemData.eItemRarity.EPIC;
        }
        else if (probablility > 75)
        {
            rarity = ItemData.eItemRarity.RARE;
        }
        else if (probablility > 50)
        {
            rarity = ItemData.eItemRarity.UNCOMMON;
        }
        else
        {
            rarity = ItemData.eItemRarity.COMMON;
        }
        return GetRandomItemWithRarity(rarity);
    }

    public ItemData GetRandomItemDataStepped() //Very steep odds, not as likely to get good gear.
    {
        float probablility = Random.Range(0, 100);
        float odds = Random.Range(0, 100);
        ItemData.eItemRarity rarity = ItemData.eItemRarity.COMMON;

        if (odds < probablility * (1 * .20))
        {
            rarity = ItemData.eItemRarity.LEGENDARY;
        }
        else if (odds < probablility * (1 * .25))
        {
            rarity = ItemData.eItemRarity.EPIC;
        }
        else if (odds < probablility * (1 *0.33))
        {
            rarity = ItemData.eItemRarity.RARE;
        }
        else if (odds < probablility * (1 * 0.5))
        {
            rarity = ItemData.eItemRarity.UNCOMMON;
        }
        else
        {
            rarity = ItemData.eItemRarity.COMMON;
        }
        return GetRandomItemWithRarity(rarity);
    }

    public ItemData GetRandomItemWithRarity(ItemData.eItemRarity rarity)
    {
        ItemData[] itemsWithRarity = Items.Where(e => e.rarity == rarity).ToArray();
        return itemsWithRarity[Random.Range(0, itemsWithRarity.Length)];
    }

    public ItemData GetItemById(string Id)
    {
        foreach (ItemData i in Items)
        {
            if (i.itemID == Id)
            {
                return i;
            }
        }
        return null;
    }

    #endregion
}

