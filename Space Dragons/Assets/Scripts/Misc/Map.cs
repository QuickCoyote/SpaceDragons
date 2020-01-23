using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject MainMap;
    public GameObject PlayerIcon;
    public LineRenderer linerendererprefab;
    public TextMeshProUGUI shortestdistanceReadout = null;

    ShipyardController[] Shipyards;
    List<LineRenderer> linerenderers = new List<LineRenderer>();
    GameObject player;

    public void MapOpen()
    {
        MainMap.SetActive(true);
        Time.timeScale = 0;
    }

    public void MapClose()
    {
        MainMap.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Start()
    {
       Shipyards = FindObjectsOfType<ShipyardController>();
        foreach (ShipyardController g in Shipyards)
        {
            linerenderers.Add(Instantiate(linerendererprefab, transform));
        }
        player = WorldManager.Instance.Player;
    }

    private void Update()
    {
        float shortestDistance = 50000;
        for (int i  = 0; i < linerenderers.Count; i++)
        {
            linerenderers[i].positionCount = 2;
            linerenderers[i].SetPosition(0, Shipyards[i].transform.position - Vector3.forward); 
            linerenderers[i].SetPosition(1, player.transform.position - Vector3.forward);
            if (Vector3.Distance(player.transform.position, Shipyards[i].transform.position) < shortestDistance) shortestDistance = Vector3.Distance(player.transform.position, Shipyards[i].transform.position);

        }
        shortestdistanceReadout.text = shortestDistance.ToString("000km");
    }

}
