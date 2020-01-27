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
        GetComponent<Rigidbody2D>().AddForce(direction * speed * Time.deltaTime);

        if(transform.position.x > Screen.width)
        {
            transform.position = new Vector3(-1529, transform.position.y, 0);
        }

        if (transform.position.y > Screen.height)
        {
            transform.position = new Vector3(transform.position.x, -769, 0);
        }

        if (transform.position.x < -Screen.width)
        {
            transform.position = new Vector3(1529, transform.position.y, 0);
        }

        if (transform.position.y < -Screen.height)
        {
            transform.position = new Vector3(transform.position.x, 769, 0);
        }
    }
}
