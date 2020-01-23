using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Transform target = null;

    LineRenderer lr = null;

    void Start()
    {
        gameObject.AddComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        Debug.Log("Drawin it >:D");
        lr.material = WorldManager.Instance.lightningMat;
        lr.startWidth = 0.15f;
        lr.endWidth = 0.15f;
        lr.positionCount = 10;
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 95;

        Vector3 TotalVector = target.position - transform.position;

        Vector3[] Segments = new Vector3[10];

        for (int i = 0; i < 10; i++)
        {
            Vector3 normalized = TotalVector/10;
            float rand = Random.Range(-1.0f, 1.0f);
            Segments[i] = normalized * (i+1) + (new Vector3(normalized.y, normalized.x, 0) * rand);
            Debug.Log(Segments[i]);
        }

        lr.SetPositions(Segments);
        lr.enabled = true;
    }

    public void RemoveLightning()
    {
        Destroy(lr);
        Destroy(this);
    }
}
