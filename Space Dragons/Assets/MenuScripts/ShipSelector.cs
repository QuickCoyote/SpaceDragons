using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelector : MonoBehaviour
{
    public GameObject ShipMenu;
    public GameObject ShopMenu;
    public ShipTest SelectedShip;
    public ShipyardController controller;
    public bool IsSlotFilled = false;

    public void OpenMenu()
    {
        if(IsSlotFilled)
        {
            ShipMenu.SetActive(true);
            ShopMenu.SetActive(false);

            foreach (Transform child in ShipMenu.GetComponentsInChildren<Transform>())
            {
                if(child.tag == "ShipyardShip")
                {
                    child.GetComponent<Image>().sprite = SelectedShip.ShipSprite;
                }
                if(child.tag == "SellButton")
                {
                    Button button = child.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(delegate { Sell(SelectedShip.SellPrice); });
                    button.onClick.AddListener(delegate { Repair(); });
                    button.onClick.AddListener(delegate { Upgrade(); });
                }
            }
            
        }
        else
        {
            ShopMenu.SetActive(true);
            ShipMenu.SetActive(false);
        }

    }

    void Sell(int sellPrice)
    {
        IsSlotFilled = false;
        ShipMenu.SetActive(false);
        controller.Ships.Remove(SelectedShip);
        controller.Ships.Add(null);
        controller.ShipyardShipSetup();
        //PLACE MONEY ADDING HERE
    }

    void Repair()
    {
        //SET SHIP HEALTH TO MAXIMUM
        controller.ShipyardShipSetup();
    }

    void Upgrade()
    {
        //UPGRADE SHIP SOMEHOW?
        controller.ShipyardShipSetup();
    }

}
