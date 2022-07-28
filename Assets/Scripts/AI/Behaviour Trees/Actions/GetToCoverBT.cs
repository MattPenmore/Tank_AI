using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetToCoverBT : Node
{
    public GetToCoverBT(Agent ownerAgent) : base(ownerAgent)
    {

    }

    public override NODE_STATUS Update()
    {
        //Go to known cover location
        if(ownerAgent.coverLocation != null)
        {
            ownerAgent.navComp.destination = ownerAgent.coverLocation.transform.position;

            return NODE_STATUS.SUCCESS;
        }
        return NODE_STATUS.FAILURE;
    }

}
