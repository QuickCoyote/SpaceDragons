using UnityEngine;

public static class GameObjectEx
{
    public static void DrawCircle(this GameObject container, float radius, float lineWidth, float variance, Material mat)
    {
        var segments = 360;
        var line = container.GetComponent<LineRenderer>();
        if (!line)
        {
            line = container.AddComponent<LineRenderer>();
        }

        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;
        line.material = mat;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, -1);
            points[i] += (points[i] - container.transform.position).normalized * variance;
        }

        line.SetPositions(points);
    }
}