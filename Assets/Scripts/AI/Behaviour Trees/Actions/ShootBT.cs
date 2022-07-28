using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ShootBT : Node
{

    public ShootingUpdateBT shootingUpdate;

    public ShootBT(Agent ownerAgent) : base(ownerAgent)
    {
        shootingUpdate = ownerAgent.gameObject.GetComponent<ShootingUpdateBT>();
    }

    public override NODE_STATUS Update()
    {
        //To ensure it is called every update, without affecting calling rest of tree, Shoot occurs in a update function on a script on the tank.

        //Tell script to start shooting if possible
        if(ownerAgent.canShoot == true && !shootingUpdate.isShooting)
        {
            shootingUpdate.isShooting = true;
        }
            return NODE_STATUS.SUCCESS;

    }
}


