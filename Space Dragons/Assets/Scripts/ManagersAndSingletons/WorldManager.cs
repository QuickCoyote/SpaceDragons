using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [SerializeField] public Transform WorldCorner = null;

    [SerializeField] public List<ItemData> Items = null;

    [SerializeField] public GameObject Warphole = null;
    [SerializeField] public GameObject Head = null;

    [SerializeField] public PlayerController PlayerController = null;
    [SerializeField] public Ship Ship;

    [SerializeField] public Material lightningMat = null;

    [SerializeField] public EnemyWaveManager enemyWaveManager = null;

    [SerializeField] Enemy[] enemiesToRender = null;

    public List<GameObject> AsteroidsToRender = null;

    [System.Serializable]
    public class Pool
    {
        public string tag = "";
        public GameObject objectPrefab;
        public int maxNumOfObject;
    }
    public Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();
    public List<Pool> GenericPools = new List<Pool>();
    private float dt = 0.0f;

    private void Start()
    {
        if (!Head)
        {
            Head = GameObject.FindGameObjectWithTag("Player");
        }
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
    }

    private void FixedUpdate()
    {
        ResetList();
        foreach (Enemy go in enemiesToRender)
        {
            if (Vector3.Distance(go.transform.position, Head.transform.position) > 50)
            {
                go.transform.position = Head.transform.position + (go.transform.position - Head.transform.position) * 0.75f;
            }
        }

        foreach(GameObject asteroid in AsteroidsToRender)
        {
            if(Vector3.Distance(asteroid.transform.position, Head.transform.position) > 50)
            {
                asteroid.SetActive(true);
            }
        }
    }

    public void ResetList()
    {
        enemiesToRender = FindObjectsOfType<Enemy>();
    }

    #region Spawner Methods

    //public void SpawnRandomExplosion(Vector3 target)
    //{
    //    Explosion explosion = SpawnFromPool("Explosion", target, Quaternion.identity).GetComponent<Explosion>();
    //    if (explosion) explosion.Activate();
    //}

    //public void SpawnWarpHole(Vector3 target)
    //{
    //    WarpHole warp = SpawnFromPool("WarpHole", target, Quaternion.identity).GetComponent<WarpHole>();
    //    if(warp) warp.Activate();
    //}

    //public void SpawnAsteroidDestruction(Vector3 target)
    //{
    //    AsteroidBreakup breakup = SpawnFromPool("AsteroidDestruction", target, Quaternion.identity).GetComponent<AsteroidBreakup>();
    //    if (breakup) breakup.Activate();
    //}

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!objectPools.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't exist.");
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
        return Items[Random.Range(0, Items.Count)];
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

        if (odds < probablility * (1 / 5))
        {
            rarity = ItemData.eItemRarity.LEGENDARY;
        }
        else if (odds < probablility * (1 / 4))
        {
            rarity = ItemData.eItemRarity.EPIC;
        }
        else if (odds < probablility * (1 / 3))
        {
            rarity = ItemData.eItemRarity.RARE;
        }
        else if (odds < probablility * (1 / 2))
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
        List<ItemData> itemsWithRarity = Items.FindAll(e => e.rarity == rarity);
        return itemsWithRarity[Random.Range(0, itemsWithRarity.Count)];
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

