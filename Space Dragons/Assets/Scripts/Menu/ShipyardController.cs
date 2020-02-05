using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShipyardController : MonoBehaviour
{
    public GameObject Shipyard;
    public Ship MotherShip;
    public List<GameObject> Ships;
    public List<GameObject> ShopShips;
    public List<ShipData> CommonShips;
    public List<ShipData> RareShips;
    public List<ShipData> EpicShips;
    public List<MothershipData> Motherships;
    [Range(0, 2)] public int ShopDifficulty;
    public GameObject ShipScrollContent;
    public GameObject ShopShipScrollContent;
    public GameObject ShipButtonPrefab;
    public GameObject ShipMenu;
    public GameObject ShopMenu;
    public GameObject MothershipMenu;
    public GameObject MothershipDisplay;
    public GameObject SelectionDisplay;
    public TextMeshProUGUI ShipCounter;
    public TextMeshProUGUI ShopTimer;
    public GameObject MaxShipWarning;
    public List<GameObject> SelectionInfoPanels;
    public Ship.eMotherShip TradeInMothership;
    public Sprite EmptySlot;

    public int selectedPurchase = 0;
    int NumOfShips;
    float Timer = 0;
    float MaxTime = 300;
    Button buyButton = null;

    int num = 0;

    public void Start()
    {
        MotherShip = WorldManager.Instance.Ship;
        ShipyardShipSetup();
        ShipyardShopSetup();
        ShipyardMotherSetup();
        ShipMenu.SetActive(false);
        ShopMenu.SetActive(false);
        SelectionDisplay.SetActive(false);
        buyButton = SelectionDisplay.GetComponentsInChildren<Button>().Where
                (o => o.name == "Buy").FirstOrDefault();
        Shipyard.SetActive(false);

    }

    public void Update()
    {
        #region Dev Debug Controls
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Timer = 5;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OpenShop();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
        #endregion

        if (Timer > 0)
        {
            int minutes = (int)Timer / 60;
            int seconds = (int)Timer % 60;
            ShopTimer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            Timer -= 1 * Time.unscaledDeltaTime;
        }
        else if (Timer <= 0)
        {
            ShopShips = new List<GameObject>();
            ShipyardShopSetup();
            int num = 0;
            for (int i = 0; i < SelectionInfoPanels.Count; i++)
            {
                if (SelectionInfoPanels[i].activeInHierarchy)
                {
                    num = i;
                    break;
                }
            }

            OpenSelectedPanel(0);
            GetSelectionInfo(true, ShopShips[selectedPurchase]);
            OpenSelectedPanel(num);
        }

        ShipCounter.text = NumOfShips + "/" + Ships.Count;
    }

    public void ShipyardMotherSetup()
    {
        MothershipMenu.GetComponentsInChildren<Image>().Where
            (o => o.name == "Mothership").FirstOrDefault().sprite = MotherShip.ShipHeadSprite.sprite;

        MothershipDisplay.GetComponentsInChildren<Image>().Where
            (o => o.name == "DisplayMothership").FirstOrDefault().sprite = MotherShip.ShipHeadSprites[(int)TradeInMothership];

        MothershipDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
            (o => o.name == "Type").FirstOrDefault().text = Motherships[(int)TradeInMothership].Title;

        MothershipDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
            (o => o.name == "Description").FirstOrDefault().text = Motherships[(int)TradeInMothership].Description;

    }

    public void ShipyardShipSetup()
    {
        int size = MotherShip.maxShipsAllowed;
        Ships = new List<GameObject>(size);
        for (int i = 0; i < size; i++)
        {
            if (i + 1 < MotherShip.bodyPartObjects.Count && MotherShip.bodyPartObjects[i + 1] != null)
            {
                MotherShip.bodyPartObjects[i + 1].SetActive(true);
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
            selector.SelecionDisplay = SelectionDisplay;
            selector.controller = this;
            if (Ships[i] != null)
            {
                NumOfShips++;
                Turret turret = Ships[i].GetComponent<Turret>();

                for (int j = 1; j < 5; j++)
                {
                    Image buttonChildImage = button.transform.GetChild(j).GetComponent<Image>();

                    switch (j)
                    {
                        case 1:
                            buttonChildImage.sprite = turret.spriteRendererWings.sprite;
                            break;
                        case 2:
                            buttonChildImage.sprite = turret.spriteRendererBase.sprite;
                            break;
                        case 3:
                            buttonChildImage.sprite = turret.spriteRendererBadge.sprite;
                            break;
                        case 4:
                            buttonChildImage.sprite = turret.spriteRendererTurret.sprite;
                            break;
                    }
                    buttonChildImage.color = new Color(buttonChildImage.color.r, buttonChildImage.color.g, buttonChildImage.color.b, 1);
                }

                selector.IsSlotFilled = true;
                selector.SelectedShip = Ships[i];
            }
            button.onClick.AddListener(delegate { selector.OpenMenu(); });
            obj.transform.SetParent(ShipScrollContent.transform);
            obj.gameObject.transform.localScale = new Vector3(1, 1);

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
            if (ShopDifficulty == 0)
            {
                GenerateShopInventory(80, 99);
            }
            else if (ShopDifficulty == 1)
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
                Turret turret = ShopShips[i].GetComponent<Turret>();

                for (int j = 1; j < 5; j++)
                {
                    Image buttonChildImage = button.transform.GetChild(j).GetComponent<Image>();

                    switch (j)
                    {
                        case 1:
                            buttonChildImage.sprite = turret.spriteRendererWings.sprite;
                            break;
                        case 2:
                            buttonChildImage.sprite = turret.spriteRendererBase.sprite;
                            break;
                        case 3:
                            buttonChildImage.sprite = turret.spriteRendererBadge.sprite;
                            break;
                        case 4:
                            buttonChildImage.sprite = turret.spriteRendererTurret.sprite;
                            break;
                    }
                    if (buttonChildImage.sprite != null)
                    {
                        buttonChildImage.color = new Color(buttonChildImage.color.r, buttonChildImage.color.g, buttonChildImage.color.b, 1);
                    }
                }
            }
            obj.transform.SetParent(ShopShipScrollContent.transform);
            obj.gameObject.transform.localScale = new Vector3(1, 1);
        }

    }

    private void GenerateShopInventory(int RareProbability, int EpicProbability)
    {
        for (int i = 0; i < 10; i++)
        {
            float randNum = Random.Range(0.0f, 100.0f);
            if (randNum > EpicProbability)
            {
                //Add Random Epic to Shop List
                int rand = Random.Range(0, EpicShips.Count);
                ShipData EpicShip = EpicShips[rand];
                GameObject Ship = CreateShipFromData(EpicShip);

                ShopShips.Add(Ship);
            }
            else if (randNum > RareProbability)
            {
                //Add Random Rare to Shop List
                int rand = Random.Range(0, RareShips.Count);
                ShipData RareShip = RareShips[rand];
                GameObject Ship = CreateShipFromData(RareShip);

                ShopShips.Add(Ship);
            }
            else
            {
                //Add Random Common to Shop List
                int rand = Random.Range(0, CommonShips.Count);
                ShipData CommonShip = CommonShips[rand];
                GameObject Ship = CreateShipFromData(CommonShip);

                ShopShips.Add(Ship);
            }
        }
    }

    public GameObject CreateShipFromData(ShipData data)
    {
        if (data == null)
        {
            return null;
        }
        GameObject Ship = Instantiate(data.prefab);
        Ship.SetActive(false);
        Turret ShipTurret = Ship.GetComponent<Turret>();

        ShipTurret.data = data;
        int badgeColor = 0;

        Sprite randBase = null;
        Sprite randTurret = null;
        Sprite randWings = null;

        switch (data.type)
        {
            case ShipData.eTurretType.FLAME:
                randBase = data.spriteBasesRed[Random.Range(0, data.spriteBasesRed.Length)];
                randTurret = data.spriteTurretsRed[Random.Range(0, data.spriteTurretsRed.Length)];
                randWings = data.spriteWingsRed[Random.Range(0, data.spriteWingsRed.Length)];
                badgeColor = 0;
                break;
            case ShipData.eTurretType.HEALING:
                randBase = data.spriteBasesGreen[Random.Range(0, data.spriteBasesGreen.Length)];
                randTurret = data.spriteTurretsGreen[Random.Range(0, data.spriteTurretsGreen.Length)];
                randWings = data.spriteWingsGreen[Random.Range(0, data.spriteWingsGreen.Length)];
                badgeColor = 1;
                break;
            case ShipData.eTurretType.LIGHTNING:
                randBase = data.spriteBasesBlue[Random.Range(0, data.spriteBasesBlue.Length)];
                randTurret = data.spriteTurretsBlue[Random.Range(0, data.spriteTurretsBlue.Length)];
                randWings = data.spriteWingsBlue[Random.Range(0, data.spriteWingsBlue.Length)];
                badgeColor = 2;
                break;
            case ShipData.eTurretType.RUSTY:
                randBase = data.spriteBasesOrange[Random.Range(0, data.spriteBasesOrange.Length)];
                randTurret = data.spriteTurretsOrange[Random.Range(0, data.spriteTurretsOrange.Length)];
                randWings = data.spriteWingsOrange[Random.Range(0, data.spriteWingsOrange.Length)];
                badgeColor = 3;
                break;
            case ShipData.eTurretType.ATTACK_DRONE:
                randBase = data.spriteBasesPurple[Random.Range(0, data.spriteBasesPurple.Length)];
                randTurret = data.spriteTurretsPurple[Random.Range(0, data.spriteTurretsPurple.Length)];
                randWings = data.spriteWingsPurple[Random.Range(0, data.spriteWingsPurple.Length)];
                badgeColor = 4;
                break;
        }

        ShipTurret.spriteRendererBase.sprite = randBase;
        ShipTurret.spriteRendererTurret.sprite = randTurret;
        ShipTurret.spriteRendererWings.sprite = randWings;
        switch (data.rarity)
        {
            case ShipData.eTurretRarity.COMMON:
                ShipTurret.spriteRendererBadge.sprite = data.spriteBadgesCommon[badgeColor];
                break;
            case ShipData.eTurretRarity.RARE:
                ShipTurret.spriteRendererBadge.sprite = data.spriteBadgesRare[badgeColor];
                break;
            case ShipData.eTurretRarity.EPIC:
                ShipTurret.spriteRendererBadge.sprite = data.spriteBadgesEpic[badgeColor];
                break;
        }


        ShipTurret.price = data.price;
        ShipTurret.turretRarity = data.rarity;

        return Ship;
    }

    public void OpenSelectedPanel(int num)
    {
        for (int i = 0; i < SelectionInfoPanels.Count; i++)
        {
            if(i == num)
            {
                SelectionInfoPanels[i].SetActive(true);
            }
            else
            {
                SelectionInfoPanels[i].SetActive(false);
            }
        }
    }

    public void GetSelectionInfo(bool isBuying, GameObject selectedShip)
    {
        if (ShopShips[selectedPurchase] != null && isBuying)
        {
            ShipData data = ShopShips[selectedPurchase].GetComponent<Turret>().data;

            SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
                (o => o.name == "Type").FirstOrDefault().text = data.shipName;

            SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
                (o => o.name == "Rarity").FirstOrDefault().text = "Rarity: " + data.rarity;

            SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
                (o => o.name == "Description").FirstOrDefault().text = "Description:\n" + data.description;

            buyButton.gameObject.SetActive(true);

            buyButton.interactable = true;

            GameObject turret = null;
            foreach (Transform child in SelectionDisplay.transform)
            {
                if (child.name == "ShipDisplay")
                {
                    turret = child.gameObject;
                }
            }
            for (int i = 0, j = turret.transform.childCount - 1; i < turret.transform.childCount; i++, j--)
            {
                switch (i)
                {
                    case 0:
                        turret.transform.GetChild(1).GetComponent<Image>().sprite = selectedShip.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        break;
                    case 1:
                        turret.transform.GetChild(3).GetComponent<Image>().sprite = selectedShip.transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        break;
                    case 2:
                        turret.transform.GetChild(0).GetComponent<Image>().sprite = selectedShip.transform.GetChild(2).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        break;
                    case 3:
                        turret.transform.GetChild(2).GetComponent<Image>().sprite = selectedShip.transform.GetChild(3).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        break;
                }
            }
            OpenSelectedPanel(1);
            GetSelectionStats(selectedShip);
            OpenSelectedPanel(0);

        }
        else if (!isBuying)
        {
            ShipData data = null;

            foreach (GameObject obj in Ships)
            {
                if (obj == selectedShip)
                {
                    data = obj.GetComponent<Turret>().data;
                }
            }

            SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
                (o => o.name == "Type").FirstOrDefault().text = data.shipName;

            SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
                (o => o.name == "Rarity").FirstOrDefault().text = "Rarity: " + data.rarity;

            SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
                (o => o.name == "Description").FirstOrDefault().text = "Description:\n" + data.description;

            buyButton.gameObject.SetActive(false);

            GameObject turret = null;
            foreach (Transform child in SelectionDisplay.transform)
            {
                if (child.name == "ShipDisplay")
                {
                    turret = child.gameObject;
                }
            }
            for (int i = 0, j = turret.transform.childCount - 1; i < turret.transform.childCount; i++, j--)
            {
                switch (i)
                {
                    case 0:
                        turret.transform.GetChild(1).GetComponent<Image>().sprite = selectedShip.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        break;
                    case 1:
                        turret.transform.GetChild(3).GetComponent<Image>().sprite = selectedShip.transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        break;
                    case 2:
                        turret.transform.GetChild(0).GetComponent<Image>().sprite = selectedShip.transform.GetChild(2).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        break;
                    case 3:
                        turret.transform.GetChild(2).GetComponent<Image>().sprite = selectedShip.transform.GetChild(3).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        break;
                }
            }
            OpenSelectedPanel(1);
            GetSelectionStats(selectedShip);
            OpenSelectedPanel(0);
        }
        else
        {
            SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
                (o => o.name == "Type").FirstOrDefault().text = "EMPTY";

            SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
                (o => o.name == "Rarity").FirstOrDefault().text = "Rarity: " + "EMPTY";

            SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
                (o => o.name == "Description").FirstOrDefault().text = "Description:\n" + "EMPTY";

            buyButton.gameObject.SetActive(true);

            buyButton.interactable = false;

            GameObject turret = null;
            foreach (Transform child in SelectionDisplay.transform)
            {
                if (child.name == "ShipDisplay")
                {
                    turret = child.gameObject;
                }
            }
            for (int i = 0, j = turret.transform.childCount - 1; i < turret.transform.childCount; i++, j--)
            {
                switch (i)
                {
                    case 0:
                        turret.transform.GetChild(1).GetComponent<Image>().sprite = EmptySlot;
                        break;
                    case 1:
                        turret.transform.GetChild(3).GetComponent<Image>().sprite = EmptySlot;
                        break;
                    case 2:
                        turret.transform.GetChild(0).GetComponent<Image>().sprite = EmptySlot;
                        break;
                    case 3:
                        turret.transform.GetChild(2).GetComponent<Image>().sprite = EmptySlot;
                        break;
                }
            }

        }
    }

    public void GetSelectionStats(GameObject selectedShip)
    {
        SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
            (o => o.name == "FireRateNum").FirstOrDefault().text = (selectedShip.GetComponent<Turret>().attackSpeed).ToString();

        SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
            (o => o.name == "DamageNum").FirstOrDefault().text = (selectedShip.GetComponent<Turret>().damage).ToString();

        SelectionDisplay.GetComponentsInChildren<TextMeshProUGUI>().Where
            (o => o.name == "RangeNum").FirstOrDefault().text = (selectedShip.GetComponent<Turret>().range).ToString();
    }

    public void SelectionIncrement()
    {
        selectedPurchase++;
        int num = 0;
        for (int i = 0; i < SelectionInfoPanels.Count; i++)
        {
            if(SelectionInfoPanels[i].activeInHierarchy)
            {
                num = i;
                break;
            }
        }

        OpenSelectedPanel(0);
        GetSelectionInfo(true, ShopShips[selectedPurchase]);
        OpenSelectedPanel(num);
    }

    public void SelectionDecrement()
    {
        selectedPurchase--;
        int num = 0;
        for (int i = 0; i < SelectionInfoPanels.Count; i++)
        {
            if (SelectionInfoPanels[i].activeInHierarchy)
            {
                num = i;
                break;
            }
        }

        OpenSelectedPanel(0);
        GetSelectionInfo(true, ShopShips[selectedPurchase]);
        OpenSelectedPanel(num);
    }

    public void Purchase()
    {
        if (ShopShips[selectedPurchase] != null)
        {
            if (NumOfShips != Ships.Count)
            {
                GameObject purchase = ShopShips[selectedPurchase];
                for (int i = 0; i < Ships.Count; i++)
                {
                    if (Ships[i] == null)
                    {
                        Ships[i] = purchase;
                        if (WorldManager.Instance.PlayerController.RemoveMoney(purchase.GetComponent<Turret>().data.price))
                        {
                            if (i + 1 < MotherShip.bodyPartObjects.Count)
                            {
                                MotherShip.bodyPartObjects[i + 1] = purchase;
                                MotherShip.SortBody();
                            }
                            else
                            {
                                MotherShip.AddBodyPart(purchase);
                                MotherShip.SortBody();
                            }
                        }
                        else
                        {
                            // YOU DON'T HAVE ENOUGH MONEY >:C
                        }

                        break;
                    }
                }
                ShopShips[selectedPurchase] = null;
                SortShips();
                GetSelectionInfo(true, ShopShips[selectedPurchase]);
                ShipyardShipSetup();
                ShipyardShopSetup();
            }
            else
            {
                //Maybe some kind of ERROR message to let the player know they're full on ships
                MaxShipWarning.SetActive(true);
            }
        }
    }

    public void SortShips()
    {
        for (int i = 1; i < ShopShips.Count; i++)
        {
            if (ShopShips[i - 1] == null)
            {
                ShopShips[i - 1] = ShopShips[i];
                ShopShips[i] = null;
            }
        }
    }

    public void AddToShop(GameObject ship)
    {
        GameObject newShip = Instantiate<GameObject>(ship);
        bool added = false;
        for (int i = 0; i < ShopShips.Count; i++)
        {
            if (ShopShips[i] == null)
            {
                ShopShips[i] = newShip;
                added = true;
                break;
            }
        }
        if (!added)
        {
            ShopShips.Add(newShip);
        }

        newShip.SetActive(false);

    }

    public void TradeIn()
    {
        int tradeInValue = 300;
        if (WorldManager.Instance.PlayerController.RemoveMoney(tradeInValue))
        {
            Ship.eMotherShip temp = MotherShip.motherShip;

            MotherShip.SetShipHead((int)TradeInMothership);

            TradeInMothership = temp;

            ShipyardMotherSetup();
        }
        else
        {
            // You don't have enough money >:C
        }
    }

    public void Repair()
    {
        float repairCostPerHP = 1f;
        float hpToRestore = 0f;
        for (int i = 0; i < MotherShip.bodyPartObjects.Count; i++)
        {
            if (MotherShip.bodyPartObjects[i] != null)
            {
                hpToRestore += MotherShip.bodyPartObjects[i].GetComponent<Health>().healthMax - MotherShip.bodyPartObjects[i].GetComponent<Health>().healthCount;
            }
        }

        if (WorldManager.Instance.PlayerController.RemoveMoney((int)(hpToRestore * repairCostPerHP)))
        {
            for (int i = 0; i < MotherShip.bodyPartObjects.Count; i++)
            {
                if (MotherShip.bodyPartObjects[i] != null)
                {
                    MotherShip.bodyPartObjects[i].GetComponent<Health>().healthCount = MotherShip.bodyPartObjects[i].GetComponent<Health>().healthMax;
                }
            }
        }
    }

    public void CloseMessage()
    {
        MaxShipWarning.SetActive(false);
    }

    public void OpenShop()
    {
        AudioManager.Instance.PlayRandomMusic("Shop Music");
        ShipyardShipSetup();
        Shipyard.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseShop()
    {
        AudioManager.Instance.PlayRandomMusic("Battle Music");
        Shipyard.SetActive(false);
        ShipMenu.SetActive(false);
        ShopMenu.SetActive(false);
        SelectionDisplay.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpenShop();
        }
    }

}
