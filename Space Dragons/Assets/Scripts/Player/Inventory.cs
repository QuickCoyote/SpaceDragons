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
    [SerializeField] GameObject itemInfoPanel = null;
    [SerializeField] Image itemInfoPanelImage = null;
    [SerializeField] TextMeshProUGUI itemInfoPanelName = null;
    [SerializeField] TextMeshProUGUI itemInfoPanelDesc = null;

    public List<ItemData> inventory = new List<ItemData>();
    public Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    public void AddItem(ItemData item, int num)
    {
        if (items.ContainsKey(item))
        {
            for (int i = 0; i < items.Keys.Count; i++)
            {
                if (items.Keys.ElementAt(i) == item)
                {
                    items[item] += num;
                    return;
                }
            }
        }
        else
        {
            items.Add(item, num);
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

    public void UpdateInventory()
    {
        inventory = new List<ItemData>();
        for (int i = 0; i < items.Keys.Count; i++)
        {
            if (items[items.Keys.ElementAt(i)] > 0)
            {
                inventory.Add(items.Keys.ElementAt(i));
            }
        }
    }

    public void UpdateDisplay()
    {
        inventory = new List<ItemData>();
        for (int i = 0; i < items.Keys.Count; i++)
        {
            if (items[items.Keys.ElementAt(i)] > 0)
            {
                inventory.Add(items.Keys.ElementAt(i));
            }
        }

        for (int i = 0; i < inventoryDisplay.transform.childCount; i++)
        {
            Transform child = inventoryDisplay.transform.GetChild(i);
            if (i < inventory.Count)
            {
                child.GetComponent<ItemObject>().itemData = inventory[i];
                child.GetComponentInChildren<Image>().sprite = inventory[i].itemImage;
                child.GetComponentInChildren<TextMeshProUGUI>().text = "x" + items[inventory[i]];
            }
            else
            {
                child.GetComponent<ItemObject>().itemData = null;
                child.GetComponentInChildren<Image>().sprite = emptyItemImage;
                child.GetComponentInChildren<TextMeshProUGUI>().text = "";
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
            ToggleItemInfo(false);
            Time.timeScale = 1.0f;
        }
    }

    public void ToggleItemInfo(bool value)
    {
        itemInfoPanel.SetActive(value);
    }

    public void UpdateItemInfoPanel(ItemObject dataHolder)
    {
        if(dataHolder.itemData != null)
        {
            ItemData data = dataHolder.itemData;

            itemInfoPanelImage.sprite = data.itemImage;
            itemInfoPanelName.text = "Item Name: " + data.itemName;
            itemInfoPanelDesc.text = "Item Description: " + data.description;
        }
    }
}
