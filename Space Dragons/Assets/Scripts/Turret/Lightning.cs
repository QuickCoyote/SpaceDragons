using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Vector3 target = Vector3.zero;

    bool doneBefore = false;

    float lifetime = 0.25f;
    float dt = 0;
    LineRenderer lr = null;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        if(lr)
        {
        }
        else
        {
            gameObject.AddComponent<LineRenderer>();
            lr = GetComponent<LineRenderer>();
        }
        lr.material = WorldManager.Instance.lightningMat;
        lr.startWidth = 2;
        lr.endWidth = 2;
        lr.positionCount = 10;
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 95;
        lr.startColor = Color.cyan;
        lr.endColor = Color.cyan;

        Vector3 TotalVector = target - transform.position;

        Vector3[] Segments = new Vector3[10];

        Segments[0] = transform.position;
        Segments[9] = target;
        for (int i = 1; i < 9; i++)
        {
            Vector3 normalized = TotalVector * 0.1f;
            float rand = Random.Range(-1.0f, 1.0f);
            Segments[i] = normalized * (i + 1) + (new Vector3(normalized.y, normalized.x, 0) * rand) + transform.position;
            Segments[i] = new Vector3(Segments[i].x, Segments[i].y, -1);
        }

        lr.SetPositions(Segments);
        lr.enabled = true;
        doneBefore = true;
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
        Destroy(GetComponent<Lightning>());
    }
}
