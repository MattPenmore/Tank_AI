using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooCloseToEnemyBT : Node
{
    float range;
    public TooCloseToEnemyBT(Agent ownerAgent) : base(ownerAgent)
    {
        range = ownerAgent.GetComponent<Agent>().enemyTooCloseRange;
    }

    public override NODE_STATUS Update()
    {
        if (Vector3.Distance(ownerAgent.transform.position, ownerAgent.enemyTarget.transform.position) < range)
        {
            return NODE_STATUS.SUCCESS;
        }
        else return NODE_STATUS.FAILURE;
    }
}
