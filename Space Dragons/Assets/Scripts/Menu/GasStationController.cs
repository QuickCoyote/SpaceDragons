using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GasStationController : MonoBehaviour
{
    public GameObject GasStationCanvas;
    public Slider StationFuel;
    public GameObject StationContent;
    public Slider PlayerFuel;
    public GameObject PlayerContent;
    public GameObject CountMarker;
    [Range(0, 2)] public int ShopDifficulty;

    public Ship playerShip;
    int GasCount;

    void Start()
    {
        playerShip = WorldManager.Instance.Ship;
        StationSetup();
        PlayerSetup();
        GasStationCanvas.SetActive(false);
    }

    void Update()
    {
        #region Dev Tools
        if (Input.GetKeyDown(KeyCode.F7))
        {
            StationFuel.value++;
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            StationFuel.value--;
        }
        if(Input.GetKeyDown(KeyCode.F5))
        {
            OpenGasStation();
        }
        else if(Input.GetKeyDown(KeyCode.F6))
        {
            CloseGasStation();
        }

        if(Input.GetKeyDown(KeyCode.Slash))
        {
            playerShip.boostFuel = playerShip.boostFuelMAX;
        }
        #endregion


    }

    public void StationSetup()
    {
        switch (ShopDifficulty)
        {
            case 0:
                StationGasCreate(4);
                break;
            case 1:
                StationGasCreate(8);
                break;
            case 2:
                StationGasCreate(12);
                break;
            default:
                break;
        }
    }

    void StationGasCreate(int num)
    {
        foreach (Transform child in StationContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < num; i++)
        {
            GameObject go = Instantiate(CountMarker);
            go.transform.SetParent(StationContent.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
        }
        StationFuel.minValue = 0;
        StationFuel.maxValue = num;
        StationFuel.value = StationFuel.maxValue;
        GasCount = num;
    }

    public void PlayerSetup()
    {
        foreach (Transform child in PlayerContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < playerShip.boostFuelMAX; i++)
        {
            GameObject go = Instantiate(CountMarker);
            go.transform.SetParent(PlayerContent.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
        }
        PlayerFuel.minValue = 0;
        PlayerFuel.maxValue = playerShip.boostFuelMAX;
        PlayerFuel.value = playerShip.boostFuel;    
    }

    public void FullRefuel()
    {
        for (int i = playerShip.boostFuel; i < playerShip.boostFuelMAX; i++)
        {
            playerShip.boostFuel++;
            PlayerFuel.value++;
            GasCount--;
            StationFuel.value--;

        }
    }

    public void OpenGasStation()
    {
        GasStationCanvas.SetActive(true);
        PlayerSetup();
        Time.timeScale = 0;
    }

    public void CloseGasStation()
    {
        GasStationCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpenGasStation();
        }
    }

}
