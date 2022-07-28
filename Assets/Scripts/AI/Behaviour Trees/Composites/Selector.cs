using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite
{
    public Selector(Agent ownerAgent) : base(ownerAgent)
    {

    }

    public override NODE_STATUS Update()
    {
        //Set node status to fail by default
        NODE_STATUS returnStatus = NODE_STATUS.FAILURE;
        foreach (Node node in childNodes)
        {
            NODE_STATUS currentNodeStatus = node.Update();

            if (currentNodeStatus == NODE_STATUS.FAILURE)
                    continue;
            else if (currentNodeStatus == NODE_STATUS.SUCCESS)
            {
                returnStatus = NODE_STATUS.SUCCESS;
                return returnStatus;
            }
            else if(currentNodeStatus == NODE_STATUS.RUNNING)
            {
                returnStatus = NODE_STATUS.RUNNING;
                return returnStatus;
            }
            else
            {
                continue;
            }
        }
            // If sequence itself has succeded or failed, reset it
        if (returnStatus == NODE_STATUS.SUCCESS || returnStatus == NODE_STATUS.FAILURE)
        {
            Reset();
        }

        return returnStatus;

    }
}
