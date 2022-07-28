using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoFullBT : Node
{
    public AmmoFullBT(Agent ownerAgent) : base(ownerAgent)
    {

    }

    public override NODE_STATUS Update()
    {
        if (ownerAgent.m_ShotsUntilReload == ownerAgent.m_ShotsBeforeReload)
        {
            return NODE_STATUS.SUCCESS;
        }
        else return NODE_STATUS.FAILURE;
    }
}
