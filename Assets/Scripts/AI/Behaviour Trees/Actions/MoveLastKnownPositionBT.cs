using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveLastKnownPositionBT : Node
{
    float acceptanceDistance = 10;
    public MoveLastKnownPositionBT(Agent ownerAgent) : base(ownerAgent)
    {
        acceptanceDistance = 10;
    }

    public override NODE_STATUS Update()
    {
        //Move towards position of enemy that was seen last
        if(Vector3.Distance(ownerAgent.transform.position, ownerAgent.lastKnownEnemyPosition) > acceptanceDistance)
        {
            ownerAgent.navComp.destination = ownerAgent.lastKnownEnemyPosition;
        }

        return NODE_STATUS.SUCCESS;
    }
}
