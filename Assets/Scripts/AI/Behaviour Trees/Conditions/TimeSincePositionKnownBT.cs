using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSincePositionKnownBT : Node
{
    float exactPosiitonTime = 5;
    float acceptanceTime = 10;

    public TimeSincePositionKnownBT(Agent ownerAgent) : base(ownerAgent)
    {
        acceptanceTime = 10;
    }

    public override NODE_STATUS Update()
    {
        if(ownerAgent.EnemyPositionKnownTime != 0)
        {
            //Get exact position of enemy if seen within last 5 seconds.
            if(Time.time - exactPosiitonTime < ownerAgent.EnemyPositionKnownTime)
            {
                ownerAgent.lastKnownEnemyPosition = ownerAgent.enemyTarget.transform.position;
                return NODE_STATUS.SUCCESS;
            }
            else if(Time.time - acceptanceTime < ownerAgent.EnemyPositionKnownTime)
            {
                return NODE_STATUS.SUCCESS;
            }
        }
        return NODE_STATUS.FAILURE;
    }
}
