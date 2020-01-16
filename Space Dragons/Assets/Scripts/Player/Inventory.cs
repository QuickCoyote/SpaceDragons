using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Sprite emptyItemImage;
    [SerializeField] GameObject inventoryDisplay = null;
    [SerializeField] GameObject closeInventory = null;

    List<ItemData> inventory = new List<ItemData>();
    Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    public void AddItem(ItemData item, int num)
    {
        if (items.Keys.Count < inventory.Count)
        {
            List<ItemData> itemsTemp = items.Keys.ToList();
            for (int i = 0; i < items.Keys.Count; i++)
            {
                if (items.Keys.Contains(item))
                {
                    items[item] += num;
                    break;
                }
                else
                {
                    items.Add(item, num);
                }
            }
        }

    }

    public void RemoveItem(ItemData item, int num)
    {
        if (items.Keys.Count > 0)
        {
            List<ItemData> itemsTemp = items.Keys.ToList();
            for (int i = 0; i < items.Keys.Count; i++)
            {
                if (itemsTemp[i] == item)
                {
                    if (items[item] >= num)
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
        int num = 0;
        for (int i = 0; i < items.Keys.Count; i++)
        {
            if (items[items.Keys.ElementAt(i)] > 0)
            {
                inventory[num] = items.Keys.ElementAt(i);
                num++;
            }
        }

        for (int i = 0; i < inventoryDisplay.transform.childCount; i++)
        {
            if (i < inventory.Count)
            {
                inventoryDisplay.transform.GetChild(i).GetComponentInChildren<Image>().sprite = inventory[i].itemImage;
                inventoryDisplay.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = "x" + items[inventory[i]];
            }
            else
            {
                inventoryDisplay.transform.GetChild(i).gameObject.GetComponentInChildren<Image>().sprite = emptyItemImage;
                inventoryDisplay.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void ToggleDisplay()
    {
        inventoryDisplay.SetActive(!inventoryDisplay.activeSelf);
        closeInventory.SetActive(!closeInventory.activeSelf);
        if (inventoryDisplay.activeSelf)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
