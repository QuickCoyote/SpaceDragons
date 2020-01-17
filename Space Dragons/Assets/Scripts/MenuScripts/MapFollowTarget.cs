using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFollowTarget : MonoBehaviour
{
    public Vector3 offset = new Vector3(0.0f, 0.0f, -10.0f);
    public GameObject Target;
    public Camera MapCam;

    public void Update()
    {
        RaycastHit hit;
        Ray ray = MapCam.ScreenPointToRay(Target.transform.position);

        if(Physics.Raycast(ray, out hit))
        {
            this.GetComponent<RectTransform>().position = hit.transform.position;
        }
    }
}
