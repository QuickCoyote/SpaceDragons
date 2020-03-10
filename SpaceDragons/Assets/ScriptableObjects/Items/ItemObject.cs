using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] public ItemData itemData;
    [SerializeField] public SpriteRenderer image;

    private void SetItemData(ItemData item)
    {
        itemData = item;
        image.sprite = itemData.itemImage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponentInParent<Inventory>().AddItem(itemData, 1);
            other.GetComponentInParent<Inventory>().UpdateDisplay();
            gameObject.SetActive(false);
        }
    }
}
