using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    #region Variables
    public List<Transform> bodyPartTransforms = new List<Transform>();
    public List<GameObject> bodyPartObjects = new List<GameObject>();
    public List<GameObject> bodyPartPrefabs = null;

    public GameObject head = null;

    public Sprite[] ShipHeadSprites;
    public SpriteRenderer ShipHeadSprite = null;

    public float minDst = 1.0f;

    public int startSize = 2;

    public float speed = 1.0f;
    public float rotationSpeed = 50.0f;
    public int maxShipsAllowed = 4;

    private float dst = 1.0f;
    private Transform curBodyPart = null;
    private Transform prevBodyPart = null;

    #endregion

    private void Start()
    {
        ShipHeadSprite = GetComponentInChildren<SpriteRenderer>();
        PlayerPrefs.SetInt("PlayerHead", 0);
        SetShipHeadSprite(PlayerPrefs.GetInt("PlayerHead"));
        bodyPartObjects.Add(bodyPartTransforms[0].gameObject);
        AddBodyPart(FindBodyPartFromPrefabs("DefaultTurret"));
    }

    private void FixedUpdate()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddBodyPart(FindBodyPartFromPrefabs("DefaultTurret"));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RemoveBodyPart(bodyPartObjects[2], false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            RemoveBodyPart(bodyPartObjects[2], true);
            SortBody();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SortBody();
        }
    }

    public void Move()
    {
        float curSpeed = speed;

        if (Input.touchCount > 0)
        {
            // This is just getting touch
            Touch touch = Input.GetTouch(0);
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(touch.position);
            targetPos.z = 0;
            // This is just getting the angle from the head of the snake to the touched position, and rotating the head accordingly
            Vector3 direction = targetPos - bodyPartTransforms[0].transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            bodyPartTransforms[0].rotation = Quaternion.Slerp(bodyPartTransforms[0].rotation, rotation, rotationSpeed * Time.deltaTime);

            bodyPartTransforms[0].Translate(bodyPartTransforms[0].up * curSpeed * Time.smoothDeltaTime, Space.World);

            for (int i = 1; i < bodyPartTransforms.Count; i++)
            {
                if (bodyPartTransforms[i] != null)
                {

                    curBodyPart = bodyPartTransforms[i];
                    prevBodyPart = bodyPartTransforms[i - 1];

                    dst = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

                    Vector3 newPos = prevBodyPart.position;
                    newPos.z = bodyPartTransforms[0].position.z;

                    float t = Time.deltaTime * dst / minDst * curSpeed;

                    if (t > .5f)
                    {
                        t = 0.5f;
                    }

                    curBodyPart.position = Vector3.Slerp(curBodyPart.position, newPos, t);
                    curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, prevBodyPart.rotation, t);
                }
            }
        }
    }

    public void AddBodyPart(GameObject bodyPart)
    {
        Transform newPart = (Instantiate(bodyPart, bodyPartTransforms[bodyPartTransforms.Count - 1].position, bodyPartTransforms[bodyPartTransforms.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform);
        bodyPartTransforms.Add(newPart);
        bodyPartObjects.Add(newPart.gameObject);
    }

    public GameObject FindBodyPartFromPrefabs(string partName)
    {
        GameObject bPart = null;

        foreach (GameObject bodyPart in bodyPartPrefabs)
        {
            if (bodyPart.name.Equals("DefaultTurret"))
            {
                bPart = bodyPart;
                break;
            }
        }

        return bPart;
    }

    public void RemoveBodyPart(GameObject bodyPart, bool selling)
    {
        int removeIndex = 0;
        for (int i = 0; i < bodyPartObjects.Count; i++)
        {
            if (bodyPartObjects[i] == bodyPart)
            {
                removeIndex = i;
                Debug.Log(removeIndex);
            }
        }

        if (!selling)
        {
            if (removeIndex != 0)
            {
                int partCount = bodyPartObjects.Count;

                for (int j = partCount; j > removeIndex; j--)
                {
                    Destroy(bodyPartObjects[j-1]);
                    bodyPartObjects.RemoveAt(j-1);
                    bodyPartTransforms.RemoveAt(j-1);
                }
            }
            // return;
        }
        else
        {
            Destroy(bodyPartObjects[removeIndex]);
            bodyPartObjects.RemoveAt(removeIndex);
            bodyPartTransforms.RemoveAt(removeIndex);
        }
    }

    public void SetShipHeadSprite(int val)
    {
        ShipHeadSprite.sprite = ShipHeadSprites[val];
    }

    public void SortBody()
    {
        for (int i = 1; i < bodyPartObjects.Count; i++)
        {
            if (bodyPartObjects[i - 1] == null)
            {
                bodyPartObjects[i - 1] = bodyPartObjects[i];
                bodyPartObjects[i] = null;
            }
        }

        for (int i = 0; i < bodyPartObjects.Count; i++)
        {
            if (bodyPartObjects[i] != null)
            {
                bodyPartTransforms[i] = bodyPartObjects[i].transform;
            }
        }
    }
}
