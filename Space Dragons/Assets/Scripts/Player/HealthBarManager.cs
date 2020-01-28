using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : Singleton<HealthBarManager>
{
    [SerializeField] GridLayoutGroup layout = null;
    [SerializeField] GameObject healthBarPrefab = null;
    GameObject[] healthBars;
    int numBars = 1;
    int cellWidth = 35;
    int spacingX = 150;
    void Start()
    {
        numBars = WorldManager.Instance.Ship.bodyPartTransforms.Count-1;
    }

    public void AddHealthBar()
    {
        numBars++;
        Instantiate(healthBarPrefab, layout.transform);
        WorldManager.Instance.Ship.bodyPartObjects[numBars-1].GetComponent<Health>().healthbarObj = layout.transform.GetChild(numBars-1).gameObject;
        checkForUpdateSizing();
    }

    public void RemoveHealthBar()
    {
        numBars--;
        Destroy(layout.transform.GetChild(numBars - 2).gameObject);
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
}
