using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
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

        GenerateStartingHealthBars();
        Debug.Log("Generated Health Bars");
    }
    void Update()
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

    public void GenerateStartingHealthBars()
    {
        for (int i = 1; i <= numBars; i++)
        {
            WorldManager.Instance.Ship.bodyPartObjects[i].GetComponent<Health>().healthbarObj = Instantiate(healthBarPrefab, gameObject.transform);
        }
    }

    public void AddHealthBar()
    {
        numBars++;
        WorldManager.Instance.Ship.bodyPartObjects[numBars].GetComponent<Health>().healthbarObj = Instantiate(healthBarPrefab, gameObject.transform);// Instantiate HealthBarPrefab, connect
    }
}
