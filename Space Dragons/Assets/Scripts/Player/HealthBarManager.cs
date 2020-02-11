using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : Singleton<HealthBarManager>
{
    [SerializeField] public GridLayoutGroup layout = null;
    [SerializeField] GameObject healthBarPrefab = null;
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

    public void CreateAllHealthBars()
    {
        for (int i = 1; i < WorldManager.Instance.Ship.bodyPartObjects.Count; i++)
        {
            if (WorldManager.Instance.Ship.bodyPartObjects[i] != null)
            {
                Instantiate(healthBarPrefab, layout.transform);
                numBars++;
            }
        }
    }
    public void RemoveAllHealthBars()
    {
        foreach (Transform child in layout.transform)
        {
            Destroy(child.gameObject);
            numBars--;
        }
    }

    public void UpdateHealthBars()
    {
        RemoveAllHealthBars();
        CreateAllHealthBars();

        for (int i = 0; i < numBars; i++)
        {
            WorldManager.Instance.Ship.bodyPartObjects[i+1].GetComponent<Health>().healthbarObj = layout.transform.GetChild(i).gameObject;
        }
    }
}
