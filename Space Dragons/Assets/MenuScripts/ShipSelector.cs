using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
    public GameObject ShipMenu;


    public void OpenMenu()
    {
        ShipMenu.SetActive(!ShipMenu.activeSelf);

    }

}
