using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Map : Singleton<Map>
{
    public GameObject MainMap;
    public Image TargetIcon;

    public LineRenderer linerendererprefab;
    public TextMeshProUGUI shortestdistanceReadout = null;
    public FollowTarget MiniMapFollow = null;

    ShipyardController[] Shipyards;
    List<LineRenderer> linerenderers = new List<LineRenderer>();
    GameObject player;
    public Vector3 nearestShipyard = Vector3.zero;

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
        MiniMapFollow.Target = player;

    }

    private void Update()
    {
        float shortestDistance = 50000;
        for (int i = 0; i < linerenderers.Count; i++)
        {
            linerenderers[i].positionCount = 2;
            linerenderers[i].SetPosition(0, Shipyards[i].transform.position - Vector3.forward);
            linerenderers[i].SetPosition(1, player.transform.position - Vector3.forward);
            if (Vector3.Distance(player.transform.position, Shipyards[i].transform.position) < shortestDistance)
            {
                shortestDistance = Vector3.Distance(player.transform.position, Shipyards[i].transform.position);
                nearestShipyard = Shipyards[i].transform.position;
            }
        }
        shortestdistanceReadout.text = shortestDistance.ToString("000km");
        Vector3 direction = nearestShipyard - player.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        TargetIcon.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}
