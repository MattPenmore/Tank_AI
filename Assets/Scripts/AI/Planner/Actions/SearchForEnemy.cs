using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForEnemy : GAction
{
    public GameObject targ;
    public float costMultiplier;

    private void Start()
    {
        actionName = "Search For Enemy";
    }

    public override bool PrePerform()
    {
        //Go to last know position
        if(gameObject.GetComponent<TankSensors>().knownEnemyPosition != Vector3.zero)
        {
            targ.transform.position = gameObject.GetComponent<TankSensors>().knownEnemyPosition;
            target = targ;
            startTime = Time.time;
            return true;
        }
        return false;

    }

    public override bool PostPerform()
    {
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        //Cost dependant on time since enemy seen
        if (gameObject.GetComponent<TankSensors>().knownEnemyPosition != Vector3.zero && gameObject.GetComponent<TankSensors>().seeEnemyTime != 0)
        {
            cost = Mathf.Max((Time.time - gameObject.GetComponent<TankSensors>().seeEnemyTime), 1) * costMultiplier;
        }
        else
            cost = Mathf.Infinity;
    }
}
