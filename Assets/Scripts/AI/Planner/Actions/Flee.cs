using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : GAction
{
    public GameObject targ;
    float fleeDistance = 6;
    public float costMultiplier;

    private void Start()
    {
        actionName = "Flee";
    }

    public override bool PrePerform()
    {
        //Move in opposite direction to Enemy
        Vector3 currentPosition = transform.position;
        Vector3 enemyPosition = gameObject.GetComponent<TankSensors>().knownEnemyPosition;
        Vector3 fleeDirection = (currentPosition - enemyPosition).normalized;
        //Add variance so not directly away
        Vector3 variance = Quaternion.AngleAxis(90, Vector3.up) * fleeDirection * Random.Range(-fleeDistance, fleeDistance);
        //Set known time so can continue to flee enemy
        gameObject.GetComponent<TankSensors>().seeEnemyTime = Time.time;

        //Set target destination
        targ.transform.position = currentPosition + fleeDirection * fleeDistance + variance;
        target = targ;
        startTime = Time.time;
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }

    private void Update()
    {
        //Cost dependant on distance from enemy
        if (gameObject.GetComponent<TankSensors>().enemy)
        {
            cost = Vector3.Distance(transform.position, gameObject.GetComponent<TankSensors>().enemy.transform.position) * costMultiplier;
        }
        else cost = Mathf.Infinity;
    }
}
