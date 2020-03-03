using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] float offset = 5f;
    Ship ship;
    CinemachineTargetGroup targetGroup;

    void Start()
    {
        ship = WorldManager.Instance.Ship;
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void FixedUpdate()
    {
        StartCoroutine("SetZoom");
    }

    IEnumerator SetZoom()
    {
        targetGroup.m_Targets = new CinemachineTargetGroup.Target[ship.bodyPartTransforms.Count];
        offset = ship.bodyPartTransforms.Count * 2;
        for (int i = 0; i < ship.bodyPartTransforms.Count; i++)
        {
            if (ship.bodyPartTransforms[i] != null)
            {
                targetGroup.m_Targets[i].target = ship.bodyPartTransforms[i].transform;
                targetGroup.m_Targets[i].weight = 1f;
                targetGroup.m_Targets[i].radius = ship.bodyPartObjects[i].GetComponentInChildren<SpriteRenderer>().sprite.bounds.max.magnitude + offset;
            }
        }

        yield return new WaitForSeconds(5f);
    }
}