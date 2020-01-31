using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Map : Singleton<Map>
{
    public GameObject MainMap;
    public Image TargetIcon;
    public Image EnemyIcon;

    public LineRenderer linerendererprefab;
    public TextMeshProUGUI shortestdistanceReadout = null;
    public FollowTarget MiniMapFollow = null;

    public bool TrackNearest = true;
    public GameObject TrackButtons;

    List<MapTargets> targets = new List<MapTargets>();
    int TargetIndex = 0;
    public Vector3 TargetBeingTracked = Vector3.zero;

    List<LineRenderer> linerenderers = new List<LineRenderer>();
    GameObject player;
    public Vector3 nearestTarget = Vector3.zero;


    private void Start()
    {
        targets = FindObjectsOfType<MapTargets>().ToList();
        foreach (MapTargets g in targets)
        {
            linerenderers.Add(Instantiate(linerendererprefab, transform));
        }
        player = WorldManager.Instance.Player;
        MiniMapFollow.Target = player;

        TargetBeingTracked = targets[0].transform.position;
    }

    public void AddTarget(MapTargets target)
    {
        targets.Add(target);
        linerenderers.Add(Instantiate(linerendererprefab, transform));
    }
    public void RemoveTarget(MapTargets target)
    {
        linerenderers.RemoveAt(targets.IndexOf(target));
        targets.Remove(target);
    }
    private void Update()
    {
        //Check for distances.
        float shortestDistance = 50000;
        for (int i = 0; i < linerenderers.Count; i++)
        {
            if (linerenderers[i] && targets[i])
            {
                linerenderers[i].positionCount = 2;
                linerenderers[i].SetPosition(0,  new Vector3(targets[i].transform.position.x, targets[i].transform.position.y, -4));
                linerenderers[i].SetPosition(1, new Vector3(player.transform.position.x, player.transform.position.y, -4));
                if (Vector3.Distance(player.transform.position, targets[i].transform.position) < shortestDistance)
                {
                    shortestDistance = Vector3.Distance(player.transform.position, targets[i].transform.position);
                    nearestTarget = targets[i].transform.position;
                }
            }
        }


        shortestdistanceReadout.text = Vector3.Distance((TrackNearest) ? nearestTarget : TargetBeingTracked, player.transform.position).ToString("000m");

        //Check where to rotate minimap tracker
        Vector3 direction = (TrackNearest) ? nearestTarget - player.transform.position : TargetBeingTracked - player.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        TargetIcon.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        TargetIcon.enabled=(direction.magnitude > 10.0f);

        //Check where to rotate enemy tracker
        float closestEnemy = 50000;
        Vector3 enemydirection = Vector3.zero;
        foreach(Enemy e in FindObjectsOfType<Enemy>())
        {
            if (Vector3.Distance(e.transform.position, player.transform.position) < closestEnemy)
            {
                enemydirection = e.transform.position - player.transform.position;
            }
        }
        float enemyangle = Mathf.Atan2(enemydirection.y, enemydirection.x) * Mathf.Rad2Deg;
        EnemyIcon.transform.rotation = Quaternion.AngleAxis(enemyangle + 90, Vector3.forward);
        EnemyIcon.enabled = (enemydirection.magnitude > 10.0f);

    }

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


    //UI stuffs
    public void SetTrackNearest(bool trck)
    {
        TrackNearest = trck;
        if (!trck)
        {
            IncrementTrackIndex();
            TrackButtons.SetActive(true);

        }
        else
        {
            TrackButtons.SetActive(false);
            resetTrackers();
        }
    }

    public void IncrementTrackIndex()
    {
        TargetIndex++;
        if (TargetIndex >= targets.Count) TargetIndex = 0;
        resetTrackers();
        targets[TargetIndex].SelectTarget(true);
        TargetBeingTracked = targets[TargetIndex].transform.position;

    }
    public void DecrimentTrackIndex()
    {
        TargetIndex--;
        if (TargetIndex < 0) TargetIndex = targets.Count - 1;
        resetTrackers();
        targets[TargetIndex].SelectTarget(true);
        TargetBeingTracked = targets[TargetIndex].transform.position;
    }

    public void resetTrackers()
    {
        foreach(MapTargets t in targets)
        {

                t.SelectTarget(false);
            
        }
    }


}
