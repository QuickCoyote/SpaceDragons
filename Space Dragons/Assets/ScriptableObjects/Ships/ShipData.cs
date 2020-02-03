using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShipData", menuName ="ScriptableObjects/ShipScriptableObject", order = 2)]

public class ShipData : ShipDataParent
{
    public float price;
    public GameObject prefab;
    public eTurretRarity rarity;
    public eTurretType type;
    public string description;
    public string shipName;

    public enum eTurretRarity
    {
        COMMON,
        RARE,
        EPIC
    };

    public enum eTurretType
    {
        RUSTY,
        LIGHTNING,
        FLAME,
        HEALING,
        ATTACK_DRONE
    };
}
