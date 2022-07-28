using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeEnemyBT : Node
{
    public AISightCone sightCone;
    float variaceAccountability = 0.1f;

    public SeeEnemyBT(Agent ownerAgent) : base(ownerAgent)
    {
        sightCone = ownerAgent.GetComponentInChildren<AISightCone>();
        
    }

    public override NODE_STATUS Update()
    {
        //Check if tank can see an enemy
        for(int i = 0; i < sightCone.GetComponent<AISightCone>().inSightObjects.Count; i++)
        {
            if(sightCone.inSightObjects[i].CompareTag("PlannerTank"))
            {
                //Set known position of enemy
                //Set distance to enemy
                //Set time enemy is seen
                ownerAgent.lastKnownEnemyPosition = sightCone.GetComponent<AISightCone>().inSightObjects[i].transform.position;
                ownerAgent.enemyTarget = sightCone.GetComponent<AISightCone>().inSightObjects[i];
                ownerAgent.distanceToEnemy = Vector3.Distance(ownerAgent.transform.position, ownerAgent.enemyTarget.transform.position);
                ownerAgent.EnemyPositionKnownTime = Time.time;
                return NODE_STATUS.SUCCESS;
            }
            
        }
        //Account for random issues. Count as seeing if seen in last 0.1 seconds.
        if ((Time.time - variaceAccountability) > ownerAgent.EnemyPositionKnownTime && ownerAgent.enemyTarget)
        {
            ownerAgent.lastKnownEnemyPosition = ownerAgent.enemyTarget.transform.position;
            ownerAgent.distanceToEnemy = Vector3.Distance(ownerAgent.transform.position, ownerAgent.enemyTarget.transform.position);
            return NODE_STATUS.SUCCESS;
        }

        return NODE_STATUS.FAILURE;
    }
}
