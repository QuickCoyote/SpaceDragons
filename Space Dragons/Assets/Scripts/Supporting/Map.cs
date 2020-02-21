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
    public GameObject highlightIcon;
    public Vector3 highlightPrevPos = Vector3.zero;
    public Image MiniMapTargetIcon;
    public Image EnemyIcon;

    public LineRenderer linerendererprefab;
    public TextMeshProUGUI shortestdistanceReadout = null;
    public FollowTarget MiniMapFollow = null;

    public Vector3 TargetBeingTracked = Vector3.zero;

    GameObject player;

    float mapX = 1000f;
    float mapY = 1000f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    Vector3 panStart;

    private void Start()
    {
        // Instantiate the Map Tracker if it doesn't already exist.
        player = WorldManager.Instance.Head;
        MiniMapFollow.Target = player;
    }

    private void Update()
    {
        Vector3 direction = Vector3.zero;
        if(highlightIcon.activeSelf)
        {
            //Check where to rotate minimap tracker
            direction = highlightIcon.transform.position - player.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            MiniMapTargetIcon.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            MiniMapTargetIcon.enabled = (direction.magnitude > 10.0f);
            shortestdistanceReadout.text = Mathf.CeilToInt(ReturnDistanceToTracker()).ToString() + "au";
        }
        else
        {
            MiniMapTargetIcon.enabled = false;
        }

        //Check where to rotate enemy tracker
        float closestEnemy = 50000;
        Vector3 enemydirection = Vector3.zero;
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            float distance = Vector3.Distance(e.transform.position, player.transform.position);
            if (distance < closestEnemy)
            {
                closestEnemy = distance;
                enemydirection = e.transform.position - player.transform.position;
            }
        }

        float enemyangle = Mathf.Atan2(enemydirection.y, enemydirection.x) * Mathf.Rad2Deg;
        EnemyIcon.transform.rotation = Quaternion.AngleAxis(enemyangle + 90, Vector3.forward);
        EnemyIcon.enabled = (enemydirection.magnitude > 10.0f);

        // This is panning/zooming on the map

        if (MainMap.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                panStart = mainMapCam.ScreenToWorldPoint(Input.mousePosition);
                SetTracker();
            }
            //if (Input.touchCount == 2)
            //{
            //    Touch touchOne = Input.GetTouch(0);
            //    Touch touchTwo = Input.GetTouch(1);

            //    Vector2 mod = Vector2.zero;

            //    mod.x -= Screen.width / 2;
            //    mod.x += (Screen.width * 0.1469594595f);

            //    mod.y -= Screen.height / 2;
            //    mod.y += (Screen.height * 0.0034722222f);

            //    touchOne.position -= mod;
            //    touchTwo.position -= mod;
              
            //    touchOne.position *= 0.825f;
            //    touchTwo.position *= 0.825f;

            //    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            //    Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

            //    float prevMagnitude = (touchOnePrevPos - touchTwoPrevPos).magnitude;
            //    float curMagnitude = (touchOne.position - touchTwo.position).magnitude;

            //    float difference = curMagnitude - prevMagnitude;

            //    Zoom(difference * 0.125f);
            //}
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
            if (mainMapCam.orthographicSize > 507)
            {
                mainMapCam.orthographicSize = 510;
                GenerateBounds();
                mainMapCam.transform.position = new Vector3(10, -10, mainMapCam.transform.position.z);
            }
        }
        highlightIcon.SetActive(true);
    }

    //public void Zoom(float increment)
    //{
    //    mainMapCam.orthographicSize = Mathf.Clamp(mainMapCam.orthographicSize - increment, 5, 510);
    //    GenerateBounds();
    //}

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
        highlightIcon.transform.position = highlightPrevPos;
        Time.timeScale = 0;
    }

    public void MapClose()
    {
        MainMap.SetActive(false);
        highlightIcon.transform.position = highlightPrevPos;
        Time.timeScale = 1f;
    }

    public void SetTracker()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            var mousePos = Input.mousePosition;
            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;

            //mousePos.x += (Screen.width * (332f / 1919.514f));
            //mousePos.y += (Screen.height * 0.0034722222f);



            //mousePos *= 1.111111111111111111111111111111f;// this is for 16:9 (1.111111111, 16/9)
            mousePos *= 1.3333333333f;// this is for 18:9 (1.333333333, 18/9)

            Vector3 newMousePos = new Vector3(0, 0, -200);
            Vector3 oldMousePos = new Vector3(mousePos.x, mousePos.y, 100);

            Vector3 tempPos;

            Debug.DrawLine(oldMousePos, newMousePos, Color.red, 100, false);

            tempPos = mainMapCam.transform.position + mousePos;
            tempPos.z = -4f;

            highlightPrevPos = highlightIcon.transform.position;
            highlightIcon.transform.position = tempPos;
            highlightIcon.SetActive(true);
        }
    }

    public float ReturnDistanceToTracker()
    {
        return (highlightIcon.transform.position - player.transform.position).magnitude;
    }
}
