using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class OutpostController : MonoBehaviour
{
    public GameObject Outpost;
    public GameObject OutpostContent;
    public GameObject PlayerContent;
    public GameObject ShoppingPanel;
    public Inventory PlayerInventory;
    public List<ItemData> Items;
    public GameObject ItemLayoutPrefab;
    public TextMeshProUGUI ShopTimer;
    [Range(0, 2)] public int ShopDifficulty;

    public AnimationCurve ItemRarityCurve;
    public AnimationCurve DemandCurve;

    ItemData selectedItem;

    Inventory outpostInventory = new Inventory();
    List<int> numsGenerated = new List<int>();
    int sliderValue;
    float Timer = 0;
    float MaxTime = 300;

    int itemBaseCost = 10;
    int price = 0;
    bool selling = false;

    public void Start()
    {
        PlayerInventory = WorldManager.Instance.Ship.GetComponent<Inventory>();

        OutpostShopSetup();
        PlayerShopSetup();
        ShoppingPanel.SetActive(false);
        Outpost.SetActive(false);
    }

    public void Update()
    {
        #region Dev Debug Controls
        if (Input.GetKeyDown(KeyCode.F2))
        {
            OpenOutpost();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOutpost();
        }
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            OutpostShopSetup();
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
            OutpostShopSetup();
        }

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
        numsGenerated = new List<int>();

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

    }

    public void CompleteSale(bool isSelling, ItemData item)
    {
        int price = 0;

        if (isSelling)
        {
            WorldManager.Instance.PlayerController.AddMoney(price);
            outpostInventory.AddItem(item, sliderValue);
            outpostInventory.UpdateInventory();
            PlayerInventory.RemoveItem(item, sliderValue);
            PlayerInventory.UpdateInventory();

            ShoppingPanel.SetActive(false);
            Refresh();
        }
        else
        {
            if (WorldManager.Instance.PlayerController.RemoveMoney(price))
            {
                outpostInventory.RemoveItem(item, sliderValue);
                outpostInventory.UpdateInventory();
                PlayerInventory.AddItem(item, sliderValue);
                PlayerInventory.UpdateInventory();
                ShoppingPanel.SetActive(false);
                Refresh();
            }
            else
            {
                // YOU'RE BROKE >:C STOP TRYING TO BUY THIS
            }
        }
    }

    public void OpenOutpost()
    {
        Outpost.SetActive(true);
        Time.timeScale = 0;
        PlayerShopSetup();
    }

    public void CloseOutpost()
    {
        Outpost.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpenOutpost();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CloseOutpost();
        }
    }

    public int CheckForAmount(ItemData item)
    {
        return outpostInventory.items[item];
    }

    public int CalculateSellPrice(ItemData item)
    {
        price = 0;

        for (int i = 0; i < sliderValue; i++)
        {
            //price += ((ShopDifficulty * itemBaseCost) + (int)(Mathf.Pow(itemBaseCost, ((int)item.rarity)/3)) / (CheckForAmount(item)+i+1));
            price += (int)(ItemRarityCurve.Evaluate((float)((int)item.rarity/5)) * DemandCurve.Evaluate(CheckForAmount(item)/1000));
        }
        
        return price;
    }

    public int CalculateBuyPrice(ItemData item)
    {
        price = 0;

        return price;
    }
}
