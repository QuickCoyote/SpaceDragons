using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelector : MonoBehaviour
{
    public GameObject ShipMenu;
    public GameObject ShopMenu;
    public GameObject SelecionDisplay;
    public GameObject SelectedShip; 
    public ShipyardController controller;
    public bool IsSlotFilled = false;
    public Sprite baseSprite = null;
    public Sprite turretSprite = null;
    public Sprite wingsSprite = null;
    public Sprite badgeSprite = null;

    public void OpenMenu()
    {
        if(IsSlotFilled)
        {
            ShipMenu.SetActive(true);
            ShopMenu.SetActive(false);
            SelecionDisplay.SetActive(false);

            foreach (Transform child in ShipMenu.GetComponentsInChildren<Transform>())
            {
                if(child.tag == "ShipyardShip")
                {
                    for(int i = 0; i < child.transform.childCount; i++)
                    {
                        child.GetChild(i).GetComponent<Image>().sprite = SelectedShip.transform.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                    }
                }
                if(child.tag == "SellButton")
                {
                    Button button = child.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(delegate { Sell(10); });
                }
                else if(child.tag == "RepairButton")
                {
                    Button button = child.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(delegate { Repair(); });
                }
                else if (child.tag == "UpgradeButton")
                {
                    Button button = child.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(delegate { Upgrade(); });
                }
            }
        }
        else
        {
            ShopMenu.SetActive(true);
            ShipMenu.SetActive(false);
            controller.GetSelectionInfo();
            SelecionDisplay.SetActive(true);
        }

    }

    void Sell(int sellPrice)
    {
        if (controller.MotherShip.bodyPartObjects.Count > 2 && controller.MotherShip.bodyPartObjects[2] != null)
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
    }

    void Repair()
    {
        //SET SHIP HEALTH TO MAXIMUM
        SelectedShip.GetComponent<Health>().healthCount = SelectedShip.GetComponent<Health>().healthMax;
        controller.ShipyardShipSetup();
    }

    void Upgrade()
    {
        //UPGRADE SHIP SOMEHOW?
        Turret curTurret = SelectedShip.GetComponent<Turret>();

        switch(curTurret.turretRarity)
        {
            case ShipData.eTurretRarity.COMMON:
                curTurret.turretRarity = ShipData.eTurretRarity.RARE;
                break;
            case ShipData.eTurretRarity.RARE:
                curTurret.turretRarity = ShipData.eTurretRarity.EPIC;
                break;
        }

        controller.ShipyardShipSetup();
    }

}
