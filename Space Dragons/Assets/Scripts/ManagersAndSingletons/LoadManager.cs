using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//C:\Users\Rowan Remy\AppData\LocalLow\Neumont Game Studio\Space Dragons
//https://www.youtube.com/watch?v=BPu3oXma97Y
public class LoadManager : Singleton<LoadManager>
{
    public SaveData saveData;
    string dataFile = "506c6179657244617461.dat"; //PlayerData in hex

    private void Start()
    {
        Load();

    }

    public void Save()
    {
        UpdateSavedData();
        try
        {
            string filePath = Application.persistentDataPath + "/" + dataFile;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            bf.Serialize(file, saveData);
            file.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Error in Saving:" + e.Message);
        }
    }

    public void UpdateSavedData()
    {
        try
        {
            saveData.PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().healthCount;
            saveData.PlayerMoney = FindObjectOfType<PlayerController>().money;
            saveData.motherShipType = FindObjectOfType<Ship>().motherShip;

            //Convert dictionary to a pair array
            saveData.setItemsFromDictionary(FindObjectOfType<Inventory>().items);

            //Convert ships to a serializable array
            List<ShipDataSavable> ships = new List<ShipDataSavable>();
            foreach (GameObject s in FindObjectOfType<Ship>().bodyPartObjects)
            {
                if (s.GetComponent<Turret>() && s.GetComponent<Health>())
                {
                    ShipDataSavable sav = new ShipDataSavable(s.GetComponent<Turret>().data, s.GetComponent<Health>().healthCount);
                    ships.Add(sav);
                }
            }
            saveData.Ships = ships.ToArray();
            saveData.CurrentWave = EnemyWaveManager.Instance.currentWave;
            saveData.CurrentCycle = EnemyWaveManager.Instance.cycleCount;
            saveData.PlayerPosition = new Vec3(FindObjectOfType<Ship>().transform.position);
        }
        catch (Exception e)
        {
            Debug.Log("Error in finding info to save:" + e.Message);
        }
    }

    public void ResetSaveData()
    {
        saveData = new SaveData();
        Debug.Log(saveData);
        try
        {
            string filePath = Application.persistentDataPath + "/" + dataFile;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            bf.Serialize(file, saveData);
            file.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Error in Saving:" + e.Message);
        }
    }

    public void Load()
    {
        string filePath = Application.persistentDataPath + "/" + dataFile;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(filePath))
        {
            try
            {
                FileStream file = File.Open(filePath, FileMode.Open);
                SaveData loaded = (SaveData)bf.Deserialize(file);
                saveData = loaded;
                file.Close();

                if (saveData == null)
                {
                    saveData = new SaveData();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error in Loading:" + e.Message);
            }
        }
        else
        {
            saveData = new SaveData();
        }
    }



    #region Serializable Objects

    [System.Serializable]
    public class SaveData
    {
        public float PlayerHealth;
        public int PlayerMoney;
        public Ship.eMotherShip motherShipType;
        public ItemPair[] items;
        public ShipDataSavable[] Ships;
        public int CurrentWave;
        public int CurrentCycle;
        public Vec3 PlayerPosition;
        public SaveData()
        {
            PlayerHealth = 100;
            PlayerMoney = 100;
            motherShipType = Ship.eMotherShip.BASIC;
            items = new ItemPair[0];
            Ships = new ShipDataSavable[0];
            CurrentWave = 0;
            CurrentCycle = 0;
            PlayerPosition = new Vec3();
        }

        public void setItemsFromDictionary(Dictionary<ItemData, int> itemsDict)
        {
            List<ItemPair> itemsList = new List<ItemPair>();
            foreach (KeyValuePair<ItemData, int> pair in itemsDict)
            {
                itemsList.Add(new ItemPair(pair.Key.itemID, pair.Value));
            }
            items = itemsList.ToArray();
        }

        public Dictionary<ItemData, int> GetItemsAsDictionary()
        {
            Dictionary<ItemData, int> itemReturnable = new Dictionary<ItemData, int>();
            if (items != null)
            {

                foreach (ItemPair i in items)
                {
                    itemReturnable.Add(WorldManager.Instance.GetItemById(i.itemID), i.num);
                }
            }

            return itemReturnable;

        }
    }

    [System.Serializable]
    public class ShipDataSavable
    {
        public string prefabName;
        public ShipData.eTurretRarity rarity;
        public float shipHealth;
        public ShipDataSavable()
        {
            prefabName = null;
            rarity = ShipData.eTurretRarity.COMMON;
            shipHealth = 0;
        }
        public ShipDataSavable(ShipData dat, float hp)
        {
            if (dat)
            {
                prefabName = (dat.prefab).name;
                rarity = dat.rarity;
                shipHealth = hp;
            }
            else
            {
                prefabName = null;
                rarity = ShipData.eTurretRarity.COMMON;
                shipHealth = 0;
            }
        }
    }

    [System.Serializable]
    public class ItemPair
    {
        public string itemID;
        public int num;
        public ItemPair(string dat, int i)
        {
            itemID = dat;
            num = i;
        }
    }

    [System.Serializable]
    public class Vec3
    {
        public float x;
        public float y;
        public float z;
        public Vec3()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public Vec3(Vector3 pos)
        {
            x = pos.x;
            y = pos.y;
            z = pos.z;
        }
        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }
    }
    #endregion
}
