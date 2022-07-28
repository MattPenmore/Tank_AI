using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasAmmoBT : Node
{
    Agent agent;
    public HasAmmoBT(Agent ownerAgent) : base(ownerAgent)
    {
        agent = ownerAgent;
    }

    public override NODE_STATUS Update()
    {
        if (agent.m_ShotsUntilReload >= 1)
        {
            return NODE_STATUS.SUCCESS;
        }
        else return NODE_STATUS.FAILURE;
    }
}
