using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TutorialPromptScriptableObject", order = 1)]
public class TutorialPromptData : ScriptableObject
{
    public string PromptText;
    public Vector3 UIWaypoint;
    public Vector3 ArrowWaypoint;
    public Vector3 ArrowRotationWaypoint;
}
