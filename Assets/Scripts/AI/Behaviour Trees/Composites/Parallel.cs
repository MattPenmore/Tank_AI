using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Composite
{

    public Parallel(Agent ownerAgent) : base(ownerAgent)
    {

    }

    public override NODE_STATUS Update()
    {
        bool anyChildIsRunning = false;
        int nFailedChildren = 0;
        //Set node status to fail by default
        NODE_STATUS returnStatus = NODE_STATUS.FAILURE;
        foreach (Node node in childNodes)
        {
            NODE_STATUS currentNodeStatus = node.Update();


            if (currentNodeStatus == NODE_STATUS.FAILURE)
            {
                nFailedChildren++;
                continue;
            }
            else if (currentNodeStatus == NODE_STATUS.SUCCESS)
            {
                continue;
            }
            else if (currentNodeStatus == NODE_STATUS.RUNNING)
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

        //If all children fail, return failure, otherwise return success, or running if not finished
        if(anyChildIsRunning)
        {
            returnStatus = NODE_STATUS.RUNNING;
        }
        else if (nFailedChildren == childNodes.Count)
        {
            returnStatus = NODE_STATUS.FAILURE;
        }
        else
        {
            returnStatus = NODE_STATUS.SUCCESS;
        }

        if (returnStatus == NODE_STATUS.SUCCESS || returnStatus == NODE_STATUS.FAILURE)
        {
            Reset();
        }
        return returnStatus;
    }
}
