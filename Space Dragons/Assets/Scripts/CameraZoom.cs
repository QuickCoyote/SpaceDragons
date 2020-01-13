using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] Ship player;
    CinemachineTargetGroup targetGroup;

    void Start()
    {
        targetGroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();
    }

    void Update()
    {
        targetGroup.m_Targets = new CinemachineTargetGroup.Target[player.bodyPartTransforms.Count];
        for(int i = 0; i < player.bodyPartTransforms.Count; i++)
        {
            targetGroup.m_Targets[i].target = player.bodyPartTransforms[i].transform;
            targetGroup.m_Targets[i].weight = 1f;
            targetGroup.m_Targets[i].radius = player.bodyPartObjects[i].GetComponentInChildren<SpriteRenderer>().sprite.bounds.max.magnitude;
        }
    }
}