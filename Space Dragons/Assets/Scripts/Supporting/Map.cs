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

    float mapX = 1000f;
    float mapY = 1000f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    public TextMeshProUGUI trackingTypeText;

    Vector3 panStart;

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
        player = WorldManager.Instance.Head;
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

        shortestdistanceReadout.text = (Vector3.Distance((TrackNearest) ? nearestTarget : TargetBeingTracked, player.transform.position)/100).ToString("0.00au");

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
                closestEnemy = Vector3.Distance(e.transform.position, player.transform.position);
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
                TrackButtons.SetActive(false);
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

        // This is panning/zooming on the map

        if (MainMap.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                panStart = mainMapCam.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.touchCount == 2)
            {
                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);

                Vector2 mod = Vector2.zero;

                mod.x -= Screen.width / 2;
                mod.x += (Screen.width * 0.1469594595f);

                mod.y -= Screen.height / 2;
                mod.y += (Screen.height * 0.0034722222f);

                touchOne.position -= mod;
                touchTwo.position -= mod;
                
                touchOne.position *= 0.825f;
                touchTwo.position *= 0.825f;

                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

                float prevMagnitude = (touchOnePrevPos - touchTwoPrevPos).magnitude;
                float curMagnitude = (touchOne.position - touchTwo.position).magnitude;

                float difference = curMagnitude - prevMagnitude;

                Zoom(difference * 0.125f);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 panDirection = panStart - mainMapCam.ScreenToWorldPoint(Input.mousePosition);
                mainMapCam.transform.position += panDirection;
            }

            if(maxX < minX)
            {
                maxX *= -1;
                minX *= -1;
            }
            if (maxY < minY)
            {
                maxY *= -1;
                minY *= -1;
            }
            var v3 = mainMapCam.transform.position;
            v3.x = Mathf.Clamp(v3.x, minX, maxX);
            v3.y = Mathf.Clamp(v3.y, minY, maxY);
            mainMapCam.transform.position = v3;
        }
    }

    public void Zoom(float increment)
    {
        mainMapCam.orthographicSize = Mathf.Clamp(mainMapCam.orthographicSize - increment, 5, 510);
        GenerateBounds();
        if(mainMapCam.orthographicSize > 507)
        {
            mainMapCam.orthographicSize = 510;
            mainMapCam.transform.position = new Vector3(5, 5, mainMapCam.transform.position.z);
        }
    }

    public void GenerateBounds()
    {
        var vertExtent = mainMapCam.orthographicSize;
        minX = vertExtent - mapX / 2.0f;
        maxX = mapX / 2.0f - vertExtent;
        minY = vertExtent - mapY / 2.0f;
        maxY = mapY / 2.0f - vertExtent;
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
        if (trackType >= 3)
        {
            trackType = 0;
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

            mousePos.x += (Screen.width*0.1469594595f);
            mousePos.y += (Screen.height*0.0034722222f);
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
            if (t.transform.position == nearestTarget)
            {
                t.SelectTarget(true);
            }
        }
    }
}
