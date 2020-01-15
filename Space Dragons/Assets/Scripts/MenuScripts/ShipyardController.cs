using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipyardController : MonoBehaviour
{
    public Ship MotherShip;
    public List<GameObject> Ships;
    public List<GameObject> ShopShips;
    public List<ShipData> CommonShips;
    public List<ShipData> RareShips;
    public List<ShipData> EpicShips;
    [Range(0, 2)] public int ShopDifficulty;
    public GameObject ShipScrollContent;
    public GameObject ShopShipScrollContent;
    public GameObject ShipButtonPrefab;
    public GameObject ShipMenu;
    public GameObject ShopMenu;
    public TextMeshProUGUI ShipCounter;
    public TextMeshProUGUI ShopTimer;
    public GameObject FullOnShipsMessage;

    int NumOfShips;
    float Timer = 0;
    float MaxTime = 300;
    int selectedPurchase = 0;

    private void Start()
    {
        ShipyardShipSetup();
        ShipyardShopSetup();

    }

    private void Update()
    {
        if(Timer > 0)
        {
            int minutes = (int)Timer / 60;
            int seconds = (int)Timer % 60;
            ShopTimer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            //ShopTimer.text = string.Format("{0d1:##}", ShopTimer.text);
            Timer -= 1 * Time.deltaTime;
        }
        else if (Timer <= 0)
        {
            ShopShips = new List<GameObject>();
            ShipyardShopSetup();
        }

        if (Input.GetKeyDown(KeyCode.F3
            ))
        {
            Timer = 5;
        }
    
        ShipCounter.text = NumOfShips + "/" + Ships.Count;
    }

    public void ShipyardShipSetup()
    {
        int size = MotherShip.maxShipsAllowed;
        Ships = new List<GameObject>(size);
        for(int i = 0; i < size; i++)
        {
            if(i+1 < MotherShip.bodyPartObjects.Count && MotherShip.bodyPartObjects[i + 1] != null)
            {
                Ships.Add(MotherShip.bodyPartObjects[i + 1]);
            }
            else
            {
                Ships.Add(null);
            }
        }

        foreach (Transform child in ShipScrollContent.transform)
        {
            Destroy(child.gameObject);
            NumOfShips = 0;
        }

        for (int i = 0; i < Ships.Count; i++)
        {
            GameObject obj = Instantiate(ShipButtonPrefab);
            Button button = obj.GetComponentInChildren<Button>();
            button.gameObject.AddComponent<ShipSelector>();
            ShipSelector selector = button.GetComponent<ShipSelector>();
            selector.ShipMenu = ShipMenu;
            selector.ShopMenu = ShopMenu;
            selector.controller = this;
            if (Ships[i] != null)
            {
                NumOfShips++;
                button.image.sprite = Ships[i].GetComponent<Turret>().spriteRenderer.sprite;
                selector.IsSlotFilled = true;
                selector.SelectedShip = Ships[i];
            }
            button.onClick.AddListener(delegate { selector.OpenMenu(); });
            obj.transform.SetParent(ShipScrollContent.transform);

        }
    }

    public void ShipyardShopSetup()
    {
        foreach (Transform child in ShopShipScrollContent.transform)
        {
            Destroy(child.gameObject);
        }

        if (Timer <= 0)
        {
            if(ShopDifficulty == 0)
            {
                GenerateShopInventory(80, 99);
            }
            else if(ShopDifficulty == 1)
            {
                GenerateShopInventory(40, 95);
            }
            else
            {
                GenerateShopInventory(20, 90);
            }
            Timer = MaxTime;
        }

        for (int i = 0; i < ShopShips.Count; i++)
        {
            GameObject obj = Instantiate(ShipButtonPrefab);
            Button button = obj.GetComponentInChildren<Button>();
            if (ShopShips[i] != null)
            {
                button.image.sprite = ShopShips[i].GetComponent<Turret>().spriteRenderer.sprite;
            }
            obj.transform.SetParent(ShopShipScrollContent.transform);
        }

    }

    private void GenerateShopInventory(int RareProbability, int EpicProbability)
    {
        for (int i = 0; i < 6; i++)
        {
            float randNum = Random.Range(0.0f, 100.0f);
            if(randNum > EpicProbability)
            {
                //Add Random Epic to Shop List
                int rand = Random.Range(0, EpicShips.Count);
                ShipData EpicShip = EpicShips[(int)rand];
                GameObject Ship = CreateShipFromData(EpicShip);

                ShopShips.Add(Ship);
            }
            else if(randNum > RareProbability)
            {
                //Add Random Rare to Shop List
                int rand = Random.Range(0, RareShips.Count);
                ShipData RareShip = RareShips[(int)rand];
                GameObject Ship = CreateShipFromData(RareShip);

                ShopShips.Add(Ship);
            }
            else
            {
                //Add Random Common to Shop List
                int rand = Random.Range(0, CommonShips.Count);
                ShipData CommonShip = CommonShips[(int)rand];
                GameObject Ship = CreateShipFromData(CommonShip);

                ShopShips.Add(Ship);
            }
        }
    }

    public GameObject CreateShipFromData(ShipData data)
    {
        GameObject Ship = data.prefab;
        Turret ShipTurret = Ship.GetComponent<Turret>();
        ShipTurret.spriteRenderer.sprite = data.sprite;
        ShipTurret.price = data.price;
        ShipTurret.spriteRenderer.color = data.color;
        ShipTurret.turretRarity = data.rarity;

        return Ship;
    }

    public void SelectionIncrement()
    {
        selectedPurchase++;
    }

    public void SelectionDecrement()
    {
        selectedPurchase--;
    }

    public void Purchase()
    {
        if(NumOfShips != Ships.Count)
        {
            GameObject purchase = ShopShips[selectedPurchase];
            for (int i = 0; i < Ships.Count; i++)
            {
                if(Ships[i] == null)
                {
                    Ships[i] = purchase;
                    break;
                }
            }
            ShopShips[selectedPurchase] = null;
            ShipyardShipSetup();
            ShipyardShopSetup();
        }
        else
        {
            //Maybe some kind of ERROR message to let the player know they're full on ships
        }
    }

    public void TradeIn()
    {
        //TRADE IN MOTHERSHIP FOR SHIPYARD'S SHIP
    }

    public void Repair()
    {
        //IMPLEMENT MOTHERSHIP REPAIR
    }
}
