using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthLowBT : Node
{
    Agent agent;
    int healthThreshhold;
    public EnemyHealthLowBT(Agent ownerAgent) : base(ownerAgent)
    {
        agent = ownerAgent;
        healthThreshhold = ownerAgent.lowHealthThreshhold;
    }

    public override NODE_STATUS Update()
    {
        if (agent.lastKnownEnemyHealth <= healthThreshhold)
        {
            return NODE_STATUS.SUCCESS;
        }
        else return NODE_STATUS.FAILURE;
    }
}
