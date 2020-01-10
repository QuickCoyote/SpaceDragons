using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
    public GameObject ShipMenu;
    public GameObject ShopMenu;
    public bool IsSlotFilled = false;

    public void OpenMenu()
    {
        if(IsSlotFilled)
        {
            ShipMenu.SetActive(true);
            ShopMenu.SetActive(false);
        }
        else
        {
            ShopMenu.SetActive(true);
            ShipMenu.SetActive(false);
        }

    }

}
