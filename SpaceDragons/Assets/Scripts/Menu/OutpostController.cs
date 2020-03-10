using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class OutpostController : UIBaseClass
{
    public GameObject OutpostContent;
    public GameObject PlayerContent;
    public GameObject ShoppingPanel;
    public GameObject ItemLayoutPrefab;

    public Inventory PlayerInventory;
    public Ship MotherShip;
    public List<ItemData> Items;

    public TextMeshProUGUI ShopTimer;
    public TextMeshProUGUI PlayerMoney;
    [Range(0, 2)] public int ShopDifficulty;

    public AnimationCurve ItemRarityCurve;
    public AnimationCurve DemandCurve;

    Inventory outpostInventory;
    ItemData selectedItem;

    List<int> numsGenerated = new List<int>();

    float Timer = 0;
    float MaxTime = 300;

    int sliderValue;
    int itemBaseCost = 10;
    int price = 0;

    bool selling = false;

    public void Start()
    {
        PlayerInventory = WorldManager.Instance.Ship.GetComponent<Inventory>();
        MotherShip = WorldManager.Instance.Ship;

        OutpostShopSetup();
        PlayerShopSetup();
        ShoppingPanel.SetActive(false);
    }

    public void Update()
    {
        if (Timer > 0)
        {
            int minutes = (int)(Timer *0.01666666666666666f);
            int seconds = (int)Timer % 60;
            ShopTimer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            Timer -= 1 * Time.unscaledDeltaTime;
        }
        else if (Timer <= 0)
        {
            OutpostShopSetup();
        }

        PlayerMoney.text = WorldManager.Instance.PlayerController.ReturnMoney();

        if (ShoppingPanel.activeInHierarchy)
        {
            Slider slider = ShoppingPanel.GetComponentsInChildren<Slider>().Where(o => o.name == "NumSlider").FirstOrDefault();
            ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "Slider Text").FirstOrDefault().text = ((int)slider.value).ToString();
            if(selectedItem)
            {
                if(selling)
                {
                    ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "Price Text").FirstOrDefault().text = "$" + (CalculateSellPrice(selectedItem)).ToString();
                }
                else
                {
                    ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "Price Text").FirstOrDefault().text = "$" + (CalculateBuyPrice(selectedItem)).ToString();
                }
            }
            else
            {
                ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "Price Text").FirstOrDefault().text = "$$$";
            }
            sliderValue = (int)slider.value;
        }
    }

    public void OutpostShopSetup()
    {
        foreach (Transform child in OutpostContent.transform)
        {
            Destroy(child.gameObject);
        }

        outpostInventory = new Inventory();

        numsGenerated.Clear();

        int numOfItems = 1;
        int amountOfEachItem = 1;

        switch (ShopDifficulty)
        {
            case 0:
                numOfItems = Random.Range(4, 14);
                amountOfEachItem = Random.Range(1, 34);
                break;
            case 1:
                numOfItems = Random.Range(16, 25);
                amountOfEachItem = Random.Range(1, 67);
                break;
            case 2:
                numOfItems = Random.Range(28, 37);
                amountOfEachItem = Random.Range(1, 100);
                break;
            default:
                break;
        }

        for (int i = 0; i < numOfItems; i++)
        {
            GameObject obj = Instantiate(ItemLayoutPrefab);
            Button button = obj.GetComponentInChildren<Button>();
            Image buttonImage = button.GetComponent<Image>();
            TextMeshProUGUI itemCount = button.GetComponentInChildren<TextMeshProUGUI>();
            bool isNumBad = true;

            int randItem = Random.Range(0, Items.Count);
            int randNum = Random.Range(1, amountOfEachItem);
            itemCount.text = "x" + randNum;
            do
            {
                if (!numsGenerated.Contains(randItem))
                {
                    numsGenerated.Add(randItem);
                    isNumBad = false;
                }
                else
                {
                    randItem = Random.Range(0, Items.Count);
                }
            } while (isNumBad);
            outpostInventory.AddItem(Items[randItem], randNum);
            buttonImage.sprite = Items[randItem].itemImage;
            outpostInventory.UpdateInventory();
            ItemData item = outpostInventory.inventory[i];
            int numOfItem = outpostInventory.items[item];

            button.onClick.AddListener(delegate { OpenShoppingMenu(false, item, numOfItem); });
            obj.transform.SetParent(OutpostContent.transform);
            obj.gameObject.transform.localScale = new Vector3(1, 1);
        }
        Timer = MaxTime;
    }

    public void OutpostShopRefresh()
    {
        foreach (Transform child in OutpostContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < outpostInventory.inventory.Count; i++)
        {
            GameObject obj = Instantiate(ItemLayoutPrefab);
            Button button = obj.GetComponentInChildren<Button>();
            Image buttonImage = button.GetComponent<Image>();
            TextMeshProUGUI itemCount = button.GetComponentInChildren<TextMeshProUGUI>();

            buttonImage.sprite = outpostInventory.inventory[i].itemImage;
            itemCount.text = "x" + outpostInventory.items[outpostInventory.inventory[i]];
            outpostInventory.UpdateInventory();
            ItemData item = outpostInventory.inventory[i];
            int numOfItem = outpostInventory.items[item];

            button.onClick.AddListener(delegate { OpenShoppingMenu(false, item, numOfItem); });
            obj.transform.SetParent(OutpostContent.transform);
            obj.gameObject.transform.localScale = new Vector3(1, 1);
        }
    }

    public void PlayerShopSetup()
    {
        foreach (Transform child in PlayerContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < PlayerInventory.inventory.Count; i++)
        {
            GameObject obj = Instantiate(ItemLayoutPrefab);
            Button button = obj.GetComponentInChildren<Button>();
            Image buttonImage = button.GetComponent<Image>();
            TextMeshProUGUI itemCount = button.GetComponentInChildren<TextMeshProUGUI>();

            buttonImage.sprite = PlayerInventory.inventory[i].itemImage;
            itemCount.text = "x" + PlayerInventory.items[PlayerInventory.inventory[i]];
            PlayerInventory.UpdateInventory();
            ItemData item = PlayerInventory.inventory[i];
            int numOfItem = PlayerInventory.items[item];

            button.onClick.AddListener(delegate { OpenShoppingMenu(true, item, numOfItem); });
            obj.transform.SetParent(PlayerContent.transform);
            obj.gameObject.transform.localScale = new Vector3(1, 1);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < 2; i++)
        {
            OutpostShopRefresh();
            PlayerShopSetup();
        }
    }

    public void CompleteSale(bool isSelling, ItemData item)
    {
        if (isSelling)
        {
            WorldManager.Instance.PlayerController.AddMoney(CalculateSellPrice(item));
            outpostInventory.AddItem(item, sliderValue);
            outpostInventory.UpdateInventory();
            PlayerInventory.RemoveItem(item, sliderValue);
            PlayerInventory.UpdateInventory();
            AndroidManager.HapticFeedback();


            ShoppingPanel.SetActive(false);
            Refresh();
            selling = true;
        }
        else
        {
            if (WorldManager.Instance.PlayerController.RemoveMoney(CalculateBuyPrice(item)))
            {
                AndroidManager.HapticFeedback();

                outpostInventory.RemoveItem(item, sliderValue);
                outpostInventory.UpdateInventory();
                PlayerInventory.AddItem(item, sliderValue);
                PlayerInventory.UpdateInventory();
                ShoppingPanel.SetActive(false);
                Refresh();
            }
            else
            {
                Debug.Log("YOU'RE BROKE >:C STOP TRYING TO BUY THIS");
            }
            selling = false;
        }
    }

    #region UI
    public new void Open()
    {
        base.Open();
        Refresh();
        AudioManager.Instance.Play("ShopEntrance");
        AudioManager.Instance.StopAll();
        AudioManager.Instance.PlayRandomMusic("Shop");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ToggleUI();
            Open();
        }
    }

    public new void Close()
    {
        base.Close();
        AudioManager.Instance.StopAll();
        AudioManager.Instance.PlayRandomMusic("Battle");
    }

    public void OpenShoppingMenu(bool isSelling, ItemData item, int numOfItem)
    {
        ShoppingPanel.SetActive(true);

        ShoppingPanel.GetComponentsInChildren<Image>().Where(o => o.name == "Item Image").FirstOrDefault().sprite = item.itemImage;
        ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "Item Count").FirstOrDefault().text = "x" + numOfItem;
        ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "Item Description").FirstOrDefault().text = item.description;
        ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "ItemName").FirstOrDefault().text = item.itemName;
        Slider slider = ShoppingPanel.GetComponentsInChildren<Slider>().Where(o => o.name == "NumSlider").FirstOrDefault();
        slider.maxValue = numOfItem;
        slider.minValue = 1;
        slider.value = 0;
        Button button = ShoppingPanel.GetComponentsInChildren<Button>().Where(o => o.name == "SaleButton").FirstOrDefault();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { CompleteSale(isSelling, item); });
        button.GetComponentInChildren<TextMeshProUGUI>().text = isSelling ? "SELL" : "BUY";
        selectedItem = item;
        selling = isSelling;

    }
    #endregion

    #region Money Calculations
    public int CheckForAmount(ItemData item)
    {
        if(outpostInventory.items.ContainsKey(item))
        {
            return outpostInventory.items[item];
        }
        else
        {
            return 0;
        }
    }
    public int CalculateSellPrice(ItemData item)
    {
        float tempPrice = 0;

        for (int i = 0; i < sliderValue; i++)
        {
            tempPrice += (itemBaseCost + (Mathf.Pow(itemBaseCost, ((int)item.rarity) * 0.333333333333333f)) ) / (CheckForAmount(item) + i + 1);
        }

        price = Mathf.FloorToInt(tempPrice * 10);
            
        return price;
    }
    public int CalculateBuyPrice(ItemData item)
    {
        float tempPrice = 0;

        for (int i = 0; i < sliderValue; i++)
        {
            tempPrice += (itemBaseCost + (Mathf.Pow(itemBaseCost, ((int)item.rarity) * 0.333333333333333f)) ) / (CheckForAmount(item) - i + 1);
        }

        price = Mathf.FloorToInt(tempPrice * 22);
        return price;
    }
    #endregion
}
