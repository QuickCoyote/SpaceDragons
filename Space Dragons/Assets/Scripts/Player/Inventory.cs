using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();
    
    public void AddItem(ItemData item, int num)
    {
        List<ItemData> itemsTemp = items.Keys.ToList();
        for (int i = 0; i < items.Keys.Count; i++)
        {
            if(itemsTemp[i] == item)
            {
                items[item] += num;
                break;
            }
        }
    }

    public void RemoveItem(ItemData item, int num)
    {
        List<ItemData> itemsTemp = items.Keys.ToList();
        for (int i = 0; i < items.Keys.Count; i++)
        {
            if (itemsTemp[i] == item)
            {
                if(items[item] >= num)
                {
                    items[item] -= num;
                }
                else
                {
                    // You don't have that many
                }
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        List<ItemData> itemsTemp = items.Keys.ToList();
        for (int i = 0; i < items.Keys.Count; i++)
        {
            ItemData item = itemsTemp[i];
            if (items[item] <= 0)
            {
                items.Remove(item);
            }
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {

    }
}
