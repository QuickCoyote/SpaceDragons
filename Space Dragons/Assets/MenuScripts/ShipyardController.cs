using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipyardController : MonoBehaviour
{
    public List<GameObject> Ships;
    public GameObject ShipScrollContent;
    public GameObject ShipButtonPrefab;
    public GameObject ShipMenu;
    public GameObject ShopMenu;

    private void Start()
    {

        foreach (Transform child in ShipScrollContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < Ships.Count; i++)
        {
            GameObject obj = Instantiate(ShipButtonPrefab);
            Button button = obj.GetComponentInChildren<Button>();
            button.gameObject.AddComponent<ShipSelector>();
            ShipSelector selector = button.GetComponent<ShipSelector>();
            selector.ShipMenu = ShipMenu;
            selector.ShopMenu = ShopMenu;
            if (Ships[i] != null)
            {
                button.image.sprite = Ships[i].GetComponent<ShipTest>().ShipSprite;
                selector.IsSlotFilled = true;
            }
            button.onClick.AddListener(delegate { selector.OpenMenu(); });
            obj.transform.parent = ShipScrollContent.transform;

        }

    }


}
