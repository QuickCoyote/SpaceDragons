using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    List<Health> objectsToDealDamageTo = new List<Health>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = null;

        collision.TryGetComponent(out health);

        if (health)
        {
            if (health != transform.parent.GetComponent<Health>())
            {
                objectsToDealDamageTo.Add(health);
            }
        }
    }

    private void Update()
    {
        Vector3[] points = new Vector3[2];

        points[0] = new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y, -1);
        Vector3 newUp = transform.parent.transform.up * transform.parent.GetComponent<HumanBossEnemy>().LaserFireDistance;
        points[1] = new Vector3(newUp.x, newUp.y, -1);

        LineRenderer line = GetComponent<LineRenderer>();
        line.SetPositions(points);

        foreach (Health health in objectsToDealDamageTo)
        {
            health.healthCount -= HumanBossEnemy.laserBeamDamage * Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Health health = null;

        collision.TryGetComponent(out health);

        if (health)
        {
            if (objectsToDealDamageTo.Contains(health))
            {
                objectsToDealDamageTo.Remove(health);
            }
        }
    }
}
