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
            WarpHole warp = WorldManager.Instance.SpawnFromPool(WorldManager.ePoolTag.WARPHOLE, collision.transform.position, Quaternion.identity).GetComponent<WarpHole>();
            if (warp) warp.Activate();
            TeleportTransition.SetTrigger("Warp");
        }
    }
    public void MovePlayer()
    {
        WarpHole warp = WorldManager.Instance.SpawnFromPool(WorldManager.ePoolTag.WARPHOLE, pos, Quaternion.identity).GetComponent<WarpHole>();
        if (warp) warp.Activate();
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
            WarpHole warp = WorldManager.Instance.SpawnFromPool(WorldManager.ePoolTag.WARPHOLE, pos, Quaternion.identity).GetComponent<WarpHole>();
            if (warp) warp.Activate();
            WarpHole warp2 = WorldManager.Instance.SpawnFromPool(WorldManager.ePoolTag.WARPHOLE, collision.transform.position, Quaternion.identity).GetComponent<WarpHole>();
            if (warp2) warp2.Activate();
            collision.transform.position = pos;
        }
    }
}
