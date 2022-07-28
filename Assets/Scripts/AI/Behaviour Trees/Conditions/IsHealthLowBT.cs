using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHealthLowBT : Node
{
    Agent agent;
    int healthThreshhold;
    public IsHealthLowBT(Agent ownerAgent) : base(ownerAgent)
    {
        agent = ownerAgent;
        healthThreshhold = ownerAgent.lowHealthThreshhold;
    }

    public override NODE_STATUS Update()
    {
        if (agent.transform.GetComponent<TankHealth>().m_CurrentHealth <= healthThreshhold)
        {
            return NODE_STATUS.SUCCESS;
        }
        else return NODE_STATUS.FAILURE;
    }
}
