using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] public Transform WorldCorner = null;
    [SerializeField] public List<ItemData> Items = null;

    public ItemData GetRandomItemData()
    {
        return Items[Random.Range(0, Items.Count)];
    }


    ///
}

