using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShipData", menuName ="ScriptableObjects/ShipScriptableObject", order = 2)]
public class ShipData : ScriptableObject
{
    public Sprite[] spriteBases;
    public Sprite[] spriteTurrets;
    public Sprite[] spriteWings;
    public Sprite[] spriteBadgesCommon;
    public Sprite[] spriteBadgesRare;
    public Sprite[] spriteBadgesEpic;
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
