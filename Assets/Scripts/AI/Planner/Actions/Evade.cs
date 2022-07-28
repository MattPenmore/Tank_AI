using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Evade : GAction
{
    Vector3 evadeDirection;
    float evadeDistance = 10;
    public GameObject targ;

    private void Start()
    {
        actionName = "Evade";
    }

    public override bool PrePerform()
    {
        // Pick randomly between left and right
        int rand = Random.Range(0, 2);

        //Move horizontal to bullet direction
        if (rand == 0)
        {
            evadeDirection = new Vector3(gameObject.GetComponent<TankSensors>().shotAtDirection.z, 0, -transform.gameObject.GetComponent<TankSensors>().shotAtDirection.x).normalized;
        }
        else
        {
            evadeDirection = new Vector3(gameObject.GetComponent<TankSensors>().shotAtDirection.z, 0, transform.gameObject.GetComponent<TankSensors>().shotAtDirection.x).normalized;
        }

        if(evadeDirection == Vector3.zero)
        {
            evadeDirection = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
        }

        //Set target destination
        targetLocation = transform.position + (evadeDirection * evadeDistance);
        
        targ.transform.position = targetLocation;
        target = targ;

        startTime = Time.time;
        return true;

    }

    public override bool PostPerform()
    {
        return true;
    }
}
