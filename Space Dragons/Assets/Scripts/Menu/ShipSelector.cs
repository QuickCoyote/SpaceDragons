using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

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

    Health shipHealth;
    Slider healthSlider;
    RectTransform healthbar;

    public void OpenMenu()
    {
        if(IsSlotFilled)
        {
            ShipMenu.SetActive(true);
            ShopMenu.SetActive(false);
            SelecionDisplay.SetActive(true);

            int num = 0;
            for (int i = 0; i < controller.SelectionInfoPanels.Count; i++)
            {
                if (controller.SelectionInfoPanels[i].activeInHierarchy)
                {
                    num = i;
                    break;
                }
            }
            controller.OpenSelectedPanel(0);
            controller.GetSelectionInfo(false, SelectedShip);
            controller.OpenSelectedPanel(num);

            controller.CheckIfSpecial(SelectedShip);

            shipHealth = SelectedShip.GetComponent<Health>();

            float repairCostPerHP = 1f;
            float hpToRestore = 0f;
            hpToRestore += shipHealth.healthMax - shipHealth.healthCount;   

            if(shipHealth.healthCount != shipHealth.healthMax)
            {
                ShipMenu.GetComponentsInChildren<TextMeshProUGUI>().Where
                    (o => o.name == "CostText").FirstOrDefault().text = "$" + ((int)(hpToRestore * repairCostPerHP)).ToString();
            }
            else
            {
                ShipMenu.GetComponentsInChildren<TextMeshProUGUI>().Where
                                    (o => o.name == "CostText").FirstOrDefault().text = "Health Full";

            }

            ShipMenu.GetComponentsInChildren<Button>().Where
                (o => o.name == "Upgrade").FirstOrDefault().interactable = false;

            if(shipHealth.healthCount == shipHealth.healthMax)
            {
                ShipMenu.GetComponentsInChildren<Button>().Where
                    (o => o.name == "Repair").FirstOrDefault().interactable = false;
            }
            else if(shipHealth.healthCount != shipHealth.healthMax)
            {
                ShipMenu.GetComponentsInChildren<Button>().Where
                    (o => o.name == "Repair").FirstOrDefault().interactable = true;
            }

            foreach (Transform child in ShipMenu.GetComponentsInChildren<Transform>())
            {
                if(child.tag == "ShipyardShip")
                {
                    for(int i = 0, j = child.transform.childCount-1; i < child.transform.childCount; i++, j--)
                    {
                        switch(i)
                        {
                            case 0:
                                child.GetChild(1).GetComponent<Image>().sprite = SelectedShip.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                                break;
                            case 1:
                                child.GetChild(3).GetComponent<Image>().sprite = SelectedShip.transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                                break;
                            case 2:
                                child.GetChild(0).GetComponent<Image>().sprite = SelectedShip.transform.GetChild(2).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                                break;
                            case 3:
                                child.GetChild(2).GetComponent<Image>().sprite = SelectedShip.transform.GetChild(3).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                                break;
                        }
                    }
                }
                Button sellButton = controller.SelectionDisplay.GetComponentsInChildren<Button>().Where
                    (o => o.name == "Sell").FirstOrDefault();

                sellButton.onClick.RemoveAllListeners();
                sellButton.onClick.AddListener(delegate { Sell(controller.GenerateSellPrice(SelectedShip)); });
                if (child.tag == "RepairButton")
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

                healthbar = ShipMenu.GetComponentsInChildren<RectTransform>().Where
                    (o => o.name == "HealthBar").FirstOrDefault();
                healthSlider = healthbar.GetComponentInChildren<Slider>();

                healthSlider.minValue = 0;
                healthSlider.maxValue = shipHealth.healthMax;
                healthSlider.value = shipHealth.healthCount;
            }
        }
        else
        {
            ShopMenu.SetActive(true);
            ShipMenu.SetActive(false);
            SelecionDisplay.SetActive(true);
            controller.GetSelectionInfo(true, controller.ShopShips[controller.scrollSnap.CurrentPage()]);
            controller.CheckIfSpecial(controller.ShopShips[controller.scrollSnap.CurrentPage()]);
            controller.OpenSelectedPanel(0);
        }

    }

    void Sell(int sellPrice)
    {
        IsSlotFilled = false;

        ShipMenu.SetActive(false);
        SelecionDisplay.SetActive(false);

        controller.Ships.Remove(SelectedShip);
        controller.Ships.Add(null);
        controller.MotherShip.RemoveBodyPart(SelectedShip, true);
        controller.MotherShip.bodyPartObjects.Add(null);
        controller.MotherShip.bodyPartTransforms.Add(null);
        controller.MotherShip.SortBody();
        controller.AddToShop(SelectedShip);
        controller.ShipyardShipSetup();
        controller.ShipyardShopSetup();

        //PLACE MONEY ADDING HERE
        WorldManager.Instance.PlayerController.AddMoney(sellPrice);
    }

    void Repair()
    {
        //SET SHIP HEALTH TO MAXIMUM
        healthSlider.value = shipHealth.healthCount;

        float repairCostPerHP = 1f;
        float hpToRestore = 0f;
        hpToRestore += shipHealth.healthMax - shipHealth.healthCount;

        if (WorldManager.Instance.PlayerController.RemoveMoney((int)(hpToRestore * repairCostPerHP)))
        {
            shipHealth.healthCount = shipHealth.healthMax;
            healthSlider.value = shipHealth.healthCount;
        }

        ShipMenu.GetComponentsInChildren<Button>().Where
            (o => o.name == "Repair").FirstOrDefault().interactable = false;


        controller.GetSelectionInfo(false, SelectedShip);

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
