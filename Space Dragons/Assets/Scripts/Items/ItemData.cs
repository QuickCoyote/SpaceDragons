using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemData : ScriptableObject
{ 
    public string itemID;
    public string itemName;
    public string description;
    public Sprite itemImage;
}
