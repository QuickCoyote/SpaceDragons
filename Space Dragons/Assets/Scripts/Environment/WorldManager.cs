using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [SerializeField] public Transform WorldCorner = null;
    
    [SerializeField] public List<ItemData> Items = null;
    [SerializeField] public List<GameObject> Explosions = null;
    [SerializeField] Rigidbody2D[] objectsToRender = null;

    [SerializeField] public GameObject Player = null;
    [SerializeField] public Ship Ship;

    [SerializeField] public Material lightningMat = null;

    private void Start()
    {
        ResetList();
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public ItemData GetRandomItemData()
    {
        return Items[Random.Range(0, Items.Count)];
    }

    public void SpawnRandomExplosion(Vector3 target)
    {
        Instantiate(Explosions[Random.Range(0, Explosions.Count)], target, Quaternion.identity, null);
    }

    private void FixedUpdate()
    {
        ResetList();
        foreach (Rigidbody2D go in objectsToRender)
        {
            if((go.transform.position - Player.transform.position).magnitude > 150)
            {
                go.gameObject.SetActive(false);
            }
            else
            {
                go.gameObject.SetActive(true);
            }
        }
    }

    public void ResetList()
    {
        objectsToRender = FindObjectsOfType<Rigidbody2D>();
    }
}

