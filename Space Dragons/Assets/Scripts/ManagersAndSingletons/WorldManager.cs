using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [SerializeField] public Transform WorldCorner = null;
    
    [SerializeField] public List<ItemData> Items = null;
    [SerializeField] public List<GameObject> Explosions = null;
    [SerializeField] public GameObject Warphole = null;
    [SerializeField] Rigidbody2D[] objectsToRender = null;
    [SerializeField] public GameObject Head = null;
    [SerializeField] public PlayerController PlayerController = null;
    [SerializeField] public Ship Ship;

    [SerializeField] public Material lightningMat = null;

    [SerializeField] public EnemyWaveManager enemyWaveManager = null;

    private void Start()
    {
        ResetList();
        if(Head == null)
        {
            Head = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void FixedUpdate()
    {
        ResetList();
        foreach (Rigidbody2D go in objectsToRender)
        {
            if(Vector3.Distance(go.transform.position, Head.transform.position) > 150)
            {
                if (go.gameObject.layer == 13)
                {
                    go.gameObject.SetActive(false);
                } 
                else
                {
                    go.transform.position = Head.transform.position + (go.transform.position - Head.transform.position) * 0.75f;
                }
            }
            else
            {
                if (go.gameObject.layer != 13)
                {
                    go.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ResetList()
    {
        objectsToRender = FindObjectsOfType<Rigidbody2D>();
    }

    #region Spawner Methods

    public void SpawnRandomExplosion(Vector3 target)
    {
        Instantiate(Explosions[Random.Range(0, Explosions.Count)], target, Quaternion.identity, null);
    }


    public void SpawnWarpHole(Vector3 target)
    {
        Instantiate(Warphole, target, Quaternion.identity, null);
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

