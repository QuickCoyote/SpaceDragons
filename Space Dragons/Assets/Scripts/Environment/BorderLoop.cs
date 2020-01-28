using UnityEngine;

public class BorderLoop : MonoBehaviour
{
    public bool flipX = true;
    public bool flipY = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 pos = collision.transform.position;
        if (flipX) pos.x = -pos.x;
        if (pos.x > 0) pos.x -= 10.0f; pos.y += 10.0f;
        if (flipY) pos.y = -pos.y;
        if (pos.y > 0) pos.y -= 10.0f; else pos.y += 10.0f;

        if (collision.transform.tag == "Player")
        {
            //Debug.Log("Collided at:" + collision.transform.position);
            //  collision.transform.position = pos;
            //   Debug.Log("Sent to:" + collision.transform.position);
            WorldManager.Instance.SpawnWarpHole(pos);

            foreach (Transform t in WorldManager.Instance.Ship.bodyPartTransforms)
            {
                t.position = pos;
            }
            //foreach (GameObject g in WorldManager.Instance.Ship.bodyPartObjects)
            //{
            //    g.transform.position = pos;
            //}
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 pos = collision.transform.position;
        if (flipX) pos.x = -pos.x;
        if (pos.x > 0) pos.x -= 10.0f; pos.y += 10.0f;
        if (flipY) pos.y = -pos.y;
        if (pos.y > 0) pos.y -= 10.0f; else pos.y += 10.0f;

        if (collision.transform.tag != "Player")
        {
            WorldManager.Instance.SpawnWarpHole(pos);
            //  Debug.Log("Collided at:" + collision.transform.position);
            collision.transform.position = pos;
          //  Debug.Log("Sent to:" + collision.transform.position);
        }
    }
}
