using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : Singleton<HealthBarManager>
{
    [SerializeField] GridLayoutGroup layout = null;
    [SerializeField] GameObject healthBarPrefab = null;
    GameObject[] healthBars;
    int numBars = 0;
    int cellWidth = 35;
    int spacingX = 150;
    void Start()
    {
        UpdateHealthBars();
    }

    public void checkForUpdateSizing()
    {
        if (numBars <= 5)
        {
            cellWidth = 35;
            spacingX = 150;
        }
        else if (numBars > 5 && numBars <= 10)
        {
            cellWidth = 17;
            spacingX = 90;
        }
        else if (numBars > 10 && numBars <= 15)
        {
            cellWidth = 12;
            spacingX = 55;
        }

        layout.cellSize = new Vector3(cellWidth, layout.cellSize.y);
        layout.spacing = new Vector3(spacingX, layout.spacing.y);
    }

    public void UpdateHealthBars()
    {
        numBars = WorldManager.Instance.Ship.bodyPartObjects.Count;

        // Destroy every HP Bar
        for (int i = 0; i < layout.transform.childCount; i++)
        {
            Destroy(layout.transform.GetChild(i).gameObject);
        }

        // go through each of the bodyPartObjects inside the player and create a hp bar
        for (int i = 1; i < numBars; i++)
        {
            if (WorldManager.Instance.Ship.bodyPartObjects[i] != null)
            {
                Instantiate(healthBarPrefab, layout.transform);
                GameObject healthBarOBJ = layout.transform.GetChild(i - 1).gameObject;
                WorldManager.Instance.Ship.bodyPartObjects[i].GetComponent<Health>().healthbarObj = healthBarOBJ;
                checkForUpdateSizing();
            }
        }
    }
}
