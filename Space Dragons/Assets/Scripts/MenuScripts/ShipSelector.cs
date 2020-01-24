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
                    for(int i = 0; i < child.transform.childCount; i++)
                    {
                        Sprite childImage = child.GetChild(i).GetComponent<Image>().sprite;

                        Sprite baseSprite = null;
                        Sprite turretSprite = null;
                        Sprite wingsSprite = null;
                        Sprite badgeSprite = null;

                        for (int j = 0; j < SelectedShip.transform.childCount; j++)
                        {
                            Sprite childSprite = SelectedShip.transform.GetChild(j).GetComponentInChildren<SpriteRenderer>().sprite;
                            switch (j)
                            {
                                case 0:
                                    baseSprite = childSprite;
                                    break;
                                case 1:
                                    turretSprite = childSprite;
                                    break;
                                case 2:
                                    wingsSprite = childSprite;
                                    break;
                                case 3:
                                    badgeSprite = childSprite;
                                    break;
                            }
                        }

                        switch(i)
                        {
                            case 0:
                                childImage = baseSprite;
                                break;
                            case 1:
                                childImage = turretSprite;
                                break;
                            case 2:
                                childImage = wingsSprite;
                                break;
                            case 3:
                                childImage = badgeSprite;
                                break;
                        }
                    }
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
        controller.ShipyardShipSetup();
    }

    void Upgrade()
    {
        //UPGRADE SHIP SOMEHOW?
        controller.ShipyardShipSetup();
    }

}
