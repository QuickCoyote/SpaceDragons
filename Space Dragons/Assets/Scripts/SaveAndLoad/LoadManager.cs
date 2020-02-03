using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
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
            List<ItemPair> items = new List<ItemPair>();
            foreach (KeyValuePair<ItemData, int> pair in FindObjectOfType<Inventory>().items)
            {
                items.Add(new ItemPair(pair.Key.itemID, pair.Value));
            }
            saveData.items = items.ToArray();

            //Convert ships to a optimized array
            List<ShipDataSavable> ships = new List<ShipDataSavable>();
            foreach (GameObject s in FindObjectOfType<Ship>().bodyPartPrefabs)
            {
                ships.Add(new ShipDataSavable(s.GetComponent<Turret>().data));
            }
            saveData.Ships = ships.ToArray();

            saveData.ShipsHealth = null;
            saveData.CurrentWave = EnemyWaveManager.Instance.currentWave;
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
            }
            catch (Exception e)
            {
                Debug.Log("Error in Loading:" + e.Message);
            }
        }

    }

    #region Serializable Objects

    [System.Serializable]
    public class SaveData
    {
        public float PlayerHealth;
        public float PlayerMoney;
        public Ship.eMotherShip motherShipType;
        public ItemPair[] items;
        public ShipDataSavable[] Ships;
        public float[] ShipsHealth;
        public int CurrentWave;
        public Vec3 PlayerPosition;
        public SaveData()
        {
            PlayerHealth = 100;
            PlayerMoney = 0;
            motherShipType = Ship.eMotherShip.BASIC;
            items = null;
            Ships = null;
            ShipsHealth = null;
            CurrentWave = 0;
            PlayerPosition = null;
        }
    }

    [System.Serializable]
    public class ShipDataSavable
    {
        public float price;
        public string prefabName;
        public ShipData.eTurretRarity rarity;
        public ShipData.eTurretType type;
        public string description;
        public string shipName;
        public ShipDataSavable()
        {
            price = 0;
            prefabName = null;
            rarity = ShipData.eTurretRarity.COMMON;
            type = ShipData.eTurretType.RUSTY;
            description = null;
            shipName = null;
        }
        public ShipDataSavable(ShipData dat)
        {
            if (dat)
            {
                price = dat.price;
                prefabName = PrefabUtility.GetCorrespondingObjectFromSource(dat.prefab).name;
                rarity = dat.rarity;
                type = dat.type;
                description = dat.description;
                shipName = dat.name;
            }
            else
            {
                price = 0;
                prefabName = null;
                rarity = ShipData.eTurretRarity.COMMON;
                type = ShipData.eTurretType.RUSTY;
                description = null;
                shipName = null;
            }
        }
        public ShipData ShipSavableToShipData()
        {
            ShipData ship = new ShipData();
            ship.price = price;
            ship.prefab = Resources.Load("Prefabs/" + prefabName) as GameObject;
            ship.rarity = rarity;
            ship.type = type;
            ship.description = description;
            ship.shipName = shipName;
            return ship;
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
