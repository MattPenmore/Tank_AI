using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsTargetBT : Node
{
    float moveDistance = 0.5f;
    float moveTime = 0;
    float timeBetweenUpdates = 1f;
    float minDistance = 20;
    public MoveTowardsTargetBT(Agent ownerAgent) : base(ownerAgent)
    {

    }

    public override NODE_STATUS Update()
    {
        //If not too close
        if (Vector3.Distance(ownerAgent.transform.position, ownerAgent.enemyTarget.transform.position) > minDistance)
        {
            //If not moved towards recently
            if ((Time.time - timeBetweenUpdates) > moveTime)
            {
                //Set destination as point in direction of enemy
                Vector3 currentPosition = ownerAgent.transform.position;
                Vector3 enemyPosition = ownerAgent.lastKnownEnemyPosition;
                Vector3 moveDirection = (enemyPosition - currentPosition).normalized;

                ownerAgent.navComp.destination = currentPosition + moveDirection * moveDistance;
                moveTime = Time.time;
            }
        }

        return NODE_STATUS.SUCCESS;
    }
}
