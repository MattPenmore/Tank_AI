using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenByEnemyBT : Node
{
    public SeenByEnemyBT(Agent ownerAgent) : base(ownerAgent)
    {

    }
    //Only Place this script in area of behaviour tree where AI can see the enemy
    public override NODE_STATUS Update()
    {
        AISightCone enemySight = ownerAgent.enemyTarget.GetComponentInChildren<AISightCone>();
        if(enemySight.inSightObjects.Contains(ownerAgent.gameObject))
        {
            return NODE_STATUS.SUCCESS;
        }

        return NODE_STATUS.FAILURE;
    }
}
