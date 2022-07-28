using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : GAction
{
    [SerializeField]
    GameObject[] navigationPoints;
    List<GameObject> visitedPoints = new List<GameObject>();
    public GameObject targ;

    private void Start()
    {
        actionName = "Wander"; 
        navigationPoints = new GameObject[6];
        navigationPoints = GameObject.FindGameObjectsWithTag("NavigationPoint");
        targ.transform.parent = null;
    }

    public override bool PrePerform()
    {
        //Remove excess visited locations from log
        if (visitedPoints.Count > 1)
        {
            visitedPoints.RemoveAt(0);
        }
        //Pick a random navigation point and travel to it
        int rand = PickRand();
        targ.transform.position = navigationPoints[rand].transform.position;
        target = targ;
        visitedPoints.Add(navigationPoints[rand]);
        startTime = Time.time;
        return true;

    }

    public override bool PostPerform()
    {
        Debug.Log("Wandered");
        return true;
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
