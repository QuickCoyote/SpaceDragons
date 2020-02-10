﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShipData", menuName ="ScriptableObjects/ShipScriptableObject", order = 2)]

public class ShipData : ShipDataParent
{
    public int price;
    public GameObject prefab;
    public eTurretRarity rarity;
    public eTurretType type;
    public string description;
    public string shipName;
    public bool isSpecial;
    public string specialDesc;
    public float specialStat;

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
