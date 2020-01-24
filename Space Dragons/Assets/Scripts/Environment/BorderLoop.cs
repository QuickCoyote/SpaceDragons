using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderLoop : MonoBehaviour
{
    public bool flipX = true;
    public bool flipY = true;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 pos = collision.transform.position;
        if (flipX) pos.x = -pos.x;
        if (pos.x > 0) pos.x -= 10.0f; pos.y += 10.0f;
        if (flipY) pos.y = -pos.y;
        if (pos.y > 0) pos.y -= 10.0f; else pos.y += 10.0f;

        if (collision.collider.name == "Player Head")
        {
            collision.transform.position = pos;

            foreach (Transform t in WorldManager.Instance.Ship.bodyPartTransforms)
            {
                t.Translate(pos, Space.World);
            }
            foreach (GameObject g in WorldManager.Instance.Ship.bodyPartObjects)
            {
                g.transform.Translate(pos, Space.World);
            }
        }
        else
        {
            collision.transform.Translate(pos);
        }
    }
}
