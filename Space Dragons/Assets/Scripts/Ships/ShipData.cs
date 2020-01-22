using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShipData", menuName ="ScriptableObjects/ShipScriptableObject", order = 2)]
public class ShipData : ShipDataParent
{
    public float price;
    public GameObject prefab;
    public eTurretRarity rarity;
    public enum eTurretRarity
    {
        COMMON,
        RARE,
        EPIC
    };
}
