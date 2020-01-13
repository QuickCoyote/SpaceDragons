using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] public Transform WorldCorner = null;
    
    [SerializeField] public List<ItemData> Items = null;
    [SerializeField] GameObject[] objectsToRender = null;

    private GameObject player = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public ItemData GetRandomItemData()
    {
        return Items[Random.Range(0, Items.Count)];
    }

    private void FixedUpdate()
    {
        foreach (GameObject go in objectsToRender)
        {
            if((go.transform.position - player.transform.position).magnitude > 1050)
            {
                go.SetActive(false);
            }
            else
            {
                go.SetActive(true);
            }
        }
    }
}

