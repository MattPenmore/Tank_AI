using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Composite
{
    public Sequence(Agent ownerAgent) : base(ownerAgent)
    {

    }

    public override NODE_STATUS Update()
    {
        //Set node status to fail by default
        NODE_STATUS returnStatus = NODE_STATUS.FAILURE;
        bool anyChildIsRunning = false;

        foreach (Node node in childNodes)
        {
            NODE_STATUS currentNodeStatus = node.Update();
            if (currentNodeStatus == NODE_STATUS.FAILURE)
            {
                returnStatus = NODE_STATUS.FAILURE;
                return returnStatus;
            }
            else if (currentNodeStatus == NODE_STATUS.SUCCESS)
                continue;
            else if(currentNodeStatus == NODE_STATUS.RUNNING)
            {
                anyChildIsRunning = true;
                continue;
            }
            else
            {
                returnStatus = NODE_STATUS.SUCCESS;
                return returnStatus;
            }
        }

        // If sequence itself has succeded or failed, reset it
        if (returnStatus == NODE_STATUS.SUCCESS || returnStatus == NODE_STATUS.FAILURE)
        {
            Reset();
        }
        returnStatus = anyChildIsRunning ? NODE_STATUS.RUNNING : NODE_STATUS.SUCCESS;
        return returnStatus;
    }
}
