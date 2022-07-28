using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvadeBT : Node
{
    Vector3 evadeDirection;
    float evadeDistance = 10;

    public EvadeBT(Agent ownerAgent) : base(ownerAgent)
    {

    }

    public override NODE_STATUS Update()
    {
        //If not evaded recently
        if ((Time.time - ownerAgent.evasionDuration) > ownerAgent.evasionTime)
        {
            //Pick randomly between left and right
            int rand = Random.Range(0, 2);

            //Move horizontal to bullet direction
            if(rand == 0)
            {
                evadeDirection = new Vector3(ownerAgent.shotAtDirection.z, 0, - ownerAgent.shotAtDirection.x);
            }
            else
            {
                evadeDirection = new Vector3(-ownerAgent.shotAtDirection.z, 0, ownerAgent.shotAtDirection.x);
            }
            ownerAgent.navComp.destination = ownerAgent.transform.position + (evadeDirection * evadeDistance);
            ownerAgent.evasionTime = Time.time;

        }
        return NODE_STATUS.SUCCESS;
    }

}
