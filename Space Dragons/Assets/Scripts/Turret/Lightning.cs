using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Transform target = null;


    float lifetime = 0.25f;
    float dt = 0;
    LineRenderer lr = null;

    void Start()
    {
        gameObject.AddComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.material = WorldManager.Instance.lightningMat;
        lr.startWidth = 2;
        lr.endWidth = 2;
        lr.positionCount = 10;
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 95;
        lr.startColor = Color.cyan;
        lr.endColor = Color.cyan;

        Vector3 TotalVector = target.position - transform.position;

        Vector3[] Segments = new Vector3[10];

        Segments[0] = transform.position;
        Segments[9] = target.transform.position;
        for (int i = 1; i < 9; i++)
        {
            Vector3 normalized = TotalVector/10;
            float rand = Random.Range(-1.0f, 1.0f);
            Segments[i] = normalized * (i+1) + (new Vector3(normalized.y, normalized.x, 0) * rand) + transform.position;
            Segments[i] = new Vector3(Segments[i].x, Segments[i].y, -1);
            Debug.Log(Segments[i]);
        }

        lr.SetPositions(Segments);
        lr.enabled = true;
    }

    void Update()
    {
        dt += Time.deltaTime;

        if(dt > lifetime)
        {
            RemoveLightning();
        }
    }

    public void RemoveLightning()
    {
        Destroy(lr);
        Destroy(this);
    }
}
