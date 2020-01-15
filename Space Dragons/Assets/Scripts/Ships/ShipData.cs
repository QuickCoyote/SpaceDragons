using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShipData", menuName ="ScriptableObjects/ShipScriptableObject", order = 2)]
public class ShipData : ScriptableObject
{
    public Sprite sprite;
    public float price;
    public GameObject prefab;
    public Color color;
    public eTurretRarity rarity;
    public enum eTurretRarity
    {
        COMMON,
        RARE,
        EPIC
    };

}
