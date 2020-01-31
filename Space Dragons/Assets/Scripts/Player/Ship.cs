﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    #region Enums
    public enum eShipToTest
    {
        RUSTY,
        LIGHTNING,
        FLAME,
        HEALER,
        ATTACK_DRONE,
    }

    public enum eMotherShip
    {
        BASIC,
        FLAMETHROWER,
        LIGHTNING,
        HEALING,
        GUARD_DRONE,
        LASER
    }
    #endregion

    #region Variables

    public Camera cam = null;
    public GameObject DeathPanel = null;

    [Header("Pre-Play Info")]
    public List<Transform> bodyPartTransforms = new List<Transform>();
    public List<GameObject> bodyPartObjects = new List<GameObject>();
    public List<GameObject> bodyPartPrefabs = null;

    [Header("Head Info")]
    public GameObject head = null;
    public Sprite[] ShipHeadSprites;
    public SpriteRenderer ShipHeadSprite = null;

    [Header("Snake Info")]
    public float minDst = 1.0f;
    public int startSize = 2;
    public int maxShipsAllowed = 4;

    [Header("Movement Info")]
    public float speed = 1.0f;
    public float rotationSpeed = 50.0f;

    [Header("Enum Info")]
    public eMotherShip motherShip = eMotherShip.BASIC;
    public eShipToTest shipToTest = eShipToTest.RUSTY;

    private float dst = 1.0f;
    private Transform curBodyPart = null;
    private Transform prevBodyPart = null;

    #endregion
    
    private void Start()
    {
        ShipHeadSprite = GetComponentInChildren<SpriteRenderer>();
        
        bodyPartObjects.Add(bodyPartTransforms[0].gameObject);

        switch (motherShip)
        {
            case eMotherShip.BASIC:
                SetShipHead(0);
                break;
            case eMotherShip.FLAMETHROWER:
                SetShipHead(1);
                break;
            case eMotherShip.LIGHTNING:
                SetShipHead(2);
                break;
            case eMotherShip.HEALING:
                SetShipHead(3);
                break;
            case eMotherShip.GUARD_DRONE:
                SetShipHead(4);
                break;
        }


        switch (shipToTest)
        {
            case eShipToTest.RUSTY:
                AddBodyPart(FindBodyPartFromPrefabs("RustyPrefab"));
                break;
            case eShipToTest.LIGHTNING:
                AddBodyPart(FindBodyPartFromPrefabs("ShockPrefab"));
                break;
            case eShipToTest.FLAME:
                AddBodyPart(FindBodyPartFromPrefabs("FlamePrefab"));
                break;
            case eShipToTest.HEALER:
                AddBodyPart(FindBodyPartFromPrefabs("HealerPrefab"));
                break;
            case eShipToTest.ATTACK_DRONE:
                AddBodyPart(FindBodyPartFromPrefabs("AttackDronePrefab"));
                break;
        }
    }

    private void FixedUpdate()
    {
        Move();

        CheckForDie();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddBodyPart(FindBodyPartFromPrefabs("ShockPrefab"));
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
            if (!UIDetectionManager.Instance.IsPointerOverUIObject())
            {
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(touch.position);
                targetPos.z = 0;
                // This is just getting the angle from the head of the snake to the touched position, and rotating the head accordingly
                Vector3 direction = targetPos - bodyPartTransforms[0].transform.position;
                float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
                bodyPartTransforms[0].rotation = Quaternion.Slerp(bodyPartTransforms[0].rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }

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

    public void AddBodyPart(GameObject bodyPart)
    {
        Transform newPart = (Instantiate(bodyPart, bodyPartTransforms[bodyPartTransforms.Count - 1].position, bodyPartTransforms[bodyPartTransforms.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform);
        bodyPartTransforms.Add(newPart);
        bodyPartObjects.Add(newPart.gameObject);

        HealthBarManager.Instance.AddHealthBar();
    }

    public GameObject FindBodyPartFromPrefabs(string partName)
    {
        GameObject bPart = null;

        foreach (GameObject bodyPart in bodyPartPrefabs)
        {
            if (bodyPart.name.Equals(partName))
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
            }
        }

        if (!selling)
        {
            if (removeIndex != 0)
            {
                int partCount = bodyPartObjects.Count;

                for (int j = partCount; j > removeIndex; j--)
                {
                    Destroy(bodyPartObjects[j - 1]);
                    bodyPartObjects.RemoveAt(j - 1);
                    bodyPartTransforms.RemoveAt(j - 1);
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

    public void SetShipHead(int val)
    {
        PlayerController playerController = GetComponent<PlayerController>();
        playerController.SwitchFireMode(motherShip);
        playerController.headBullet = playerController.headBullets[val];
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

    public void CheckForDie()
    {
        if (head.GetComponent<Health>().healthCount <= 0)
        {
            Time.timeScale = 0;
            DeathPanel.SetActive(true);
        }
    }
}
