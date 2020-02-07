using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDroneBay : Turret
{
    int droneCount = 0;
    public override void Attack()
    {
        if(droneCount < 2)
        {
            SpawnDrone();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDrone()
    {

    }
}
