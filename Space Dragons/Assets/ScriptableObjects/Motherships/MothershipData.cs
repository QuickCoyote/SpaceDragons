using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MothershipData", menuName = "ScriptableObjects/MothershipScriptableObject", order = 3)]

public class MothershipData : ScriptableObject
{
    public string Title;
    public string Description;
    public eMotherShip mothership;

    public enum eMotherShip
    {
        BASIC = 0,
        FLAMETHROWER = 1,
        LIGHTNING = 2,
        HEALING = 3,
        GUARD_DRONE = 4,
        LASER = 5
    };

}
