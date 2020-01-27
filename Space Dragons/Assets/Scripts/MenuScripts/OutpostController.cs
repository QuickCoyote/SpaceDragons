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

    Inventory outpostInventory = new Inventory();
    List<int> numsGenerated = new List<int>();

    public void Start()
    {
        PlayerInventory = WorldManager.Instance.Ship.GetComponent<Inventory>();
        
        OutpostShopSetup();
        PlayerShopSetup();
        Outpost.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            OpenOutpost();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOutpost();
        }
        else if(Input.GetKeyDown(KeyCode.F12))
        {
            OutpostShopSetup();
        }

        if(ShoppingPanel.activeInHierarchy)
        {
            Slider slider = ShoppingPanel.GetComponentsInChildren<Slider>().Where(o => o.name == "NumSlider").FirstOrDefault();
            ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "Slider Text").FirstOrDefault().text = (int)slider.value + " - " + "$" + 20;

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
        for (int i = 0; i < 16; i++)
        {
            GameObject obj = Instantiate(ItemLayoutPrefab);
            Button button = obj.GetComponentInChildren<Button>();
            Image buttonImage = button.GetComponent<Image>();
            TextMeshProUGUI itemCount = button.GetComponentInChildren<TextMeshProUGUI>();
            bool isNumBad = true;

            int randItem = Random.Range(0, Items.Count);
            int randNum = Random.Range(1, 100);
            itemCount.text = "x" + randNum;
            do
            {
                if(!numsGenerated.Contains(randItem))
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

    public void OpenShoppingMenu(bool isSelling, ItemData item, int numOfItem)
    {
        ShoppingPanel.SetActive(true);

        ShoppingPanel.GetComponentsInChildren<Image>().Where(o => o.name == "Item Image").FirstOrDefault().sprite = item.itemImage;
        ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "Item Count").FirstOrDefault().text = "x"+ numOfItem;
        ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "Item Description").FirstOrDefault().text = item.description;
        ShoppingPanel.GetComponentsInChildren<TextMeshProUGUI>().Where(o => o.name == "ItemName").FirstOrDefault().text = item.itemName;
        ShoppingPanel.GetComponentsInChildren<Slider>().Where(o => o.name == "NumSlider").FirstOrDefault().maxValue = numOfItem;


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

}
