using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrackingManager : Singleton<TrackingManager>
{
    public MainMapController MainMap;

    [Header("Minimap")]
    public Image MiniMapTargetIcon;
    public Image EnemyIcon;
    public FollowTarget MiniMapFollow = null;
    public Vector3 TargetBeingTracked = Vector3.zero;
    
    GameObject player;

    private void Start()
    {
        // Instantiate the Map Tracker if it doesn't already exist.
        player = WorldManager.Instance.Head;
        MiniMapFollow.Target = player;
    }

    private void Update()
    {
        Vector3 direction = Vector3.zero;
        if(MainMap.highlightIcon.activeSelf)
        {
            //Check where to rotate minimap tracker
            direction = MainMap.highlightIcon.transform.position - player.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            MiniMapTargetIcon.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            MiniMapTargetIcon.enabled = (direction.magnitude > 10.0f);
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
    }

    public float ReturnDistanceToTracker()
    {
        return (MainMap.highlightIcon.transform.position - player.transform.position).magnitude;
    }
}
