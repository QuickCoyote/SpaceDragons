using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GasStationController : MonoBehaviour
{
    public Slider StationFuel;
    public GameObject StationContent;
    public Slider PlayerFuel;
    public GameObject PlayerContent;
    public GameObject CountMarker;
    [Range(0, 2)] public int ShopDifficulty;

    Ship playerShip;

    void Start()
    {
        //playerShip = WorldManager.Instance.Ship;
        StationSetup(); 
    }

    public void StationSetup()
    {
        switch (ShopDifficulty)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                {
                    GameObject go = Instantiate(CountMarker);
                    go.transform.SetParent(StationContent.transform);
                    go.transform.localScale = new Vector3(1, 1, 1);
                }
                StationFuel.minValue = 0;
                StationFuel.maxValue = 4;
                StationFuel.value = StationFuel.maxValue;
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F6))
        {
            GameObject go = Instantiate(CountMarker);

            go.transform.SetParent(StationContent.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
            StationFuel.minValue = 0;
            StationFuel.maxValue = StationContent.transform.childCount;
        }
        if(Input.GetKeyDown(KeyCode.F7))
        {
            StationFuel.value++;
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            StationFuel.value--;
        }
    }
}
