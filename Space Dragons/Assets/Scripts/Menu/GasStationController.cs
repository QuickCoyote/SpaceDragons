using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GasStationController : UIBaseClass
{
    [Header("Fuel Bars")]
    public Slider StationFuel;
    public GameObject StationContent;
    public Slider PlayerFuel;
    public GameObject PlayerContent;
    public GameObject CountMarker;

    [Header("Buttons")]
    public Button FullRefillButton;
    public Button SegmentRefillButton;
    public Button UpgradeButton;

    [Header("ReadOuts")]
    public TextMeshProUGUI playerMoneyReadout;
    public TextMeshProUGUI FullPriceReadout;
    public TextMeshProUGUI segmentPriceReadout;
    public TextMeshProUGUI upgradePriceReadout;
    public TextMeshProUGUI timerReadout;

    [Header("Values")]
    [Range(0, 2)] public int ShopDifficulty;
    Ship playerShip;
    PlayerController playerController;
    public int segmentPrice = 20;
    [Range(0, 4)] public int upgradeTotal;
    public int upgradePrice = 250;
    public float StockTimer = 0.0f;
    public float ResetStockTimer = 180.0f;
    public int GasCount;

    [Header("Ads")]
    public Sprite[] ads;
    public Image advertDisplay;
    public float adDisplayTimer = 3.0f;

    void Start()
    {
        playerShip = WorldManager.Instance.Ship;
        playerController = WorldManager.Instance.PlayerController;
        StationSetup();
        PlayerSetup();
        UpdateUI();
        advertDisplay.sprite = ads[Random.Range(0, ads.Length)];
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

        if(Input.GetKeyDown(KeyCode.Slash))
        {
            playerShip.boostFuel = playerShip.boostFuelMAX;
        }
        #endregion

        if (UICanvas.activeSelf)
        {
            UpdateUI();
        }

        if (StockTimer > 0)
        { 
            StockTimer -= 1 * Time.unscaledDeltaTime;
        }
        else if (StockTimer <= 0)
        {
            StationSetup();
        }

    }


    #region SetUp and Resets
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
        StockTimer = ResetStockTimer;
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

    #endregion

    #region Button Methods
    public void FullRefuel()
    {
        if (playerShip.boostFuel < playerShip.boostFuelMAX)
        {
            if ((playerShip.boostFuelMAX - playerShip.boostFuel) < GasCount)
            {
                playerController.RemoveMoney((playerShip.boostFuelMAX - playerShip.boostFuel) * segmentPrice);
                AndroidManager.HapticFeedback();

                GasCount -= (playerShip.boostFuelMAX - playerShip.boostFuel);
                playerShip.RefillBoost();
            }
            else
            {
                playerController.RemoveMoney(GasCount * segmentPrice);
                for (int i = 0; i < GasCount; i++)
                {
                    playerShip.boostFuel++;
                }
                GasCount = 0;
                AndroidManager.HapticFeedback();

            }

        }
        UpdateUI();
    }

    public void RefuelSegment()
    {
        if (playerShip.boostFuel < playerShip.boostFuelMAX)
        {
            playerShip.boostFuel++;
            GasCount--;
            playerController.RemoveMoney(segmentPrice);
        }
        UpdateUI();
    }


    public void UpgradeSegment()
    {
        playerShip.boostFuelMAX++;
        playerShip.boostFuel++;
        upgradeTotal--;
        playerController.RemoveMoney(upgradePrice);
        UpdateUI();
        AndroidManager.HapticFeedback();

    }
    #endregion

    #region UI Activation

    public void UpdateUI()
    {
        PlayerSetup();
        StationFuel.value = GasCount;

        int minutes = (int)StockTimer / 60;
        int seconds = (int)StockTimer % 60;
        timerReadout.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        playerMoneyReadout.text = playerController.ReturnMoney();
        segmentPriceReadout.text = "$" + segmentPrice.ToString();
        upgradePriceReadout.text = "$" + upgradePrice.ToString();

        if ((playerShip.boostFuelMAX - playerShip.boostFuel) < GasCount)
        {
            FullPriceReadout.text = "$" + ((playerShip.boostFuelMAX - playerShip.boostFuel) * segmentPrice).ToString();
            FullRefillButton.interactable = ((GasCount > 0) && ((playerShip.boostFuelMAX - playerShip.boostFuel) * segmentPrice) <= playerController.money && (playerShip.boostFuel < playerShip.boostFuelMAX));
        }
        else
        {
            FullPriceReadout.text = "$" + (GasCount * segmentPrice).ToString();
            FullRefillButton.interactable = ((GasCount > 0) && (GasCount * segmentPrice <= playerController.money) && (playerShip.boostFuel < playerShip.boostFuelMAX));
        }
        if (upgradeTotal > 0)
        {
            upgradePriceReadout.text = "$" + upgradePrice.ToString();

        } else
        {
            upgradePriceReadout.text = "--.--";
        }

        SegmentRefillButton.interactable = (GasCount > 0 && segmentPrice <= playerController.money && (playerShip.boostFuel < playerShip.boostFuelMAX));
        UpgradeButton.interactable = (upgradeTotal > 0 && upgradePrice <= playerController.money);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ToggleUI();
        }
    }

    public new void Open()
    {
        base.Open();

        PlayerSetup();
        advertDisplay.sprite = ads[Random.Range(0, ads.Length)];
        UpdateUI();
    }

    #endregion
}
