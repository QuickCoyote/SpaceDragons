using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] public ItemData ItemData;
    [SerializeField] public SpriteRenderer image;

    private void Start()
    {
        image.sprite = ItemData.itemImage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<Inventory>().AddItem(ItemData, 1);
            Destroy(gameObject);
        }
    }
}
