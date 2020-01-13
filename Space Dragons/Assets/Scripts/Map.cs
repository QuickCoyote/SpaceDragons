﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject MainMap;

    public void MapOpen()
    {
        MainMap.SetActive(true);
        Time.timeScale = 0.0000000000001f;
    }

    public void MapClose()
    {
        MainMap.SetActive(false);
        Time.timeScale = 1f;
    }

}
