using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelector : MonoBehaviour
{
    public GameObject ShipMenu;
    public GameObject ShopMenu;
    public GameObject SelectedShip;
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
                    child.GetComponent<Image>().sprite = SelectedShip.GetComponent<Image>().sprite;
                    child.GetComponent<Image>().color = SelectedShip.GetComponent<Image>().color;
                }
                if(child.tag == "SellButton")
                {
                    Button button = child.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(delegate { Sell(10); });
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
        controller.MotherShip.RemoveBodyPart(SelectedShip, true);
        controller.MotherShip.bodyPartObjects.Add(null);
        controller.MotherShip.bodyPartTransforms.Add(null);
        controller.MotherShip.SortBody();
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
