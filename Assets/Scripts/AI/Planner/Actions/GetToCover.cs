using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetToCover : GAction
{
    public GameObject targ;
    public float costMultiplier;
    public float coverDistanceCostMultiplier;
    public float enemyDistanceCostMultiplier;

    private void Start()
    {
        actionName = "GetToCover";
    }
    public override bool PrePerform()
    {
        // Move towards known cover location
        targ.transform.position = gameObject.GetComponent<TankSensors>().coverLocation.transform.position;
        target = targ;
        startTime = Time.time;
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        // Cost dependant on distance to cover and distance from enemy
        if (gameObject.GetComponent<TankSensors>().enemy)
        {
            cost = Mathf.Max((gameObject.GetComponent<TankSensors>().coverDistance * coverDistanceCostMultiplier - gameObject.GetComponent<TankSensors>().enemyDistance * enemyDistanceCostMultiplier) * costMultiplier, 1);
        }
        else cost = Mathf.Infinity;
    }
}
