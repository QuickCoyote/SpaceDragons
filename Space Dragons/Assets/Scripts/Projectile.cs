using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletSpeed = 0.01f;
    public float lifetime = 1.0f;

    public GameObject parent = null;

    private float timer = 0.0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
