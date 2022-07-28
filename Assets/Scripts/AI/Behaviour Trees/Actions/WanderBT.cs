using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderBT : Node
{
    GameObject[] navigationPoints;
    List<GameObject> visitedPoints;
    float acceptanceDistance = 10;


    public WanderBT(Agent ownerAgent) : base(ownerAgent)
    {
        visitedPoints = new List<GameObject>();
        navigationPoints = new GameObject[6];
        navigationPoints = GameObject.FindGameObjectsWithTag("NavigationPoint");
    }

    public override NODE_STATUS Update()
    {
        //Remove excess visited locations from log
        if (visitedPoints.Count > 1)
        {
            visitedPoints.RemoveAt(0);
        }

        //If close enough to previous target
        if (Vector3.Distance(ownerAgent.navComp.destination, ownerAgent.transform.position) < acceptanceDistance) 
        {
            //Pick a random navigation point and travel to it
            int rand = PickRand();
            ownerAgent.navComp.destination = navigationPoints[rand].transform.position;

            visitedPoints.Add(navigationPoints[rand]);

            return NODE_STATUS.SUCCESS;
        }
        else
        {
            //Keep travelling to previous destination
            ownerAgent.navComp.destination = ownerAgent.navComp.destination;

            return NODE_STATUS.FAILURE;
        }
        
    }


    int PickRand()
    {
        //Prevent travelling to previously travelled location
        int rand = Random.Range(0, navigationPoints.Length);
        if (visitedPoints.Contains(navigationPoints[rand]))
        {
            rand = PickRand();
        }

        return rand;
    }
}
