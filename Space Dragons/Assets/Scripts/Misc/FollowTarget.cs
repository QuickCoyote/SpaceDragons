using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0.0f, 0.0f, -10.0f);
    [SerializeField] public  GameObject Target = null;

    Ship targetS = null;

    public void Start()
    {
        Target.TryGetComponent(out targetS);
    }

    public void FixedUpdate()
    {        

        if (targetS)
        {
            transform.Translate(targetS.bodyPartTransforms[0].up * targetS.speed * Time.smoothDeltaTime);
        }
        else
        {
            transform.position = Target.transform.position + offset;
        }
    }
}
