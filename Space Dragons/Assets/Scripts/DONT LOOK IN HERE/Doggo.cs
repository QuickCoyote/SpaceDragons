using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doggo : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    Vector3 direction;
    void Start()
    {
        direction = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if(transform.position.x > 1530)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }

        if (transform.position.y > 770)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        if (transform.position.x < -1530)
        {
            transform.position = new Vector3(1920, transform.position.y, transform.position.z);
        }

        if (transform.position.y < -770)
        {
            transform.position = new Vector3(transform.position.x, 1080, transform.position.z);
        }
    }
}
