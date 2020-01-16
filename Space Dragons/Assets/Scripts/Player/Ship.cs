using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ship : MonoBehaviour
{
    #region Variables
    public Queue<Transform> bodyPartTransforms = new Queue<Transform>();
    public Queue<GameObject> bodyPartObjects = new Queue<GameObject>();
    public List<GameObject> bodyPartPrefabs = null;

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
        bodyPartObjects.Enqueue(bodyPartTransforms.Peek().gameObject);
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
            RemoveBodyPart(bodyPartObjects.ElementAt(2), false);
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
            Vector3 direction = targetPos - bodyPartTransforms.ElementAt(0).transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            bodyPartTransforms.ElementAt(0).rotation = Quaternion.Slerp(bodyPartTransforms.Peek().rotation, rotation, rotationSpeed * Time.deltaTime);

            bodyPartTransforms.Peek().Translate(bodyPartTransforms.Peek().up * curSpeed * Time.smoothDeltaTime, Space.World);

            for (int i = 1; i < bodyPartTransforms.Count; i++)
            {
                curBodyPart = bodyPartTransforms.ElementAt(i);
                prevBodyPart = bodyPartTransforms.ElementAt(i - 1);

                dst = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

                Vector3 newPos = prevBodyPart.position;
                newPos.z = bodyPartTransforms.Peek().position.z;

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

    public void AddBodyPart(GameObject bodyPart)
    {
        List<Transform> BodyPartTransformList = bodyPartTransforms.ToList();
        Transform newPart = (Instantiate(bodyPart, BodyPartTransformList[bodyPartTransforms.Count - 1].position, BodyPartTransformList[bodyPartTransforms.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform);
        bodyPartTransforms.Enqueue(newPart);
        bodyPartObjects.Enqueue(newPart.gameObject);
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
        List<Transform> BodyPartTransformList = bodyPartTransforms.ToList();
        List<GameObject> BodyPartObjectList = bodyPartObjects.ToList();

        int removeIndex = 0;
        for (int i = 0; i < bodyPartObjects.Count; i++)
        {
            if (bodyPartObjects.ElementAt(i) == bodyPart)
            {
                removeIndex = i;
            }
        }

        if (!selling)
        {
            if (removeIndex != 0)
            {
                int partCount = bodyPartObjects.Count - 1;

                for (int j = partCount; j > removeIndex; j--)
                {
                    Destroy(bodyPartObjects.ElementAt(j));
                    BodyPartObjectList.RemoveAt(j);
                    BodyPartTransformList.RemoveAt(j);
                }
            }
        }
        else if (removeIndex != 0)
        {
            Destroy(bodyPartObjects.ElementAt(removeIndex));
            BodyPartObjectList.RemoveAt(removeIndex);
            BodyPartTransformList.RemoveAt(removeIndex);
        }
        bodyPartObjects = new Queue<GameObject>(BodyPartObjectList);
        bodyPartTransforms = new Queue<Transform>(BodyPartTransformList);
    }

    public void SetShipHeadSprite(int val)
    {
        ShipHeadSprite.sprite = ShipHeadSprites[val];
    }
}
