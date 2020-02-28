using UnityEngine;

public class BorderLoop : MonoBehaviour
{
    public bool flipX = true;
    public bool flipY = true;
    [SerializeField] Animator TeleportTransition = null;
    Vector3 pos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        pos = collision.transform.position;
        if (flipX)
        {
            pos.x = -pos.x;
        }
        if (pos.x > 0)
        {
            pos.x -= 10.0f;
        }
        else
        {
            pos.y += 10.0f;
        }
        if (flipY)
        {
            pos.y = -pos.y;
        }
        if (pos.y > 0)
        {
            pos.y -= 10.0f;
        }
        else
        {
            pos.y += 10.0f;
        }


        if (collision.transform.tag == "Player")
        {
            WorldManager.Instance.SpawnWarpHole(collision.transform.position);
            TeleportTransition.SetTrigger("Warp");
        }
    }
    public void MovePlayer()
    {
        WorldManager.Instance.SpawnWarpHole(pos);
        foreach (Transform t in WorldManager.Instance.Ship.bodyPartTransforms)
        {
            t.position = pos;
        }
        AndroidManager.HapticFeedback();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        pos = collision.transform.position;
        if (flipX)
        {
            pos.x = -pos.x;
        }
        if (pos.x > 0)
        {
            pos.x -= 10.0f;
            pos.y += 10.0f;
        }
        if (flipY)
        {
            pos.y = -pos.y;
        }
        if (pos.y > 0)
        {
            pos.y -= 10.0f;
        }
        else
        {
            pos.y += 10.0f;
        }

        if (collision.transform.tag != "Player" && collision.gameObject.layer != 8 && collision.gameObject.layer != 11) //dont do turrets or snake
        {
            WorldManager.Instance.SpawnWarpHole(pos);
            WorldManager.Instance.SpawnWarpHole(collision.transform.position);
            collision.transform.position = pos;
        }
    }
}
