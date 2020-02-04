using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Map : Singleton<Map>
{
    public Camera mainMapCam = null;

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

    public eTrackingType trackingType = eTrackingType.TRACK_NEAREST;
    int trackType = 0;

    public TextMeshProUGUI trackingTypeText;

    public enum eTrackingType
    {
        TAP_TRACKING,
        TRACK_NEAREST,
        TRACK_SWAPPING
    }


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
                linerenderers[i].SetPosition(0, new Vector3(targets[i].transform.position.x, targets[i].transform.position.y, -4));
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
        TargetIcon.enabled = (direction.magnitude > 10.0f);

        //Check where to rotate enemy tracker
        float closestEnemy = 50000;
        Vector3 enemydirection = Vector3.zero;
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (Vector3.Distance(e.transform.position, player.transform.position) < closestEnemy)
            {
                enemydirection = e.transform.position - player.transform.position;
            }
        }
        float enemyangle = Mathf.Atan2(enemydirection.y, enemydirection.x) * Mathf.Rad2Deg;
        EnemyIcon.transform.rotation = Quaternion.AngleAxis(enemyangle + 90, Vector3.forward);
        EnemyIcon.enabled = (enemydirection.magnitude > 10.0f);

        if (MainMap.activeSelf)
        {
            if (trackingType == eTrackingType.TAP_TRACKING)
            {
                CheckTrackedByTouch();
            }
            if (trackingType == eTrackingType.TRACK_NEAREST)
            {
                TrackButtons.SetActive(false);
                resetTrackers();
                TargetBeingTracked = nearestTarget;
                CheckMapIconActivation();
            }
            if (trackingType == eTrackingType.TRACK_SWAPPING)
            {
                TrackButtons.SetActive(true);
            }
        }
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

    public void NextTrackingType()
    {
        trackType++;
        if(trackType >= 3)
        {
            trackType = 0;
        }
        switch(trackType)
        {
            case 0:
                trackingType = eTrackingType.TRACK_NEAREST;
                trackingTypeText.text = "Track Nearest";
                break;
            case 1:
                trackingType = eTrackingType.TRACK_SWAPPING;
                trackingTypeText.text = "Track Swapping";
                break;
            case 2:
                trackingType = eTrackingType.TAP_TRACKING;
                trackingTypeText.text = "Tap Tracking";
                break;
        }
    }
    public void PrevTrackingType()
    {
        trackType++;
        if (trackType <= -1)
        {
            trackType = 2;
        }
        switch (trackType)
        {
            case 0:
                trackingType = eTrackingType.TRACK_NEAREST;
                trackingTypeText.text = "Track Nearest";
                break;
            case 1:
                trackingType = eTrackingType.TRACK_SWAPPING;
                trackingTypeText.text = "Track Swapping";
                break;
            case 2:
                trackingType = eTrackingType.TAP_TRACKING;
                trackingTypeText.text = "Tap Tracking";
                break;
        }
    }

    public void CheckTrackedByTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            TrackNearest = false;

            var mousePos = Input.mousePosition;
            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;

            mousePos.x += 435;
            mousePos.y += 5;
            mousePos *= 0.825f;

            Vector3 newMousePos = new Vector3(0, 0, -200);
            Vector3 oldMousePos = new Vector3(mousePos.x, mousePos.y, 100);

            Vector3 tempPos = new Vector3(mainMapCam.transform.position.x, mainMapCam.transform.position.y, 1000f);
            Debug.DrawLine(oldMousePos, newMousePos, Color.red, 100, false);
            RaycastHit2D[] hits = Physics2D.RaycastAll(oldMousePos, oldMousePos - newMousePos, 10000f, ~9);

            resetTrackers();

            foreach (RaycastHit2D result in hits)
            {
                if (result.collider.gameObject.layer == 9)
                {
                    result.transform.parent.gameObject.GetComponent<MapTargets>().SelectTarget(true);
                    TargetBeingTracked = result.transform.position;
                    return;
                }
            }
        }
    }

    public void resetTrackers()
    {
        foreach (MapTargets t in targets)
        {
            t.SelectTarget(false);
        }
    }

    public void CheckMapIconActivation()
    {
        foreach (MapTargets t in targets)
        {
            if(t.transform.position == nearestTarget)
            {
                t.SelectTarget(true);
            }
        }
    }
}
