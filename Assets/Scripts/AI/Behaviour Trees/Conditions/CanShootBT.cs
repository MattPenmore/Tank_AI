using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanShootBT : Node
{
    public CanShootBT(Agent ownerAgent) : base(ownerAgent)
    {

    }

    public override NODE_STATUS Update()
    {       
        if(Time.time - ownerAgent.m_TimeBetweenShots > ownerAgent.shootTime)
        {
            ownerAgent.canShoot = true;
        }
        else
        {
            ownerAgent.canShoot = false;
        }
        //Always return true to prevent going back up tree
        return NODE_STATUS.SUCCESS;
    }
}
