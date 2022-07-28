using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeBT : Node
{
    float fleeDistance = 2;
    float fleeTime = 0;
    float timeBetweenUpdates = 1;
    public FleeBT(Agent ownerAgent) : base(ownerAgent)
    {
        
    }

    public override NODE_STATUS Update()
    {
        //If not fleed recently
        if ((Time.time - timeBetweenUpdates) > fleeTime)
        {
            //Move in opposite direction to Enemy
            Vector3 currentPosition = ownerAgent.transform.position;
            Vector3 enemyPosition = ownerAgent.lastKnownEnemyPosition;
            Vector3 fleeDirection = (currentPosition - enemyPosition).normalized;
            //Add variance so not directly away
            Vector3 variance = Quaternion.AngleAxis(90, Vector3.up) * fleeDirection * Random.Range(-fleeDistance,fleeDistance);
            //Set known time so can continue to see enemy
            ownerAgent.EnemyPositionKnownTime = Time.time;
            ownerAgent.navComp.destination = currentPosition + fleeDirection * fleeDistance + variance;

            fleeTime = Time.time;
        }

        return NODE_STATUS.SUCCESS;
    }
}
