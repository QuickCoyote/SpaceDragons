using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : Singleton<HealthBarManager>
{
    public GridLayoutGroup layout = null;
    public GameObject HealthBarParent = null;

    [SerializeField] GameObject healthBarPrefab = null;

    void Start()
    {
        CreateAllHealthBars();
    }

    public void CreateAllHealthBars()
    {
        RemoveAllHealthBars();
        for (int i = 1; i < WorldManager.Instance.Ship.bodyPartObjects.Count; i++)
        {
            if (WorldManager.Instance.Ship.bodyPartObjects[i] != null)
            {
                GameObject HealthBar = Instantiate(healthBarPrefab, HealthBarParent.transform);
                WorldManager.Instance.Ship.bodyPartObjects[i].GetComponent<Health>().healthbarObj = HealthBar;
            }
        }
    }
    public void RemoveAllHealthBars()
    {
        foreach (Transform child in HealthBarParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
