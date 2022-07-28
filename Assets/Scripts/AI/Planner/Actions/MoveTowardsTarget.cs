using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsTarget : GAction
{
    float moveDistance = 6f;
    public GameObject targ;
    TankSensors sensors;

    public float baseCost;
    public float costMultiplier;

    private void Start()
    {
        actionName = "TowardsTarget";
        sensors = gameObject.GetComponent<TankSensors>();
    }

    public override bool PrePerform()
    {
        Vector3 currentPosition = transform.position;
        Vector3 enemyPosition = sensors.knownEnemyPosition;
        Vector3 moveDirection = (enemyPosition - currentPosition).normalized;
        targ.transform.position = currentPosition + moveDirection * moveDistance;
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
        // Cost dependant on distance from enemy
        if(gameObject.GetComponent<TankSensors>().enemy)
        {
            cost = Mathf.Max((baseCost - sensors.enemyDistance) * costMultiplier, 1);
        }
        else
        {
            cost = Mathf.Infinity;
        }
    }
}
